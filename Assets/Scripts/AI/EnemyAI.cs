using System.Collections;
using UnityEngine;

namespace XGStudios
{
    public class EnemyAI : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        public GameObject followObject;
        [Header("Normal AI Settings")]
        bool isChasing;
        [SerializeField]
        float stoppingDistance;
        [SerializeField]
        float speed = 1;
        [Header("Circling Around Settings")]
        [SerializeField]
        float circleSpeed;
        [SerializeField]
        float radiusAroundTarget;
        [SerializeField]
        float rotationSpeed = 1;
        bool isCircling;
        [Header("Field of view")]
        [SerializeField]
        public float detectionRadius = 10;
        [Range(0, 360)]
        [SerializeField]
        public float viewAngle = 30f;
        [SerializeField]
        public float searchDelay;
        public int scanFrequency = 30;
        public LayerMask Layers;
        public LayerMask occlusionLayers;
        public bool canSeePlayers;
        [SerializeField]
        AiBullet bullet;
        [SerializeField]
        GameObject enemy;
        [SerializeField]
        Transform shootPoint;
        [SerializeField]
        GameObject bulletPrefab;
        [SerializeField]
        Transform[] flipPointCheck;
        public float groundRadius;
        public LayerMask isGround;
        public Rigidbody myBody;
        [Header("Shooting")]
        bool shooting;

        Vector3 direction;
        [SerializeField] Terrain myTerrain;

        void Start()
        {
            isChasing = true;
            followObject = GameObject.FindGameObjectWithTag("Car");
            shooting = true;
            StartCoroutine(fov());
            myTerrain = GameObject.FindObjectOfType<Terrain>();
            myBody = GetComponent<Rigidbody>();
        }
        // Update is called once per frame

        private void FixedUpdate()
        {
           
        }
        void Update()
        {
            

            float distance = Vector3.Distance(transform.position, followObject.transform.position);

            if (distance >= stoppingDistance && isChasing)
            {
                isCircling = false;
                //transform.position = Vector3.MoveTowards(transform.position, followObject.transform.position,
                //   speed * Time.fixedDeltaTime);
                direction = (followObject.transform.position - transform.position);
                myBody.AddForce(direction * speed * Time.deltaTime);

                transform.LookAt(followObject.transform);

            }
            else if (distance < stoppingDistance)
            {
                isCircling = true;
                moveToPoint(followObject.transform, radiusAroundTarget);
                transform.RotateAround(followObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
               
                transform.LookAt(followObject.transform);
            }
            if (canSeePlayers && shooting)
                StartCoroutine(shoot());

        }
        IEnumerator stopChasing()
        {
            isChasing = false;
            float xRand;
            float yVal;
            float zRand;
            xRand = Random.Range(myTerrain.transform.position.x, myTerrain.transform.position.x + myTerrain.terrainData.size.x);
            zRand = Random.Range(myTerrain.transform.position.z, myTerrain.transform.position.z + myTerrain.terrainData.size.z);
            yVal = myTerrain.SampleHeight(new Vector3(xRand, 0, zRand));
            //yVal = yVal + yOffset;
            //transform.position = Vector3.MoveTowards(transform.position, aiDirection, Time.fixedDeltaTime);
            Vector3 point = new Vector3(xRand, followObject.transform.position.y, zRand);
            Vector3 dir = point - transform.position;
            myBody.velocity = dir * (speed ) * Time.deltaTime;
            yield return new WaitForSeconds(1.3f);
            isChasing = true;

        }
        void OnCollisionEnter(Collision collision)
        {
            

            if (collision.gameObject.tag.Equals("obstruction"))
            {
                Debug.Log("colliding OBSTRUCTION");
                StartCoroutine(stopChasing());

            }


        }


        IEnumerator fov()
        {
            while (true)
            {
                yield return new WaitForSeconds(searchDelay);
                fieldOfviewSerch();
            }
        }


        void fieldOfviewSerch()
        {
            //var rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, playerMask);
            var rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, Layers);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);


                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, occlusionLayers))
                        canSeePlayers = true;
                    else
                        canSeePlayers = false;
                }
                else
                    canSeePlayers = false;
            }
            else if (canSeePlayers)
                canSeePlayers = false;
        }

        public bool isFlipped()
        {
            if (Physics.CheckSphere(flipPointCheck[0].position, groundRadius, isGround))
                return true;

            if (Physics.CheckSphere(flipPointCheck[1].position, groundRadius, isGround))
                return true;

            if (Physics.CheckSphere(flipPointCheck[2].position, groundRadius, isGround))
                return true;

            return false;
        }


        void moveToPoint(Transform target, float radius)
        {
            float angle = Random.Range(0, 360);

            Vector3 myPoint = new Vector3(target.position.x + radius * Mathf.Cos(angle), 0,
                target.position.z + radius * Mathf.Sin(angle));

            //Vector3.Lerp(transform.position, myPoint, circleSpeed * Time.deltaTime);
            Vector3 dir = (myPoint - transform.position).normalized;
            myBody.velocity = (dir * circleSpeed);
        }

        float findDisplacement()
        {
            float bulletDistance = Vector3.Distance(shootPoint.position, followObject.transform.position);

            if (bulletDistance > 20)
                return Random.Range(0.2f, 0.25f);

            if (bulletDistance > 10)
                return Random.Range(0.1f, 0.15f);

            if (bulletDistance > 5)
                return Random.Range(0.05f, 0.025f);

            return 0;
        }

        IEnumerator shoot()
        {
            bullet.SendData(followObject, findDisplacement(), shootPoint);
            shooting = false;
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            shooting = true;

        }
        //private void OnCollisionExit(Collision collision)
        //{
        //    Rigidbody rigid = transform.GetComponent<Rigidbody>();
        //    rigid.isKinematic = false;
        //}
    }
}
