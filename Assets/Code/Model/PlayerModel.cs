using System;
using Code.Game;
using Code.Shared;
using UnityEngine;

namespace Code.Model
{
    public class PlayerModel : IModel
    {
        public event Action<PlayerModel> die = delegate { };
        public event Action<PlayerModel> killedEnemy = delegate { };
        public event Action<PlayerModel, float> damageTaken = delegate { };

        // Added new event to handle when player reaches the door
        public event Action<PlayerModel> reachedDoor = delegate { };

        public float Speed { get; }
        public float BulletDamage { get; }
        public float BulletSpeed { get; }
        public float BulletCooldown { get; }
        public float BaseBulletSize { get; private set; } = 0.5f; 
        public float InitialSize { get; private set; }
        public float CurrentSize { get; set; } = 1.0f;
        public float MinSize { get; private set; } = 0.2f;
        public float CurrentHealth { get; set; } // Made setter public

        // Added new property to represent critical minimum size
        public float CriticalMinimumSize { get; private set; } = 0.1f;

        public int Score { get; private set; }
        
        public PlayerModel(PlayerConfig config)
        {
            Speed = config.Speed;
            CurrentHealth = config.Hitpoints; // Adjusted to use new property name
            BulletDamage = config.BulletDamage;
            BulletCooldown = config.BulletCooldown;
            BulletSpeed = config.BulletSpeed;
            InitialSize = config.InitialSize;
        }

        public bool IsDead()
        {
            return CurrentHealth <= 0; // Adjusted to use new property name
        }

        public void TakeDamage(float damage)
        {
            if (IsDead())
            {
                return;
            }

            CurrentHealth = (int) Mathf.Max(0, CurrentHealth - damage); // Adjusted to use new property name
            damageTaken(this, damage);

            if (IsDead())
            {
                die(this);
            }
        }

        public void KillEnemy()
        {
            Score += 1;
            killedEnemy(this);
        }

        public void UpdateSize(float sizeChange)
        {
            InitialSize = Mathf.Max(InitialSize - sizeChange, MinSize);
        }

        public void ResetSize()
        {
            CurrentSize = InitialSize;
        }

        public float GetBulletSize(float shotPower)
        {
            return BaseBulletSize * shotPower;
        }

        public float GetInfectionRadius()
        {
            return Mathf.Max(0.1f, InitialSize - CurrentSize);
        }

        // Added new method to handle reaching the door
        public void ReachDoor()
        {
            reachedDoor(this);
        }
    }
}
