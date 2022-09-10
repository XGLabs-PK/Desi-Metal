using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace XGStudios
{
    public class EnemySwarm : MonoBehaviour
    {
        [SerializeField]
        float lerpTime;
        [SerializeField]
        float spacing;
        [SerializeField]
        AISpawner mySpawner;
        Rigidbody _myBody;

        List<GameObject> _aiObjects;

        void Start()
        {
            mySpawner = FindObjectOfType<AISpawner>();
            _aiObjects = mySpawner.enemies;
            _myBody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            Func();
        }

        void Func()
        {
            foreach (Vector3 dir in from go in _aiObjects where go != gameObject && go != null let distance =
                         Vector3.Distance(go.transform.position, transform.position) where distance <= spacing select transform.position
                         - go.transform.position into dir let aiDirection = transform.position + dir select dir) 
                _myBody.velocity = dir * lerpTime * Time.deltaTime;
        }
    }
}
