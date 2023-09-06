using System;
using UnityEngine;

namespace Code.Shared
{
    /// <summary>
    /// Manages the object pools for the game's enemies, bullets, and leaderboard entries.
    /// </summary>
    public class ObjectPoolsController : MonoBehaviour, IManager
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject leaderboardPrefab;
        
        private ObjectPool enemyPool;
        private ObjectPool bulletPool;
        private ObjectPool leaderboardPool;
        
        public ObjectPool EnemyPool => enemyPool;
        public ObjectPool BulletPool => bulletPool;
        public ObjectPool LeaderboardPool => leaderboardPool;
        
        private void Start()
        {
            Init();
            DontDestroyOnLoad(this);
        }
        
        private void Init()
        {
            var enemyContainer = new GameObject("EnemyContainer");
            var bulletContainer= new GameObject("BulletContainer");
            var leaderboardContainer = new GameObject("LeaderboardContainer");
            enemyContainer.transform.SetParent(transform);
            bulletContainer.transform.SetParent(transform);
            leaderboardContainer.transform.SetParent(transform);
            
            enemyPool = new ObjectPool(enemyPrefab, 100, enemyContainer.transform);
            bulletPool = new ObjectPool(bulletPrefab, 5, bulletContainer.transform);
            leaderboardPool = new ObjectPool(leaderboardPrefab, 10, leaderboardContainer.transform);
            
            Debug.Log("Object pools have been created.");
        }
    }
}