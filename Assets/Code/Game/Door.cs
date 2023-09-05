using System;
using UnityEngine;

namespace Code.Game
{
    public class Door : MonoBehaviour
    {
        // Это событие будет вызвано, когда игрок войдет в дверь
        public event Action onPlayerEntered = delegate { };

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                onPlayerEntered();
            }
        }
    }
}