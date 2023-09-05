using Code.Model;
using Code.Shared;
using Code.UI;
using Cysharp.Threading.Tasks;
using Game;
using Game.Model.Popups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private SpawnerConfig spawnerConfig;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;
        [SerializeField] private Transform doorSpawnPoint;

        private ObjectPool enemyPool;
        private Player player;
        private float lastTimeSpawnedEnemy;

        private void Start()
        {
            Debug.Log("Start gameplay called.");

            SpawnPlayer();
            SpawnDoor();

            var topBar = Code.Game.Game.Get<PopupManager>().Get<TopBar>();
            topBar.Setup(player.Model);

            if (!Code.Game.Game.Get<PopupManager>().IsOpen<TopBar>())
            {
                Code.Game.Game.Get<PopupManager>().Open<TopBar>().Forget();
            }

            ReloadScene();
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (player.Model.IsDead()) // Проверьте, нужно ли вам изменить или удалить эту проверку в связи с изменениями в механике смерти игрока
            {
                return;
            }

            if (lastTimeSpawnedEnemy + spawnerConfig.SpawnDelay <= Time.timeSinceLevelLoad)
            {
                lastTimeSpawnedEnemy = Time.timeSinceLevelLoad;
                SpawnEnemy();
            }
        }

        private void ReloadScene()
        {
            Debug.Log("Scene reload called.");

            Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.ReturnAllObjectsToPool();
            Code.Game.Game.Get<ObjectPoolsController>().BulletPool.ReturnAllObjectsToPool();

            player.transform.position = playerSpawnPoint.position;
        }

        private void SpawnPlayer()
        {
            Debug.Log("Spawn player called.");

            var playerGo = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, transform);
            player = playerGo.GetComponent<Player>();
            player.Setup(new PlayerModel(playerConfig));

            player.onDie -= HandlePlayerDeath;
            player.onDie += HandlePlayerDeath;
        }

        private void SpawnDoor()
        {
            Debug.Log("Spawn door called.");

            var doorGo = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity, transform);
            
        }

        private void SpawnEnemy()
        {
            Debug.Log("Spawn enemy called.");

            var g = Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.GetObject();
            g.transform.position = enemySpawnPoint.transform.position +
                                   new Vector3(
                                       Random.Range(spawnerConfig.SpawnPosition.x, spawnerConfig.SpawnPosition.y),
                                       0, 0);
            var enemy = g.GetComponent<Enemy>();
            enemy.Setup(new EnemyModel(enemyConfig));

            enemy.onInfected -= HandleEnemyInfection; // новый обработчик событий заражения
            enemy.onInfected += HandleEnemyInfection; // новый обработчик событий заражения
        }

        private void HandleEnemyInfection(Enemy obj) // новый обработчик событий заражения
        {
            obj.ChangeColor(Color.yellow); // меняем цвет на желтый
            obj.Explode(); // взрываем врага
            Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.ReturnObjectToPool(obj.gameObject); // возвращаем объект в пул
        }

        private void HandlePlayerDeath(Player playerModel)
        {
            var endPopup = Code.Game.Game.Get<PopupManager>().Get<EndPopup>();
            endPopup.Setup(player.Model);
            Code.Game.Game.Get<PopupManager>().Open<EndPopup>().Forget();
            Time.timeScale = 0;
        }
    }
}