using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheBullet : MonoBehaviour
    {
        public float speed = 800.0f;

        void Start()
        {
            Destroy(transform.gameObject, 4.0f);
        }

        void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}
