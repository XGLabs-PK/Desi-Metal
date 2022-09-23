using Unity.VisualScripting;
using UnityEngine;
using XG.Studios;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheWeapon : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform weapon;
        public Transform firePoint;
        public GameObject carImpactEffect;
        PoolManager _poolManager;

        [Space]
        public float delay = 0.1f;

        Camera _cam;
        float _timer;

        void Start()
        {
            _cam = Camera.main;
            _poolManager = FindObjectOfType<PoolManager>();
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
            Destroy(Instantiate(bulletPrefab, firingTransform.position, firingTransform.rotation), 4f);
            
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("WeaponFiring");

            _timer = delay;

            if (!Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, 3000))
                return;

            if (hit.transform.CompareTag("Ground") || hit.transform.CompareTag("obstruction"))
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.Play("GroundHitImpact");

                for (int i = 0; i < _poolManager.bulletImpactList.Count; i++)
                {
                    if (_poolManager.bulletImpactList[i].activeInHierarchy == false)
                    {
                        _poolManager.bulletImpactList[i].SetActive(true);
                        _poolManager.bulletImpactList[i].transform.position = hit.point;
                        _poolManager.bulletImpactList[i].transform.rotation = Quaternion.LookRotation(hit.normal);
                        break;
                    }

                    if (i == _poolManager.bulletImpactList.Count - 1)
                    {
                        //Last Bullet
                        GameObject newBulletImpact = Instantiate(_poolManager.bulletImpactPrefab);
                        newBulletImpact.SetActive(false);
                        newBulletImpact.transform.parent = _poolManager.transform.parent;
                        _poolManager.bulletImpactList.Add(newBulletImpact);
                    }
                }
            }

            if (hit.transform.CompareTag("RealCar")) return;
            if (!hit.transform.CompareTag("AI")) return;

            if (AudioManager.Instance != null)
                AudioManager.Instance.Play("CarHitImpact");

            Destroy(Instantiate(carImpactEffect, hit.point, Quaternion.identity), 2f);

            FeelManager.Instance.enemyDamage.PlayFeedbacks();
            int randomDamage = Random.Range(10, 20);
            hit.transform.gameObject.GetComponent<NavmeshAi>().TakeDamage(randomDamage);
            //Sound
        }
    }
}
