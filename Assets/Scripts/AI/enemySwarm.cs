using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySwarm : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] AIobjects;
    [SerializeField] [Range(0f, 4f)] float lerptime;
    [SerializeField] float spacing;
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
                    Vector3 aiDirection = (transform.position) + dir;
                    transform.position = Vector3.MoveTowards(transform.position, aiDirection, lerptime *Time.fixedDeltaTime);
                   
                }
            }
        }
    }
    Vector3 randomizedPlusorMinus(Vector3 direction,Vector3 ourPosition) {
        float randomVar = Random.Range(0f, 1f);
        Vector3 finalDirection;
        if (randomVar > 0.5) {
            finalDirection = direction + ourPosition;
        }
        else {
            finalDirection = ourPosition - direction;
        }

        return finalDirection;
    }
}
