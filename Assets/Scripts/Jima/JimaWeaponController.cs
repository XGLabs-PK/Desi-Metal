using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimaWeaponController : MonoBehaviour
{
    public Camera cam;
    public Transform weapon;
    public GameObject bulletPrefab;
    public Transform muzzle;
    [Space]
    public float lerpFactor = 12.0f;
    public Vector3 camOffset = Vector3.zero;
    public float mouseSensitivity = 0.1f;
    public bool invertY = true;
    [Space]
    public float delay = 0.1f;

    private float timer = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position,
            weapon.transform.position + (weapon.transform.right * camOffset.x) + (weapon.transform.up * camOffset.y) + (weapon.transform.forward * camOffset.z),
            lerpFactor * Time.deltaTime);

        Vector3 rotation = cam.transform.rotation.eulerAngles;
        rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotation.x += Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? -1 : 1);
        cam.transform.rotation = Quaternion.Euler(rotation);

        weapon.transform.rotation = Quaternion.LookRotation(-cam.transform.forward, cam.transform.up);

        if(Input.GetButton("Fire1") && timer <= 0.0f)
        {
            Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
            timer = delay;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
