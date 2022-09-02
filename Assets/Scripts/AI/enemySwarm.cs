using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace XGStudios
{
    public class enemySwarm : MonoBehaviour
    {
        // Start is called before the first frame update

        List<GameObject> AIobjects;
        [SerializeField] [Range(0f, 4f)] float lerptime;
        [SerializeField] float spacing;
        [SerializeField] AISpawner mySpawner;

        void Start()
        {
            mySpawner = GameObject.FindObjectOfType<AISpawner>();
            AIobjects = mySpawner.enemies;
        }

        // Update is called once per frame
        void Update()
        {
            func();
        }

        void func()
        {
            foreach (GameObject go in AIobjects)
            {
                if (go != gameObject && go != null)
                {
                    float distance = Vector3.Distance(go.transform.position, transform.position);
                    if (distance <= spacing)
                    {
                        Vector3 dir = transform.position - go.transform.position;
                        Vector3 aiDirection = (transform.position) + dir;
                        transform.position = Vector3.MoveTowards(transform.position, aiDirection, lerptime * Time.fixedDeltaTime);

                    }
                }
            }
        }
    }
}
