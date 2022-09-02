using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AiBullet : MonoBehaviour
    {
    [SerializeField] float bulletSpeed = 0.2f;
    Vector3 finalPosition;
    public GameObject tar;
    [SerializeField]Transform EnemyShootPoint;
    private void Start()
    {
        Destroy(gameObject, 4f);
    }
    public void sendData(GameObject target,float displacement,Transform shotPoint) {
        tar = target;
         finalPosition = new Vector3(target.transform.position.x + displacement, target.transform.position.y, target.transform.position.z+displacement);
        EnemyShootPoint = shotPoint;
        }
    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position,finalPosition, bulletSpeed * Time.deltaTime);
        transform.position += EnemyShootPoint.forward * (bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {

            Destroy(this.gameObject);
        }
    }
}

