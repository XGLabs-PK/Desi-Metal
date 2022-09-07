using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheBullet : MonoBehaviour
    {
        public float speed = 800.0f;
        public int damage = 10;

        void Start()
        {
            Destroy(transform.gameObject, 4.0f);
        }

        void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("AI")) {

                other.GetComponent<NavmeshAi>().TakeDamage(damage);
                Debug.Log("HIT");
                
            }
        }
    }
}
