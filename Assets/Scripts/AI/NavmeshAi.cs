using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace XGStudios
{
    public class NavmeshAi : MonoBehaviour
    {
        NavMeshAgent _agent;
        Transform _player;
        [SerializeField] float rotationSpeed;
        bool _isCircling;
        [SerializeField] float rammingTime;

        float  _distanceBetweenPlayer;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Car").transform;
            _isCircling = true;
            rammingTime = Random.Range(10, 31);
        }
        
       void FixedUpdate()
        {
              _distanceBetweenPlayer = Vector3.Distance(_agent.transform.position, _player.position);
            rammingTime = rammingTime - Time.deltaTime;
            if (rammingTime <= 0) {
                StartCoroutine(Ramming());
            }
            if (!(_distanceBetweenPlayer <= _agent.stoppingDistance) || !_isCircling)
                _agent.SetDestination(_player.position);
            else
            {
                transform.RotateAround(_player.position, Vector3.up, rotationSpeed * Time.deltaTime);
                transform.LookAt(_player);
            }


        }
        IEnumerator Ramming() {
            //float stop = agent.stoppingDistance;
            _agent.stoppingDistance = 0;
            Vector3 position = _player.position;
            _agent.SetDestination(position);
            yield return new WaitUntil(()=> _distanceBetweenPlayer<=5);
            _agent.stoppingDistance = 20; ;
            _agent.SetDestination(position);
            rammingTime = Random.Range(10, 31);
        }
    }
}
