using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
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

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Car")) return;
            if (other.gameObject.CompareTag("Enemy"))
            {
                //Decrease Enemy Health
                //Particles
                //Sound
            }
        }
    }
}
