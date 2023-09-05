using UnityEngine;

namespace Code.UI
{
    /// <summary>
    /// Adjusts the UI element's RectTransform to fit within the device's safe area.
    /// This helps in ensuring that UI elements are not obscured by notches, rounded corners, or other screen obstructions.
    /// </summary>
    public class SafeArea : MonoBehaviour
    {
        void Awake()
        {
            if (TryGetComponent<RectTransform>(out var rectTransform))
            {
                Vector2 anchorMin = Screen.safeArea.position;
                Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }
    }
}