using UnityEngine;

public class PlayerControlHandler
{
    public bool IsShooting;
    public bool HasReleasedShootButton { get; private set; } = true;
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
        if (Input.GetMouseButtonUp(0))
        {
            IsShooting = false;
            HasReleasedShootButton = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            IsShooting = true;
            HasReleasedShootButton = false;
        }
#else
for (int i = 0; i < Input.touchCount; i++)
{
    Touch touch = Input.GetTouch(i);

    if (touch.phase == TouchPhase.Ended)
    {
        IsShooting = false;
        HasReleasedShootButton = true;
    }
    else if (touch.phase == TouchPhase.Began)
    {
        IsShooting = true;
        HasReleasedShootButton = false;
    }
}
#endif

        
        if (IsShooting)
        {
            if (!wasShooting)
            {
                isHolding = true;
                HoldTime = 0f;
                HasReleasedShootButton = false;
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
