using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios.GameScene
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] GameObject followObject;
        [SerializeField] float stoppingDistance;
        [SerializeField] float speed = 1;
        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(transform.position, followObject.transform.position);
            Debug.Log(distance);
            if (distance >= stoppingDistance) {
              transform.position =  Vector3.MoveTowards(transform.position, followObject.transform.position, speed * Time.deltaTime);
            }
          
        }
        public void shoot() { }
    }
}
