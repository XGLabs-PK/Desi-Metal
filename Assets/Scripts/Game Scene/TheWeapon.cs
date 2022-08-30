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
        public AudioSource weaponAudio;

        float _timer;

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
                weaponAudio.Play();
                Transform firingTransform = firePoint.transform;
                Instantiate(bulletPrefab, firingTransform.position, firingTransform.rotation);
                _timer = delay;
            }
            else
            {
                //weaponAudio.Stop();
                _timer -= Time.deltaTime;
            }
        }
    }
}
