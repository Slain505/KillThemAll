using System;
using Code.Shared;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform doorTransform;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Door collided with " + other.gameObject.name);
            Game.Get<PopupManager>().Get<LevelWinPopup>();
            Game.Get<PopupManager>().Open<LevelWinPopup>().Forget(); 
            Time.timeScale = 0;
        }
    }
}