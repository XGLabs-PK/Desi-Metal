using UnityEngine;
using UnityEngine.Serialization;

namespace XGStudios
{
    public class AiBullet : MonoBehaviour
    {
        [SerializeField]
        float bulletSpeed = 0.2f;
        public GameObject tar;
        [FormerlySerializedAs("EnemyShootPoint")]
        [SerializeField]
        Transform enemyShootPoint;
        Vector3 _finalPosition;

        void Start()
        {
            Destroy(gameObject, 1.5f);
        }

        void Update()
        {
            //transform.position = Vector3.Lerp(transform.position,finalPosition, bulletSpeed * Time.deltaTime);
            if (enemyShootPoint != null)
                transform.position += enemyShootPoint.forward * (bulletSpeed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("AI")) return;

            if (other.gameObject.CompareTag("Car"))
                TheHealth.Instance.TakeDamage(5);

            Destroy(transform.gameObject);
        }

        public void SendData(GameObject target, float displacement, Transform shotPoint)
        {
            tar = target;

            _finalPosition = new Vector3(target.transform.position.x + displacement, target.transform.position.y,
                target.transform.position.z + displacement);

            enemyShootPoint = shotPoint;
            //Destroy(gameObject);
        }
    }
}
