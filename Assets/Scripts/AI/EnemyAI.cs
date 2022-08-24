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
        private void Start()
        {
            followObject = GameObject.FindGameObjectWithTag("Body");
        }
        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(transform.position, followObject.transform.position);
           
            if (distance >= stoppingDistance) {
              transform.position =  Vector3.Lerp(transform.position, followObject.transform.position, speed * Time.fixedDeltaTime);
            }
          
        }
        public void shoot() { }
    }
}
