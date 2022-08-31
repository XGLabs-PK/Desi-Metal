using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class TheHealth : MonoBehaviour
    {
        public static TheHealth Instance;
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
            
            FeelManager.Instance.carDamage.PlayFeedbacks();
            _maxHealth -= damage;

            if (_maxHealth <= 0)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
                FeelManager.Instance.carDestroyed.PlayFeedbacks();
                GameManager.Instance.carDestroyed = true;
            }
        }
    }
}
