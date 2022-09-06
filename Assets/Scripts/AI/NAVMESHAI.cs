using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace XGStudios
{
    public class NAVMESHAI : MonoBehaviour
    {
        NavMeshAgent agent;
        [SerializeField] private Transform player;
        [SerializeField] float rotationSpeed;
        bool isCircling;
        [SerializeField] private float rammingTime;

        float  distanceBetweenplayer;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Car").transform;
            isCircling = true;
            rammingTime = Random.Range(10, 31);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
              distanceBetweenplayer = Vector3.Distance(agent.transform.position, player.position);
            rammingTime = rammingTime - Time.deltaTime;
            if (rammingTime <= 0) {
                StartCoroutine(Ramming());
            }
                if (distanceBetweenplayer <= agent.stoppingDistance && isCircling)
                {

                    transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
                    transform.LookAt(player);

                }
                else
                {
                    agent.SetDestination(player.position);
                }
            

            
             
        }
        IEnumerator Ramming() {
            //float stop = agent.stoppingDistance;
            agent.stoppingDistance = 0;
            agent.SetDestination(player.position);
            yield return new WaitUntil(()=> distanceBetweenplayer<=5);
            agent.stoppingDistance = 20; ;
            agent.SetDestination(player.position);
            rammingTime = Random.Range(10, 31);

        
        }
    }
}
