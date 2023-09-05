using UnityEngine;

namespace Code.Shared
{
    public interface IShrinkStrategy
    {
        void Shrink(Transform ballTransform, float shrinkSpeed, float maxShrinkSize, ref float timeHeldDown);
    }
}
