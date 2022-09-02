using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AiBullet : MonoBehaviour
    {
        [SerializeField] float bulletSpeed = 0.2f;
    Vector3 finalPosition;
    public GameObject tar;
    private void Start()
    {
        Destroy(gameObject, 6f);
    }
    public void moveToTarget(GameObject target,float displacement) {
        tar = target;
        Debug.Log(displacement);
         finalPosition = new Vector3(target.transform.position.x + displacement, target.transform.position.y, target.transform.position.z+displacement);
        }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, bulletSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Destroy(this.gameObject);
        }

    }
}

