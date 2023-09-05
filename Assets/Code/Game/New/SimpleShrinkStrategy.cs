using Code.Shared;
using UnityEngine;

namespace Code.Game
{
    public class SimpleShrinkStrategy : IShrinkStrategy
    {
        public void Shrink(Transform ballTransform, float shrinkSpeed, float maxShrinkSize, ref float timeHeldDown)
        {
            timeHeldDown += Time.deltaTime;
            float newScale = Mathf.Max(ballTransform.localScale.x - (timeHeldDown * shrinkSpeed), maxShrinkSize);
            ballTransform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}