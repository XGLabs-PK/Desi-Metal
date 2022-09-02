using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] public GameObject followObject;
        [Header("Normal AI Settings")]
        [SerializeField] float stoppingDistance;
        [SerializeField] float speed = 1;
        [Header("Circling Around Settings")]
        [SerializeField] float circleSpeed;
        [SerializeField] float radiusAroundTarget;
        [SerializeField] float rotationSpeed = 1;
        [Header("Field of view")]
        [SerializeField]  public float detectionRadius;
        [Range(0,360)]
        [SerializeField] public float viewAngle;
        [SerializeField] float searchDelay = 0.1f;
        public LayerMask playerMask;
        public LayerMask obstructions;
        public bool canSeePlayers;
        bool shooting;
        [SerializeField]AiBullet bullet;
        [SerializeField]GameObject enemy;
   
    [SerializeField]GameObject bulletPrefab;

        private void Start()
        {
            followObject = GameObject.FindGameObjectWithTag("Body");
       // bullet = fin
        
        shooting = true;
            StartCoroutine(fov());
        }
        private IEnumerator fov() {
            while (true) {
                yield return new WaitForSeconds(searchDelay);
                fieldOfviewSerch();
            }
        }

        private void fieldOfviewSerch()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, playerMask);
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructions))
                    {
                        canSeePlayers = true;

                    }
                    else
                        canSeePlayers = false;
                }
                else
                {
                    canSeePlayers = false;
                }
            }
            else if (canSeePlayers)
                canSeePlayers = false;
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
                moveToPoint(followObject.transform,radiusAroundTarget);
                transform.RotateAround(followObject.transform.position, -transform.up, rotationSpeed*Time.deltaTime);
            
            }
        if (canSeePlayers && shooting) {
            StartCoroutine(shoot());
        
        }
          
        }
       
       void moveToPoint(Transform target,float radius)
        {
            float angle = Random.Range(0, 360);
            Vector3 myPoint = new Vector3(target.position.x + (radius * Mathf.Cos(angle)), target.position.y, target.position.z + (radius * Mathf.Sin(angle)));
            Vector3.Lerp(transform.position, myPoint, circleSpeed * Time.deltaTime);
        }
    float findDisplacement() {
        float bulletDistance = Vector3.Distance(transform.position, followObject.transform.position);
        if (bulletDistance > 20)
        {
            return Random.Range(0.5f, 0.8f);
        }
        else if (bulletDistance > 10)
        {
            return Random.Range(0.3f, 0.4f);
        }
        else if (bulletDistance > 5) {
            return Random.Range(0.1f,0.2f);
        }
        return 0;
    }
    IEnumerator shoot() {
        bullet.moveToTarget(followObject,findDisplacement());
        shooting = false;
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        shooting = true;

    }

    }
   


