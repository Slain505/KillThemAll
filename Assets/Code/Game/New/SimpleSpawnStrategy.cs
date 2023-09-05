using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class SimpleSpawnStrategy : MonoBehaviour, ISpawnStrategy
    {
        private Rigidbody _cachedRigidbody;
        
        private void Awake()
        {
            _cachedRigidbody = GetComponent<Rigidbody>();
        }
        
        public void SpawnChildBall(Transform mainBallTransform, GameObject childBallPrefab, float launchSpeed, float timeHeldDown)
        {
            GameObject childBall =
                GameObject.Instantiate(childBallPrefab, mainBallTransform.position, Quaternion.identity);
            _cachedRigidbody.velocity = Vector3.forward * launchSpeed;
        }
    }
}