using System;
using UnityEngine;

namespace Code.Game
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform doorTransform;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                Debug.Log("Player entered door.");
            }
        }
    }
}