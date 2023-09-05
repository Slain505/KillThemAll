using UnityEngine;

namespace Code.Shared
{
    public interface ISpawnStrategy
    {
        void SpawnChildBall(Transform mainBallTransform, GameObject childBallPrefab, float launchSpeed,
            float timeHeldDown);
    }
}