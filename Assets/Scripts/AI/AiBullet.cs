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

        void Start()
        {
            Destroy(gameObject, 1.5f);
        }

        void Update()
        {
            if (enemyShootPoint != null)
                transform.position += enemyShootPoint.forward * (bulletSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("AI")) return;

            if (other.gameObject.CompareTag("RealCar"))
            {
                TheHealth.Instance.TakeDamage(2);
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
