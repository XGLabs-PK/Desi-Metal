using UnityEngine;
using UnityEngine.Serialization;

namespace XGStudios
{
    public class AiBullet : MonoBehaviour
    {
        [SerializeField]
        public float bulletSpeed = 0.2f;
        public Transform tar;
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
            if (other.gameObject.CompareTag("Car"))
                TheHealth.Instance.TakeDamage(2);
            
            Destroy(transform.gameObject);
        }

        public void SendData(Transform shotPoint)
        {
            enemyShootPoint = shotPoint;
        }
    }
}
