using Code.Shared;
using UnityEngine;

namespace Code.Game
{
	/// <summary>
	/// Represents a bullet in the game.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		private float damage;
		private float infectionRadius;

		private Rigidbody cachedRigidbody;
		public float InfectionRadius => infectionRadius;
		private void Awake()
		{
			cachedRigidbody = GetComponent<Rigidbody>();
		}
		
		/// Configures the bullet with attributes appropriate for a player's bullet.
		/// </summary>
		/// <param name="speed">The speed at which the bullet moves forward.</param>
		/// <param name="damage">The damage this bullet can inflict.</param>
		/// <param name="infectionRadius">The radius within which the bullet can infect enemies.</param>
		public void SetupPlayerBullet(float speed, float bulletSize)
		{
			gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
			GetComponent<Renderer>().material.color = Color.yellow;
			cachedRigidbody.velocity = Vector3.forward * speed;
			infectionRadius = bulletSize * 1.2f;
			transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
		}

		private void OnTriggerEnter(Collider other)
		{
			Game.Get<ObjectPoolsController>().BulletPool.ReturnObjectToPool(gameObject);
		}
	}
}