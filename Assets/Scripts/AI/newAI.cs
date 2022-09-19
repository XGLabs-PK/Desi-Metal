using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace XGStudios
{
    public class newAI : MonoBehaviour
    {
        NavMeshAgent agent;
        Collider[] rangeChecks;
        Transform rTarget;
        Vector3 directionToTarget;
        float distanceToTarget;
        Transform myTransform;
        [SerializeField] float detectionRadius;
        [SerializeField] LayerMask Layers;
        [SerializeField] LayerMask occlusionLayers;
        bool canSeePlayers;
        [SerializeField] float viewAngle;
        WaitForSeconds delay;
        int health = 100;
        [SerializeField] Vector3 rayCastOffset;
        RaycastHit slopeHit;
        float maxRay = 20f;
        Quaternion newRot;
        float slopSpeed = 4f;
        [SerializeField] LayerMask isGround;
        Transform player;
        [SerializeField]float rotationSpeed;
        float distanceBetween;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("RealCar").GetComponent<Transform>();
            delay = new WaitForSeconds(0.1f);
            agent = GetComponent<NavMeshAgent>();
            myTransform = transform;
            StartCoroutine(fov());
        }
        private void FixedUpdate()
        {
            distanceBetween = Vector3.Distance(player.position, myTransform.position);
            if (Physics.Raycast(myTransform.position + rayCastOffset, Vector3.down, out slopeHit, maxRay, isGround))
            {
                Debug.DrawLine(myTransform.position + rayCastOffset, slopeHit.point, Color.red);
                newRot = Quaternion.FromToRotation(myTransform.up, slopeHit.normal) * myTransform.rotation;
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, newRot, slopSpeed);
            }
            if (distanceBetween > agent.stoppingDistance)
                agent.SetDestination(player.position);
            else if (distanceBetween <=agent.stoppingDistance) {
                myTransform.RotateAround(player.position,Vector3.up,rotationSpeed*Time.deltaTime);
                myTransform.LookAt(player);
            }

        }
        // Update is called once per frame
        void FieldOfViewSearch()
        {
            rangeChecks = Physics.OverlapSphere(myTransform.position, detectionRadius, Layers);

            if (rangeChecks.Length != 0)
            {
                rTarget = rangeChecks[0].transform;
                directionToTarget = (rTarget.position - myTransform.position).normalized;

                if (Vector3.Angle(myTransform.forward, directionToTarget) < viewAngle / 2)
                {
                    distanceToTarget = Vector3.Distance(myTransform.position, rTarget.position);

                    canSeePlayers = !Physics.Raycast(myTransform.position, directionToTarget,
                        distanceToTarget, occlusionLayers);
                }
                else
                    canSeePlayers = false;
            }
            else if (canSeePlayers)
                canSeePlayers = false;
        }
        IEnumerator fov() {
            while (true) {
                yield return delay;
                FieldOfViewSearch();
            }
        }
        public void TakeDamage(int damageAmount) {
            health -= damageAmount;
        }
    }
   
}
