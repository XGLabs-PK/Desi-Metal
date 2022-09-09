using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace XGStudios
{
    public class NavmeshAi : MonoBehaviour
    {
        NavMeshAgent _agent;
       public Transform _player;
        [SerializeField] float rotationSpeed;
        bool _isCircling;
        [SerializeField] float rammingTime;
        float  _distanceBetweenPlayer;
        [Header("Field of view")]
        [SerializeField]
        public float detectionRadius = 10;
        [Range(0, 360)]
        [SerializeField]
        public float viewAngle = 30f;
        [SerializeField]
        public float searchDelay;
        public LayerMask Layers;
        public LayerMask occlusionLayers;
        public bool canSeePlayers;
        [Header("Shooting")]
        [SerializeField]AiBullet bullet;
        [SerializeField]GameObject bulletPrefab;
        bool isShooting;
        [SerializeField]Transform shootPoint;
        [SerializeField]GameObject[] Carpoints;
        float rateOfFire = 0;
        [Header("Health")]
        [SerializeField] public int health = 100;
        [SerializeField] GameObject deathParticle;
        public LayerMask isGround;
        [SerializeField]
        GameObject[] flipPointCheck;
        public float groundRadius;
        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Car").transform;
            _isCircling = true;
            rammingTime = Random.Range(10, 31);
            isShooting = true;
            Carpoints = new GameObject[3];
            Carpoints = GameObject.FindGameObjectsWithTag("ShotPoint");
            StartCoroutine(fov());
            flipPointCheck = new GameObject[3];
            flipPointCheck = GameObject.FindGameObjectsWithTag("Flip");

        }
        public bool isFlipped()
        {
            if (Physics.CheckSphere(flipPointCheck[0].transform.position, groundRadius, isGround))
            {
                Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                gameObject.SetActive(false);
                Destroy(gameObject, 5f);
                return true;

            }


            if (Physics.CheckSphere(flipPointCheck[1].transform.position, groundRadius, isGround))
            {
                Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                gameObject.SetActive(false);
                Destroy(gameObject, 5f);
                return true;
            }


            if (Physics.CheckSphere(flipPointCheck[2].transform.position, groundRadius, isGround))
            {
                Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                gameObject.SetActive(false);
                Destroy(gameObject, 5f);
                return true;
            }
                

            return false;
        }
        void FixedUpdate()
        {
            if (canSeePlayers && isShooting)
            {
                StartCoroutine(shoot());
            }
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
        private void Update()
        {
            if (health <= 0)
            {
                Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                gameObject.SetActive(false);
                Destroy(gameObject, 5f);

            }

        }
        IEnumerator Ramming() {
            //float stop = agent.stoppingDistance;
            _agent.stoppingDistance = 0;
            Vector3 position = _player.position;
            _agent.SetDestination(position);
            yield return new WaitUntil(()=> _distanceBetweenPlayer<=5);
            _agent.stoppingDistance = 20;
            _agent.SetDestination(position);
            rammingTime = Random.Range(10, 31);
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
        IEnumerator fov()
        {
            while (true)
            {
                yield return new WaitForSeconds(searchDelay);
                fieldOfviewSerch();
            }
        }
        IEnumerator shoot() {
            isShooting = false;
            if (_distanceBetweenPlayer > 20)
            {
                bullet.bulletSpeed = 150;
                rateOfFire = 1f;
                shootPoint.LookAt(Carpoints[Random.Range(0, 3)].transform);
            }
            else if (_distanceBetweenPlayer > 10)
            {
                bullet.bulletSpeed = 170;
                rateOfFire = 0.8f;
                shootPoint.LookAt(Carpoints[Random.Range(0, 3)].transform);
            }
            else {
                bullet.bulletSpeed = 200;
                rateOfFire = 0.5f;
                shootPoint.LookAt(_player);
            }
            bullet.SendData(shootPoint);
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(rateOfFire);
            isShooting = true;
        }
        public void TakeDamage(int damageAmount) {
            health -= damageAmount;
        }

    }

}
