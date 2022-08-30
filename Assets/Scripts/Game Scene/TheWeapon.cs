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
        public GameObject muzzleFlash;
        [Space]
        public float delay = 0.1f;

        float _timer;
        
        //20 and -50 on X
        //20 and -20 on Z

        void Start()
        {
            _cam = Camera.main;
            muzzleFlash.SetActive(false);
        }

        void Update()
        {
            weapon.transform.rotation = _cam.transform.rotation;
            muzzleFlash.SetActive(Input.GetButton("Fire1"));
            if(Input.GetButton("Fire1") && _timer <= 0f)
            {
                Transform firingTransform = firePoint.transform;
                Instantiate(bulletPrefab, firingTransform.position, firingTransform.rotation);
                _timer = delay;
            }
            else
                _timer -= Time.deltaTime;
            
        }
    }
}
