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
		
		public float Speed { get; }
		public float BulletDamage { get; }
		public float BulletSpeed { get;}
		public float BulletCooldown { get; }
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
			return HitPoints == 0;
		}

		public void TakeDamage(float damage)
		{
			if (IsDead())
			{
				return;
			}
			
			HitPoints = (int) Mathf.Max(0, HitPoints - damage);
			damageTaken(this, damage);
			
			if (!IsDead())
			{
				return;
			}
            
			die(this);
		}

		public void KillEnemy()
		{
			Score += 1;
			killedEnemy(this);
		}
	}
}