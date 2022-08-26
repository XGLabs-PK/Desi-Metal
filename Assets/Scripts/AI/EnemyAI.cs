using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XGStudios.GameScene
{
    
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] GameObject followObject;
        [Header("Normal AI Settings")]
        [SerializeField] float stoppingDistance;
        [SerializeField] float speed = 1;
        [Header("Circling Around Settings")]
        [SerializeField] float circleSpeed;
        [SerializeField] float radiusAroundTarget;
        [SerializeField] float rotationSpeed = 1;
        private void Start()
        {
            followObject = GameObject.FindGameObjectWithTag("Body");
        }
        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(transform.position, followObject.transform.position);

            if (distance >= stoppingDistance)
            {
                transform.position = Vector3.Lerp(transform.position, followObject.transform.position, speed * Time.fixedDeltaTime);
            }
            else 
            {
                //foreach (GameObject go in myAI)
                //{
                //    if (go != gameObject)
                //    {
                //        go.transform.position = Vector3.Lerp(go.transform.position, calculatePoint(followObject.transform, radiusAroundTarget), speed * Time.deltaTime);
                //    }
                //}

                //transform.position = Vector3.Lerp(transform.position, calculatePoint(followObject.transform,radiusAroundTarget), circleSpeed * Time.deltaTime);
                moveToPoint(followObject.transform,radiusAroundTarget);
                transform.RotateAround(followObject.transform.position, Vector3.up, rotationSpeed*Time.deltaTime);
            }
          
        }
       
       void moveToPoint(Transform target,float radius)
        {
            float angle = Random.Range(0, 360);
            Vector3 myPoint = new Vector3(target.position.x + (radius * Mathf.Cos(angle)), target.position.y, target.position.z + (radius * Mathf.Sin(angle)));
            //return myPoint;
            Vector3.Lerp(transform.position, myPoint, circleSpeed * Time.deltaTime);
        }

    }
   
}

