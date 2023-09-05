using System;
using System.Threading;
using Code.Model;
using Code.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float changeDirectionInterval;
    
        public event Action<Enemy> onDie = delegate { };

        private Transform _transform;
        private Vector3 targetPosition;
        private EnemyModel model;
        private GameObject bulletPrefab;
        private CancellationTokenSource shootTaskCts;
    
        private float currentInterval;
        private float currentLerpTime;
        private float lastTimeShot;
        private float lerpTime = 5.0f;
    
        public void Setup(EnemyModel model)
        {
            this.model = model;
            model.die += OnModelDie;
        
            if(model.CanShoot)
            {
                shootTaskCts?.Cancel();
                shootTaskCts?.Dispose();
                shootTaskCts = new CancellationTokenSource();
            
                _ = ShootLoop(shootTaskCts.Token);
            }
        }

        private async UniTask ShootLoop(CancellationToken token)
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(model.ShootInterval), cancellationToken: token);
                Shoot();
            }
        }

        private void OnModelDie(EnemyModel obj)
        {
            onDie(this);
        
            shootTaskCts?.Cancel();
            shootTaskCts?.Dispose();
            shootTaskCts = null;
        
            Debug.Log("Enemy died.");
        }

        private void Update()
        {
            if (currentInterval <= 0)
            {
                ChangeDirection();
                currentInterval = changeDirectionInterval;
            }
            else
            {
                currentInterval -= Time.deltaTime;
            }
        
            // Moves the enemy towards the target position
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }
        
            float perc = currentLerpTime / lerpTime;
            float newX = Mathf.Lerp(transform.position.x, targetPosition.x, perc);
            float newZ = transform.position.z - model.Speed * Time.deltaTime;
        
            transform.position = new Vector3(newX, transform.position.y, newZ);
        }

        private void TakeDamage(float damage)
        {
            model.TakeDamage(damage);
        }

        /// <summary>
        /// Randomly changes the enemy's movement direction.
        /// </summary>
        private void ChangeDirection()
        {
            Vector3 newDirection = Random.insideUnitSphere * 10;
            targetPosition = new Vector3(newDirection.x, transform.position.y, transform.position.z);
            currentLerpTime = 0;
        }

        private void Shoot()
        {
            var bulletGo = Game.Get<ObjectPoolsController>().BulletPool.GetObject();
            bulletGo.transform.position = transform.position;
            bulletGo.layer = LayerMask.NameToLayer("EnemyBullet");
        
            var bullet = bulletGo.GetComponent<Bullet>();
            bullet.SetupEnemyBullet(model.BulletSpeed, model.BulletDamage);
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Bullet>(out var bullet))
            {
                TakeDamage(bullet.Damage);
                Game.Get<ObjectPoolsController>().BulletPool.ReturnObjectToPool(other.gameObject);
            }
            else if (other.TryGetComponent<Player>(out var player))
            {
                player.TakeDamage(model.Damage);
            }
        }

        private void OnDisable()
        {
            shootTaskCts?.Cancel();
            shootTaskCts?.Dispose();
            shootTaskCts = null;
        }
    }
}