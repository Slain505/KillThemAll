using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class Enemy : MonoBehaviour
    {

        public void Setup()
        {
            
        }

        private void HandleInfection(float infectionRadius)
        {
            var colliders = Physics.OverlapSphere(transform.position, infectionRadius, LayerMask.GetMask("Enemy"));
            
            foreach (var collider in colliders)
            {
                Game.Get<ObjectPoolsController>().EnemyPool.ReturnObjectToPool(collider.gameObject);
            }
            
            Game.Get<ObjectPoolsController>().EnemyPool.ReturnObjectToPool(gameObject);
        }

        public void ChangeColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        public void Explode()
        {
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enemy collided with " + other.gameObject.name);
            
            HandleInfection(other.GetComponent<Bullet>().InfectionRadius);
        }
    }
}