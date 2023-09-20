using System;
using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> onDie = delegate { }; 
        public event Action<Player> onShoot = delegate { };
        
        private float lastTimeShot;
        private float currentSize = 3f;
        private float maxSize = 5f;
        private float minSize = 1f;
        private float clickStartTime = 0f;

        private void Start()
        {
            transform.localScale = Vector3.one * currentSize;
            onShoot(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickStartTime = Time.realtimeSinceStartup;
            }

            if (Input.GetMouseButtonUp(0))
            {
                float timeSinceClickStartTime = Time.realtimeSinceStartup - clickStartTime;
                if (timeSinceClickStartTime >= 0.3f)
                {
                    float sizeDecrease = timeSinceClickStartTime / 5f;
                    UpdatePlayerSize(sizeDecrease);
                    Shoot(sizeDecrease * 5f);
                    onShoot(this);
                }
            }

            if (Input.GetMouseButton(0))
            {
                float timeSinceClickStartTime = Time.realtimeSinceStartup - clickStartTime;
                float sizeDecrease = timeSinceClickStartTime / 5f;
                
                if(currentSize - sizeDecrease <= minSize)
                {
                    OnModelDie();
                }
            }

            // Check if the player size has reached critical minimum size
            if(currentSize <= minSize)
            {
                OnModelDie();
            }
        }

        private void UpdatePlayerSize(float sizeDecrease)
        {
            // Update player size based on hold time (this is just a placeholder, refine as needed)
            currentSize -= sizeDecrease;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = Vector3.one * currentSize;
        }

        private void OnModelDie()
        {
            onDie(this);
        }

        private void Shoot(float size)
        {
            if (lastTimeShot <= Time.timeSinceLevelLoad)
            {
                var bulletGo = Game.Get<ObjectPoolsController>().BulletPool.GetObject();
                bulletGo.transform.position = transform.position;

                var bullet = bulletGo.GetComponent<Bullet>();
                bullet.SetupPlayerBullet(5, size);
            }
        }
    }
}
