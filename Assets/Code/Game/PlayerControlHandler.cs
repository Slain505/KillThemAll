using UnityEngine;

namespace Code.Game
{
    public enum MoveDirection
    {
        None,
        Left,
        Right
    }

    /// <summary>
    /// Handles the player's input controls, distinguishing between touch inputs and keyboard inputs.
    /// </summary>
    public class PlayerControlHandler
    {
        public MoveDirection CurrentMoveDirection;
        public bool IsShooting;
        
        public void HandleTouchInputs()
        {
            HandleSideTouches();
            HandleShootsTouches();
        }
        
        private void HandleSideTouches()
        {
            int touchDirection = 0;
            
#if UNITY_EDITOR            
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                touchDirection += GetTouchSide(new Touch()
                {
                    position = Input.mousePosition
                });
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchDirection += GetTouchSide(touch);
                }
            }
#endif
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("Left Key Detected");
                touchDirection--;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("Right Key Detected");
                touchDirection++;
            }
            
            if(touchDirection == 0)
            {
                CurrentMoveDirection = MoveDirection.None;
            }
            else if(touchDirection < 0)
            {
                CurrentMoveDirection = MoveDirection.Left;
            }
            else
            {
                CurrentMoveDirection = MoveDirection.Right;
            }
        }
        
        private void HandleShootsTouches()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                IsShooting = IsTouchCenter(new Touch()
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
                    IsShooting = IsTouchCenter(touch);
                    if (IsShooting)
                    {
                        break;
                    }
                }
            }
#endif
        }
        
        /// <summary>
        /// Determines on which side of the screen the touch input occurred.
        /// </summary>
        /// <param name="touch">The touch input to check.</param>
        /// <returns>Returns -1 for left, 1 for right, and 0 otherwise.</returns>
        private int GetTouchSide(Touch touch)
        {
            if (touch.position.x < Screen.width * 0.33f)
            {
                return -1;
            }
            else if (touch.position.x > Screen.width * 0.66f)
            {
                return 1;
            }
            return 0;
        }
        
        /// <summary>
        /// Checks if the touch input is centered on the screen.
        /// </summary>
        /// <param name="touch">The touch input to check.</param>
        /// <returns>True if touch is in the center of the screen, false otherwise.</returns>
        private bool IsTouchCenter(Touch touch)
        {
            return touch.position.x > Screen.width * 0.33f && touch.position.x < Screen.width * 0.66f;
        }
    }
}