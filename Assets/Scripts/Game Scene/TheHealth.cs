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
        public GameObject smokeEffect;
        public GameObject fireSmoke;
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
            SmokeEffect(_maxHealth <= 25f);
            BlitEffect(_maxHealth <= 20f);
            FireEffect(_maxHealth <= 15f);
            
            StartCoroutine(RepairCar());
            
            if (GameManager.Instance.carDestroyed)
                BlitEffect(false);
        }

        IEnumerator RepairCar()
        {
            if (!(_maxHealth < health))yield break;
            yield return new WaitForSeconds(10f);
            _maxHealth += 0.25f * Time.deltaTime;
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

        void SmokeEffect(bool boolean)
        {
            smokeEffect.SetActive(boolean);
        }

        void FireEffect(bool boolean)
        {
            fireSmoke.SetActive(boolean);
        }
    }
}
