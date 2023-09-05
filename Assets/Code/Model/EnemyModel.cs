using System;
using Code.Game;
using Code.Shared;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Model
{
    public class EnemyModel : IModel
    {
        public event Action<EnemyModel> die = delegate { };
        
        public int MaxHp { get; }
        public float Speed { get; }
        public float Damage { get; }
        public bool CanShoot { get; }
        public float BulletSpeed { get; }
        public float BulletDamage { get; }
        public float ShootInterval { get; }
        private int Hp { get; set; }
        
        public EnemyModel(EnemyConfig config)
        {
            MaxHp = config.Hitpoints;
            Hp = config.Hitpoints;
            Speed = config.Speed;
            Damage = config.Damage;
            BulletSpeed = config.BulletSpeed;
            BulletDamage = config.BulletDamage;
            ShootInterval = config.ShootInterval;
            CanShoot = Random.Range(0, 100) % 2 == 0;
        }
        
        public void TakeDamage(float damage)
        {
            if(IsDead())
            {
                return;
            }
            
            Hp = (int) Mathf.Max(0, Hp - damage);
            
            if(IsDead())
            {
                die(this);
            }
        }
        
        private bool IsDead()
        {
            return Hp == 0;
        }
    }
}