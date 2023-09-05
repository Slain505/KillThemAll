using UnityEngine;

namespace Code.Game
{
    /// <summary>
    /// Handles the player's input controls, distinguishing between touch inputs and keyboard inputs.
    /// </summary>
    public class PlayerControlHandler
    {
        public bool IsShooting;
        public float HoldTime { get; private set; }
        public float ShotPower { get; private set; }
        
        private float maxHoldTime = 5f;
        private bool isHolding = false;

        public void HandleTouchInputs()
        {
            HandleShootsTouches();
        }

        private void HandleShootsTouches()
        {
            bool wasShooting = IsShooting;
            
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                IsShooting = TouchDetected(new Touch()
                {
                    position = Input.mousePosition
                });
            }
            else
            {
                IsShooting = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space Key Detected");
                IsShooting = true;
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    IsShooting = IsTouchDetected(touch);
                    if (IsShooting)
                    {
                        break;
                    }
                }
            }
#endif
            
            if (IsShooting)
            {
                if (!wasShooting)
                {
                    isHolding = true;
                    HoldTime = 0f;
                }
                
                if (isHolding)
                {
                    HoldTime += Time.deltaTime;
                    ShotPower = Mathf.Clamp(HoldTime / maxHoldTime, 0f, 1f);
                }
            }
            else if (wasShooting)
            {
                isHolding = false;
            }
        }

        private bool TouchDetected(Touch touch)
        {
            return true;
        }
    }
}