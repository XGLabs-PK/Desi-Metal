using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class TheWeapon : MonoBehaviour
    {
        Camera _cam;
        public Transform weapon;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public GameObject impactEffect;
        [Space]
        public float delay = 0.1f;
        public AudioSource weaponAudio;

        float _timer;

        void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            weapon.transform.rotation = _cam.transform.rotation;
            if(Input.GetButton("Fire1") && _timer <= 0f)
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
            if (hit.transform.CompareTag("Car")) return;
            if (hit.transform.CompareTag("Enemy"))
            {
                //Decrease Enemy Health
                //Particles
                //Sound
            }
            
            Instantiate(impactEffect, hit.point, Quaternion.identity);
        }
    }
}
