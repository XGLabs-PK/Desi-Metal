using UnityEngine;
using UnityEngine.Serialization;

namespace XGStudios
{
    public class AiBullet : MonoBehaviour
    {
        [SerializeField]
        public float bulletSpeed = 0.2f;
        [FormerlySerializedAs("EnemyShootPoint")]
        [SerializeField]
        Transform enemyShootPoint;
        AudioSource _hitImpactOnCarSound;

        void Start()
        {
            Destroy(gameObject, 1.5f);
            _hitImpactOnCarSound = GameObject.Find("HitImpactOnCar").GetComponent<AudioSource>();
        }

        void Update()
        {
            if (enemyShootPoint != null)
                transform.position += enemyShootPoint.forward * (bulletSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("AI")) return;

            if (other.gameObject.CompareTag("Car"))
            {
                TheHealth.Instance.TakeDamage(2);
                _hitImpactOnCarSound.Play();
            }
            
            Destroy(transform.gameObject);
        }

        public void SendData(Transform shotPoint)
        {
            enemyShootPoint = shotPoint;
        }
    }
}
