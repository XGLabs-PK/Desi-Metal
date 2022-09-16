using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using XG.Studios;

namespace XGStudios
{
    public class NavmeshAi : MonoBehaviour
    {
        static int _addScore;
        [HideInInspector]
        public Transform player;
        [SerializeField]
        float rotationSpeed;
        [SerializeField]
        float rammingTime;
        [Space(5f)]
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
        [SerializeField]
        AiBullet bullet;
        [SerializeField]
        GameObject bulletPrefab;
        [SerializeField]
        Transform shootPoint;
        [SerializeField]
        GameObject[] Carpoints;
        [Header("Health")]
        [SerializeField]
        public int health = 100;
        [SerializeField]
        GameObject deathParticle;
        public LayerMask isGround;
        public float groundRadius;
        [Header("Slope")]
        [SerializeField]
        Vector3 rayCastOffset;
        [SerializeField]
        float maxRay;
        [SerializeField]
        float slopSpeed = 10f;
        [SerializeField]
        float stopDist;
        NavMeshAgent _agent;
        float _distanceBetweenPlayer;
        bool _isShooting;
        int _killCounter;
        float _rateOfFire;
        TextMeshProUGUI killCounterTxt;
        RaycastHit slopeHit;
        readonly float yOffset = 23.7f;
        PoolManager pool;

        void Awake()
        {
            killCounterTxt = GameObject.Find("KillCounter").GetComponent<TextMeshProUGUI>();
            _agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("RealCar").GetComponent<Transform>();

            rammingTime = Random.Range(10, 31);
            _isShooting = true;
            Carpoints = new GameObject[3];
            Carpoints = GameObject.FindGameObjectsWithTag("ShotPoint");
            StartCoroutine(FOV());
            pool = FindObjectOfType<PoolManager>();
        }

        void Update()
        {
            if (Physics.Raycast(transform.position + rayCastOffset, Vector3.down, out slopeHit, maxRay, isGround))
            {
                Debug.DrawLine(transform.position + rayCastOffset, slopeHit.point, Color.red);
                Quaternion newRot = Quaternion.FromToRotation(transform.up, slopeHit.normal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, slopSpeed);
            }


            if (health <= 0)
            {
                GameManager.Score++;

                if (killCounterTxt != null)
                    killCounterTxt.text = GameManager.Score.ToString();

                int.TryParse(killCounterTxt.text, out _killCounter);
                PlayerPrefs.SetInt("KillCounter", _killCounter);
                Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                gameObject.SetActive(false);

                if (gameObject.name.Contains("Mehran"))
                    pool.mehranQueue.Enqueue(gameObject);
                else if (gameObject.name.Contains("Rickshaw"))
                    pool.RickshawQueue.Enqueue(gameObject);
                else
                    pool.truck.SetActive(false);

                AudioManager.Instance.Play("CarDestruction");
                FeelManager.Instance.enemyDestroyed.PlayFeedbacks();
            }
        }

        void FixedUpdate()
        {
            if (canSeePlayers && _isShooting)
                StartCoroutine(Shoot());

            _distanceBetweenPlayer = Vector3.Distance(_agent.transform.position, player.position);
            rammingTime -= Time.deltaTime;

            if (rammingTime <= 0)
                StartCoroutine(Ramming());

            if (!(_distanceBetweenPlayer <= _agent.stoppingDistance))
                _agent.SetDestination(player.position);

            else
            {
                transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
                transform.LookAt(player);
            }
        }

        IEnumerator Ramming()
        {
            float acce = _agent.acceleration;
            _agent.stoppingDistance = stopDist;
            Vector3 position = player.position;
            _agent.SetDestination(position);
            yield return new WaitUntil(() => _distanceBetweenPlayer <= 8);
            StartCoroutine(_MoveAway());
            _agent.stoppingDistance = 20;
            _agent.SetDestination(position);
            rammingTime = Random.Range(10, 31);
            _agent.stoppingDistance = 20;
        }

        void FieldOfViewSearch()
        {
            var rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, Layers);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    canSeePlayers = !Physics.Raycast(transform.position, directionToTarget,
                        distanceToTarget, occlusionLayers);
                }
                else
                    canSeePlayers = false;
            }
            else if (canSeePlayers)
                canSeePlayers = false;
        }

        IEnumerator FOV()
        {
            while (true)
            {
                yield return new WaitForSeconds(searchDelay);
                FieldOfViewSearch();
            }
        }

        IEnumerator Shoot()
        {
            _isShooting = false;

            switch (_distanceBetweenPlayer)
            {
                case > 20:
                    bullet.bulletSpeed = 150;
                    _rateOfFire = 1f;
                    shootPoint.LookAt(Carpoints[Random.Range(0, 3)].transform);
                    break;
                case > 10:
                    bullet.bulletSpeed = 170;
                    _rateOfFire = 0.8f;
                    shootPoint.LookAt(Carpoints[Random.Range(0, 3)].transform);
                    break;
                default:
                    bullet.bulletSpeed = 200;
                    _rateOfFire = 0.5f;
                    shootPoint.LookAt(player);
                    break;
            }

            bullet.SendData(shootPoint);
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(_rateOfFire);
            _isShooting = true;
        }

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;
        }

        Vector3 findPoint()
        {
            Vector3 point = new Vector3(player.transform.position.x + Random.Range(100, 300), yOffset,
                player.transform.position.z + Random.Range(100, 300));

            return point;

        }

        IEnumerator _MoveAway()
        {
            //NavMeshHit hitit;
            //if (UnityEngine.AI.NavMesh.SamplePosition(findPoint(), out hitit, 300f, UnityEngine.AI.NavMesh.AllAreas)) {
            //    _agent.SetDestination(hitit.position);
            //}
            Vector3 myPoint = findPoint();
            _agent.SetDestination(myPoint);
            yield return new WaitForSeconds(5);

        }
    }
}
