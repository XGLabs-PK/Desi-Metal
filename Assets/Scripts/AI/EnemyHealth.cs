using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class EnemyHealth : MonoBehaviour
    {
        public static EnemyHealth Instance;
        public GameObject deathParticles;

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
                //Screen Grey
            }
            
            if (_maxHealth <= 15)
            {
                //smoke from car
                //Screen Dark Grey
            }
        }

        public void TakeDamage(int damage)
        {
            if (_maxHealth == 0) return;
            _maxHealth -= damage;

            if (_maxHealth <= 0)
            {
                Destroy(Instantiate(deathParticles, transform.position, Quaternion.identity), 2f);
                Destroy(gameObject);
            }
        }
    }
}
