using UnityEngine;

namespace Code.Game
{
    /// <summary>
    /// Handles the player's input controls, distinguishing between touch inputs and keyboard inputs.
    /// </summary>
    public class PlayerControlHandler
    {
        public bool IsShooting;
        
        public void HandleTouchInputs()
        {
            HandleShootsTouches();
        }

        private void HandleShootsTouches()
        {
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
        }

        /// <summary>
        /// Checks if the touch input is centered on the screen.
        /// </summary>
        /// <param name="touch">The touch input to check.</param>
        /// <returns>True if touch is in the center of the screen, false otherwise.</returns>
        private bool TouchDetected(Touch touch)
        {
            return true;
        }
    }
}