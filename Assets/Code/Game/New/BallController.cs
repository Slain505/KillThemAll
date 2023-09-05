using System;
using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class BallController
    {
        [SerializeField] private GameObject mainBall;
        [SerializeField] private GameObject childBallPrefab;
        [SerializeField] private float maxShrinkSize = 0.5f;
        [SerializeField] private float shrinkSpeed = 0.01f;
        [SerializeField] private float launchSpeed = 10f;
        private float timeHeldDown;
        private Vector3 originalSize;
        private bool isHoldingDown;
        
        private IShrinkStrategy shrinkStrategy = new SimpleShrinkStrategy();
        private ISpawnStrategy spawnStrategy = new SimpleSpawnStrategy();

        private void Start()
        {
            originalSize = mainBall.transform.localScale;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHoldingDown = true;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                isHoldingDown = false;
                spawnStrategy.SpawnChildBall(mainBall.transform, childBallPrefab, launchSpeed, timeHeldDown);
                mainBall.transform.localScale = originalSize;
                timeHeldDown = 0;
            }

            if (isHoldingDown)
            {
                shrinkStrategy.Shrink(mainBall.transform, shrinkSpeed, maxShrinkSize, ref timeHeldDown);
            }
        }
    }
}