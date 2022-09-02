using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace XGStudios
{
    public class enemySwarm : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 4f)]
        float lerptime;
        [SerializeField]
        float spacing;
        // Start is called before the first frame update
<<<<<<< Updated upstream
        GameObject[] AIobjects;
=======
        List<GameObject> AIobjects;
        [SerializeField] [Range(0f, 4f)] float lerptime;
        [SerializeField] float spacing;
      [SerializeField] AISpawner mySpawner;
>>>>>>> Stashed changes

        void Start()
        {
            mySpawner = GameObject.FindObjectOfType<AISpawner>();
            AIobjects = mySpawner.enemies;
        }

        // Update is called once per frame
        void Update()
        {
<<<<<<< Updated upstream
            foreach (GameObject go in AIobjects)
                if (go != gameObject)
                {
                    float distance = Vector3.Distance(go.transform.position, transform.position);

                    if (distance <= spacing)
                    {
                        Vector3 dir = transform.position - go.transform.position;
                        Vector3 aiDirection = transform.position + dir;

                        transform.position = Vector3.MoveTowards(transform.position, aiDirection,
                            lerptime * Time.fixedDeltaTime);
=======
            func();
        }

        void func() {
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
>>>>>>> Stashed changes

                    }
                }
        }
    }
}
