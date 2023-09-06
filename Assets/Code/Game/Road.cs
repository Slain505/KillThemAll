using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game
{
    public class Road : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Enemy exited road.");
            
            var enemiesOnRoad = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity,
                LayerMask.GetMask("Enemy"));
            if (enemiesOnRoad.Length == 0)
            {
                Debug.Log("No enemies on road. Game over.");
            }
        }
    }
}