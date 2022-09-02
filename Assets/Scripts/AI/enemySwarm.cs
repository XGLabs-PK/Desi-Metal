using UnityEngine;

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
        GameObject[] AIobjects;

        void Start()
        {
            AIobjects = GameObject.FindGameObjectsWithTag("AI");
        }

        // Update is called once per frame
        void Update()
        {
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

                    }
                }
        }
    }
}
