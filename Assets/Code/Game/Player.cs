using System;
using Code.Model;
using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> onDie = delegate { };
        
        private PlayerControlHandler playerControlHandler;
        private PlayerModel model;
        private float lastTimeShot;
        
        public PlayerModel Model => model;
        
        private void Start()
        {
            playerControlHandler = new PlayerControlHandler();
        }

        private void Update()
        {
            playerControlHandler.HandleTouchInputs();
            
            if (playerControlHandler.IsShooting)
            {
                Shoot();
            }
            
            // Update player size based on hold time (this is just a placeholder, refine as needed)
            float sizeDecrease = playerControlHandler.HoldTime * 0.1f;
            model.CurrentSize = model.InitialSize - sizeDecrease;
            UpdatePlayerSize();

            // Check if the player size has reached critical minimum size
            if(model.CurrentSize <= model.CriticalMinimumSize)
            {
                // Trigger lose condition
                OnModelDie(model);
            }
        }
        
        public void Setup(PlayerModel model)
        {
            this.model = model;
            model.die += OnModelDie;
            
            transform.localScale = Vector3.one * model.InitialSize;
        }

        public void TakeDamage(float damage)
        {
            model.TakeDamage(damage);
            
            UpdatePlayerSize();
        }
        
        private void UpdatePlayerSize()
        {
            transform.localScale = Vector3.one * model.CurrentSize;
        }
        
        private void OnModelDie(PlayerModel obj)
        {
            onDie(this);
        }

        private void Shoot()
        {
            if (lastTimeShot + model.BulletCooldown <= Time.timeSinceLevelLoad)
            {
                var bulletGo = Code.Game.Game.Get<ObjectPoolsController>().BulletPool.GetObject();
                bulletGo.transform.position = transform.position;
                bulletGo.layer = LayerMask.NameToLayer("PlayerBullet");
                var bullet = bulletGo.GetComponent<Bullet>();
                
                float infectionRadius = model.GetInfectionRadius() * playerControlHandler.ShotPower;
                bullet.SetupPlayerBullet(model.BulletSpeed, model.BulletDamage, infectionRadius);
                lastTimeShot = Time.timeSinceLevelLoad;
                
                // Reduce the player size based on the shot power (you might need to adjust the multiplier)
                model.CurrentSize -= playerControlHandler.ShotPower * 0.2f;
                UpdatePlayerSize();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Bullet>(out var bullet))
            {
                TakeDamage(bullet.Damage);
                Code.Game.Game.Get<ObjectPoolsController>().BulletPool.ReturnObjectToPool(other.gameObject);
            }
        }
    }
}
