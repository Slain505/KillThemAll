using System;
using Code.Model;
using Code.Shared;
using UnityEngine;

namespace Code.Game
{
	/// <summary>
	/// Represents the player character in the game, handling movement, shooting, and interactions.
	/// </summary>
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

			if (playerControlHandler.CurrentMoveDirection == MoveDirection.Left)
			{
                MoveLeft();
			}
			else if (playerControlHandler.CurrentMoveDirection == MoveDirection.Right)
			{
				MoveRight();	
			}

			if (playerControlHandler.IsShooting)
			{
				Shoot();
			}
		}
		
		public void Setup(PlayerModel model)
		{
			this.model = model;
			model.die += OnModelDie;
		}
		public void TakeDamage(float damage)
		{
			model.TakeDamage(damage);
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
				
				bullet.SetupPlayerBullet(model.BulletSpeed, model.BulletDamage);
				lastTimeShot = Time.timeSinceLevelLoad;
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