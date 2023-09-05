using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    /// <summary>
    /// Represents the walls in the game environment.
    /// Handles enemies and bullets disappearing when they out of FoV.
    /// </summary>
    public class Walls : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Enemy is out of FoV.");
                global::Code.Game.Game.Get<ObjectPoolsController>().EnemyPool.ReturnObjectToPool(other.gameObject);
            }
            else if (other.CompareTag("Bullet"))
            {
                Debug.Log("Bullet is out of FoV.");
                global::Code.Game.Game.Get<ObjectPoolsController>().BulletPool.ReturnObjectToPool(other.gameObject);
            }
        }
    }
}