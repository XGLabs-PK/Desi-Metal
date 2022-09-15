using UnityEngine;
using XG.Studios;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheWeapon : MonoBehaviour
    {
        public Transform weapon;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public GameObject desertImpactEffect;
        public GameObject carImpactEffect;

        [Space]
        public float delay = 0.1f;

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

            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("WeaponFiring");

            _timer = delay;

            if (!Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, 3000))
                return;

            if (hit.transform.CompareTag("Ground") || hit.transform.CompareTag("obstruction"))
                Destroy(Instantiate(desertImpactEffect, hit.point, Quaternion.identity), 2f);

            if (hit.transform.CompareTag("Ground"))
                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play("GroundHitImpact");

            if (hit.transform.CompareTag("obstruction"))
                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play("ObsHitImpact");

            if (hit.transform.CompareTag("RealCar")) return;
            if (!hit.transform.CompareTag("AI")) return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("CarHitImpact");

            Destroy(Instantiate(carImpactEffect, hit.point, Quaternion.identity), 2f);
            FeelManager.Instance.enemyDamage.PlayFeedbacks();
            hit.transform.gameObject.GetComponent<NavmeshAi>().TakeDamage(15);
            //Sound
        }
    }
}
