using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheWeapon : MonoBehaviour
    {
        public Transform weapon;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public GameObject impactEffect;
        [Space]
        public float delay = 0.1f;
        public AudioSource weaponAudio;
        Camera _cam;

        float _timer;

        void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            weapon.transform.rotation = _cam.transform.rotation;

            if (Input.GetButton("Fire1") && _timer <= 0f)
                Shoot();
            else
                _timer -= Time.deltaTime;
        }

        void Shoot()
        {
            Transform firingTransform = firePoint.transform;
            Instantiate(bulletPrefab, firingTransform.position, firingTransform.rotation);
            weaponAudio.Play();
            _timer = delay;

            if (!Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, 3000))
                return;

            Instantiate(impactEffect, hit.point, Quaternion.identity);
            if (hit.transform.CompareTag("Car")) return;

            if (hit.transform.CompareTag("AI"))
            {
                FeelManager.Instance.enemyDamage.PlayFeedbacks();
                //EnemyHealth.Instance.TakeDamage(15);
                //Particles
                //Sound
            }

        }
    }
}
