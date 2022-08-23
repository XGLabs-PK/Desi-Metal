using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySwarm : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] AIobjects;
    [SerializeField] float spacing;
    [SerializeField] float separatingSpeed;
    void Start()
    {
        AIobjects = GameObject.FindGameObjectsWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject go in AIobjects) {
            if (go != gameObject) {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance <= spacing) {
                    Vector3 dir = transform.position - go.transform.position;
                    Vector3.Lerp(transform.position, dir, separatingSpeed *Time.deltaTime);
                }
            }
        }
    }
}
