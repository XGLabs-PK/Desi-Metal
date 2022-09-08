using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheHealth : MonoBehaviour
    {
        public static TheHealth Instance;
        public GameObject deathParticles;
        public ScriptableRendererFeature blitRender;

        public int health = 100;
        public Slider healthSlider;

        float _maxHealth;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            BlitEffect(false);
            _maxHealth = health;
        }

        void Update()
        {
            BlitEffect(_maxHealth <= 25f);
            
            StartCoroutine(RepairCar());
            
            if (GameManager.Instance.carDestroyed)
                BlitEffect(false);
        }

        IEnumerator RepairCar()
        {
            if (!(_maxHealth < health))yield break;
            yield return new WaitForSeconds(5f);
            _maxHealth += 0.75f * Time.deltaTime;
            UpdateHealthBar(_maxHealth);
        }

        public void TakeDamage(float damage)
        {
            if (_maxHealth == 0) return;
            FeelManager.Instance.carDamage.PlayFeedbacks();
            _maxHealth -= damage;
            UpdateHealthBar(_maxHealth);

            if (_maxHealth <= 0)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
                FeelManager.Instance.carDestroyed.PlayFeedbacks();
                GameManager.Instance.carDestroyed = true;
            }
        }
        
        void UpdateHealthBar(float currentHealth)
        {
            healthSlider.value = currentHealth;
        }
        
        void BlitEffect(bool boolean)
        {
            blitRender.SetActive(boolean);
        }
    }
}
