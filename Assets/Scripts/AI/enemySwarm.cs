using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class enemySwarm : MonoBehaviour
    {
        [SerializeField]
        float lerptime;
        [SerializeField]
        float spacing;
        [SerializeField]
        AISpawner mySpawner;
        Rigidbody myBody;
        // Start is called before the first frame update

        List<GameObject> AIobjects;

        void Start()
        {
            mySpawner = FindObjectOfType<AISpawner>();
            AIobjects = mySpawner.enemies;
            myBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            func();
        }

        void func()
        {
            foreach (GameObject go in AIobjects)
                if (go != gameObject && go != null)
                {
                    float distance = Vector3.Distance(go.transform.position, transform.position);

                    if (distance <= spacing)
                    {
                        Vector3 dir = transform.position - go.transform.position;
                        Vector3 aiDirection = transform.position + dir;

                        //transform.position = Vector3.MoveTowards(transform.position, aiDirection,
                        //    lerptime * Time.fixedDeltaTime);
                        myBody.velocity = dir * lerptime * Time.deltaTime;

                    }
                }
        }
    }
}
