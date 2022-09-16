using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheBullet : MonoBehaviour
    {
        public float speed = 800.0f;

        void OnEnable()
        {
            Invoke(nameof(OnDisable), 3f);
        }

        void OnDisable()
        {
            transform.gameObject.SetActive(false);
        }

        void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}
