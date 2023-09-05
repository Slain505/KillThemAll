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

        // Added set access modifier for these properties to allow for size updates
		public float InitialSize { get; private set; } = 1.0f;
		public float CurrentSize { get; set; } = 1.0f;
		public float MinSize { get; private set; } = 0.2f;

        // Added new property to represent critical minimum size
        public float CriticalMinimumSize { get; private set; } = 0.1f;

		public int Score { get; private set; }
		public int HitPoints { get; private set; }
		
		public PlayerModel(PlayerConfig config)
		{
			Speed = config.Speed;
			HitPoints = config.Hitpoints;
			BulletDamage = config.BulletDamage;
			BulletCooldown = config.BulletCooldown;
			BulletSpeed = config.BulletSpeed;
		}
		
		public bool IsDead()
		{
			return HitPoints <= 0;
		}

		public void TakeDamage(float damage)
		{
			if (IsDead())
			{
				return;
			}
			
			HitPoints = (int) Mathf.Max(0, HitPoints - damage);
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
			CurrentSize = Mathf.Max(InitialSize - sizeChange, MinSize);
		}
        
		public void ResetSize()
		{
			CurrentSize = InitialSize;
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
