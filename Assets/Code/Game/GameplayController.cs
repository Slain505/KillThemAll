using System.Collections.Generic;
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
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;
        [SerializeField] private Transform doorSpawnPoint;

        private ObjectPool enemyPool;
        private Player player;
        private Door door;
        private int currentEnemyCount = 0;
        private int maxEnemyCount = 20;
        private int minEnemyCount = 5;

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

            if (maxEnemyCount >= currentEnemyCount)
            {
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
            playerGo.transform.localScale = new Vector3(playerConfig.InitialSize, playerConfig.InitialSize, playerConfig.InitialSize);
            player = playerGo.GetComponent<Player>();
            player.Setup(new PlayerModel(playerConfig));

            player.onDie -= HandlePlayerDeath;
            player.onDie += HandlePlayerDeath;
        }

        private void SpawnDoor()
        {
            Debug.Log("Spawn door called.");

            var doorGo = Instantiate(doorPrefab, doorSpawnPoint.position, Quaternion.identity, transform);
            door = doorGo.GetComponent<Door>();
        }

        private void SpawnEnemy()
        {
            Debug.Log("Spawn enemy called.");

            var randomEnemyCount = Random.Range(minEnemyCount, maxEnemyCount);
            currentEnemyCount += randomEnemyCount;

            float spawnAreaWidth = 10.0f; 
            float enemySpacing = 3.0f; // Увеличиваем значение, чтобы предотвратить перекрытие

            List<Vector3> occupiedPositions = new List<Vector3>();

            for (int i = 0; i < randomEnemyCount; i++)
            {
                var g = Game.Get<ObjectPoolsController>().EnemyPool.GetObject();

                Vector3 spawnPosition;
                int attemptCount = 0;

                do
                {
                    float xPositionOffset = Random.Range(0, spawnAreaWidth - enemySpacing);
                    float zPositionOffset = Random.Range(0, spawnAreaWidth - enemySpacing);

                    spawnPosition = enemySpawnPoint.transform.position + new Vector3(xPositionOffset, 0, zPositionOffset);
                    attemptCount++;

                    if (attemptCount > 500)
                    {
                        Debug.LogWarning("Could not find a suitable spawn position");
                        break;
                    }
                }
                while (IsPositionOccupied(spawnPosition, occupiedPositions, enemySpacing));

                if (attemptCount <= 500)
                {
                    occupiedPositions.Add(spawnPosition);

                    g.transform.position = spawnPosition;
                    var enemy = g.GetComponent<Enemy>();
                    enemy.Setup(new EnemyModel(enemyConfig));
                    //g.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                else
                {
                    Debug.LogWarning("Max attempts reached, enemy not spawned");
                }
            }
        }

        private bool IsPositionOccupied(Vector3 position, List<Vector3> occupiedPositions, float spacing)
        {
            foreach (var occupiedPosition in occupiedPositions)
            {
                if (Vector3.Distance(position, occupiedPosition) < spacing)
                {
                    return true;
                }
            }
            return false;
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