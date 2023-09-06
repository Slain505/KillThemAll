using System.Collections.Generic;
using Code.Shared;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;
        [SerializeField] private Transform doorSpawnPoint;

        private ObjectPool enemyPool;
        private Player player;
        private Door door;
        private Road road;

        private RaycastHit hit;
        private int currentEnemyCount = 0;
        private int maxEnemyCount = 100;
        private int minEnemyCount = 50;

        private void Start()
        {
            ClearScene();
            Time.timeScale = 1;
            
            Debug.Log("Start gameplay called.");

            SpawnPlayer();
            SpawnDoor();
            SpawnRoad();
            
            if (maxEnemyCount >= currentEnemyCount)
            {
                SpawnEnemy();
            }
        }

        private void Update()
        {
           
        }

        private void ClearScene()
        {
            Debug.Log("Scene reload called.");

            Game.Get<ObjectPoolsController>().EnemyPool.ReturnAllObjectsToPool();
            Game.Get<ObjectPoolsController>().BulletPool.ReturnAllObjectsToPool();

            if (player != null)
            {
                player.transform.position = playerSpawnPoint.position;
            }
        }

        private void SpawnPlayer()
        {
            Debug.Log("Spawn player called.");

            var playerGo = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, transform);
            
            player = playerGo.GetComponent<Player>();
            //player.Setup(new PlayerModel(playerConfig));

            player.onDie -= HandlePlayerDeath;
            player.onDie += HandlePlayerDeath;
            player.onShoot -= HandlePlayerShoot;
            player.onShoot += HandlePlayerShoot;
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
            float enemySpacing = 1.0f; // Увеличиваем значение, чтобы предотвратить перекрытие

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
                    //g.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                else
                {
                    Debug.LogWarning("Max attempts reached, enemy not spawned");
                }
            }
        }

        private void SpawnRoad()
        {
            Debug.Log("Spawn road called.");

            float distance = Vector3.Distance(playerSpawnPoint.position, doorSpawnPoint.position) / 10;
            Vector3 vectorDirection = (doorSpawnPoint.position - playerSpawnPoint.position);
            Vector3 middlePoint = playerSpawnPoint.position + vectorDirection / 2;
            
            var roadGo = Instantiate(roadPrefab, middlePoint, Quaternion.identity, transform);
            road = roadGo.GetComponentInChildren<Road>();
            road.transform.localScale = new Vector3(road.transform.localScale.x, road.transform.localScale.y, distance);
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

        private void HandlePlayerDeath(Player obj)
        {
            Debug.Log("Player died.");
            Destroy(obj.gameObject);
            Time.timeScale = 0;
        }
        
        private void HandlePlayerShoot(Player obj)
        {
            road.transform.parent.localScale = new Vector3(obj.transform.localScale.x, road.transform.parent.localScale.y, road.transform.parent.localScale.z);
        }
    }
}
