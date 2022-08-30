using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class TheHealth : MonoBehaviour
    {
        public static TheHealth Instance;

        public int health = 100;

        int _maxHealth;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            _maxHealth = health;
        }

        void Update()
        {
            if (_maxHealth <= 25)
            {
                //smoke from car
            }
        }

        public void RepairCar()
        {
            switch (_maxHealth)
            {
                case 100:
                    //Car is already at Full Text
                    break;
                case < 100:
                    _maxHealth += 10;
                    break;
            }

        }

        public void TakeDamage(int damage)
        {
            if (_maxHealth == 0) return;
            
            _maxHealth -= damage;

            if (_maxHealth <= 0)
                GameManager.Instance.carDestroyed = true;
        }
    }
}
