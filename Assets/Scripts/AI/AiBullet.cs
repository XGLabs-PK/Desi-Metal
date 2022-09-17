using UnityEngine;
using UnityEngine.Serialization;
using XG.Studios;

namespace XGStudios
{
    public class AiBullet : MonoBehaviour
    {
        [SerializeField]
        public float bulletSpeed = 0.2f;
        [FormerlySerializedAs("EnemyShootPoint")]
        [SerializeField]
        Transform enemyShootPoint;
        Transform myTransform;
        float randomDamage;

        void Start()
        {
            Destroy(gameObject, 1.5f);
            myTransform = transform;
        }

        void Update()
        {
            if (enemyShootPoint != null)
                myTransform.position += enemyShootPoint.forward * (bulletSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("AI")) return;

            if (other.gameObject.CompareTag("RealCar"))
            {
                randomDamage = Random.Range(1, 10);
                TheHealth.Instance.TakeDamage(randomDamage);

                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play("CarHitImpact");
            }

            Destroy(transform.gameObject);
        }

        public void SendData(Transform shotPoint)
        {
            enemyShootPoint = shotPoint;
        }
    }
}
