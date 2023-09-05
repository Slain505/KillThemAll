using System;
using System.Threading;
using Code.Model;
using Code.Shared;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game
{
    public class Enemy : MonoBehaviour
    {
        public event Action<Enemy> onInfected = delegate { };
        private EnemyModel model;

        public void Setup(EnemyModel enemyModel)
        {
            model = enemyModel;
            model.Infected += HandleInfection;
            // Инициализируйте другие свойства и события
        }

        private void HandleInfection(EnemyModel enemyModel)
        {
            onInfected(this);
        }

        public void ChangeColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        public void Explode()
        {
            
        }
    }
}