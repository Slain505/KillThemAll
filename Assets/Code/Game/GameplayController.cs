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
    /// <summary>
    /// Controls the main gameplay loop, spawning and managing game entities.
    /// </summary>
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private SpawnerConfig spawnerConfig;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;

        private ObjectPool enemyPool;
        private Player player;
        private float lastTimeSpawnedEnemy;
        
        private void Start()
        {
            Debug.Log("Start gameplay called.");
            
            SpawnPlayer();
            
            var topBar = Code.Game.Game.Get<PopupManager>().Get<TopBar>();
            topBar.Setup(player.Model);

            if(!Code.Game.Game.Get<PopupManager>().IsOpen<TopBar>())
            {
                Code.Game.Game.Get<PopupManager>().Open<TopBar>().Forget();
            }
            
            ReloadScene();
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (player.Model.IsDead())
            {
                return;
            }

            if (lastTimeSpawnedEnemy + spawnerConfig.SpawnDelay <= Time.timeSinceLevelLoad)
            {
                lastTimeSpawnedEnemy = Time.timeSinceLevelLoad;
                SpawnEnemy();
            }
        }
        
        /// <summary>
        /// Reloads the current scene by resetting positions
        /// and returning all objects to their respective pools.
        /// </summary>
        private void ReloadScene()
        {
            Debug.Log("Scene reload called.");
            
            Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.ReturnAllObjectsToPool();
            Code.Game.Game.Get<ObjectPoolsController>().BulletPool.ReturnAllObjectsToPool();
            
            player.transform.position = playerSpawnPoint.position;
        }
        
        /// <summary>
        /// Spawns the player and sets up related configurations.
        /// </summary>
        private void SpawnPlayer()
        {
            Debug.Log("Spawn player called.");
            
            var playerGo = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, transform);
            player = playerGo.GetComponent<Player>();
            player.Setup(new PlayerModel(playerConfig));

            player.onDie -= HandlePlayerDeath;
            player.onDie += HandlePlayerDeath;
        }
        
        /// <summary>
        /// Spawns an enemy and sets up related configurations.
        /// </summary>
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
            
            enemy.onDie -= HandleEnemyDeath;
            enemy.onDie += HandleEnemyDeath;
        }

        /// <summary>
        /// Handles the event when an enemy dies, returns the enemy to the pool, and updates player's score.
        /// </summary>
        private void HandleEnemyDeath(Enemy obj)
        {
            Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.ReturnObjectToPool(obj.gameObject);
            player.Model.KillEnemy();
        }

        /// <summary>
        /// Handles the event when the player dies and opens the end game popup.
        /// </summary>
        private void HandlePlayerDeath(Player playerModel)
        {
            var endPopup = Code.Game.Game.Get<PopupManager>().Get<EndPopup>();
            endPopup.Setup(player.Model);
            Code.Game.Game.Get<PopupManager>().Open<EndPopup>().Forget();
            Time.timeScale = 0;
        }
    }
}
