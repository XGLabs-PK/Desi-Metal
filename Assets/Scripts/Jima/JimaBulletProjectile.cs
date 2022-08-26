using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimaBulletProjectile : MonoBehaviour
{
    public float speed = 100.0f;

    void Start()
    {
        Destroy(transform.parent.gameObject, 2.0f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
