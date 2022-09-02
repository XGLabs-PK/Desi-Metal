using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class EnemyAI : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        public GameObject followObject;
        [Header("Normal AI Settings")]
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
        [Header("Field of view")]
        public LayerMask playerMask;
        public LayerMask obstructions;
        [SerializeField] public float detectionRadius = 10;
        [Range(0, 360)]
        [SerializeField] public float viewAngle = 30f;
        [SerializeField] public float searchDelay;
        public int scanFrequency = 30;
        public LayerMask Layers;
        public LayerMask occlusionLayers;
        public bool canSeePlayers;
        [Header("Shooting")]
        bool shooting;
        [SerializeField] AiBullet bullet;
        [SerializeField] GameObject enemy;
        [SerializeField] Transform shootPoint;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform[] flipPointCheck;
        public float groundRadius;
        public LayerMask isGround;

        void Start()
        {
            followObject = GameObject.FindGameObjectWithTag("Car");
            shooting = true;
            StartCoroutine(fov());

        }



        private IEnumerator fov()
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
                Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, Layers);

                if (rangeChecks.Length != 0)
                {
                    Transform target = rangeChecks[0].transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);


                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructions))

                            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, occlusionLayers))
                            {
                                canSeePlayers = true;
                            }
                            else
                                canSeePlayers = false;
                    }
                    else
                        canSeePlayers = false;
                }
                else if (canSeePlayers)
                    canSeePlayers = false;
            }

            public bool isFlipped() {
                if (Physics.CheckSphere(flipPointCheck[0].position, groundRadius, isGround))
                    return true;
                if (Physics.CheckSphere(flipPointCheck[1].position, groundRadius, isGround))
                    return true;
                if (Physics.CheckSphere(flipPointCheck[2].position, groundRadius, isGround))
                    return true;
                else
                    return false;
            }
            // Update is called once per frame

            void Update()
            {
                float distance = Vector3.Distance(transform.position, followObject.transform.position);

                if (distance >= stoppingDistance)
                {
                    transform.position = Vector3.Lerp(transform.position, followObject.transform.position, speed * Time.fixedDeltaTime);
                    transform.LookAt(followObject.transform);
                }
                else
                {
                    moveToPoint(followObject.transform, radiusAroundTarget);
                    transform.RotateAround(followObject.transform.position, -Vector3.up, rotationSpeed * Time.deltaTime);
                    transform.LookAt(followObject.transform);

                }
                if (canSeePlayers && shooting) {
                    StartCoroutine(shoot());

                }

            }


            void moveToPoint(Transform target, float radius)
            {
                float angle = Random.Range(0, 360);

                Vector3 myPoint = new Vector3(target.position.x + radius * Mathf.Cos(angle), target.position.y,
                    target.position.z + radius * Mathf.Sin(angle));

                Vector3.Lerp(transform.position, myPoint, circleSpeed * Time.deltaTime);
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
            private void OnCollisionEnter(Collision collision)
            {
                Rigidbody rigid = transform.GetComponent<Rigidbody>();
                rigid.isKinematic = true;

            }
            private void OnCollisionExit(Collision collision)
            {
                Rigidbody rigid = transform.GetComponent<Rigidbody>();
                rigid.isKinematic = false;
            }
        }

    } 

