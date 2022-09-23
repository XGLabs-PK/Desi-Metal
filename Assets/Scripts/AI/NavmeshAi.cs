using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using XG.Studios;

namespace XGStudios
{
    public class NavmeshAi : MonoBehaviour
    {
        Collider[] rangeChecks;
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
        Transform myTransform;
        GameObject myGameobject;
        Quaternion newRot;
        Transform rTarget;
        Vector3 directionToTarget;
        float distanceToTarget;
        Vector3 findPointPoint;
        Vector3 moveAwayPoint;
        WaitForSeconds moveAwayTime;
        float myStop;
       public bool isMoving = false;
        public  int myCoount = 0;
        [SerializeField]NavMeshObstacle playerObstacle;
        private void Start()
        {
            killCounterTxt = GameObject.Find("KillCounter").GetComponent<TextMeshProUGUI>();
            _agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("RealCar").GetComponent<Transform>();
            playerObstacle = GameObject.FindGameObjectWithTag("RealCar").GetComponent<NavMeshObstacle>();

            rammingTime = Random.Range(10, 31);
            _isShooting = true;
            Carpoints = new GameObject[3];
            Carpoints = GameObject.FindGameObjectsWithTag("ShotPoint");
            StartCoroutine(FOV());
            pool = FindObjectOfType<PoolManager>();
            myTransform = transform;
            myGameobject = gameObject;
            moveAwayTime = new WaitForSeconds(1.5f);
            myStop = _agent.stoppingDistance;
            
        }
        void Update()
        {
            if (Physics.Raycast(myTransform.position + rayCastOffset, Vector3.down, out slopeHit, maxRay, isGround))
            {
                Debug.DrawLine(myTransform.position + rayCastOffset, slopeHit.point, Color.red);
                newRot = Quaternion.FromToRotation(myTransform.up, slopeHit.normal) * myTransform.rotation;
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, newRot, slopSpeed);
            }


            if (health <= 0)
            {
                if (CarController.AbilityUsed)
                    GameManager.Score += 2;
                else
                    GameManager.Score++;

                if (killCounterTxt != null)
                    killCounterTxt.text = GameManager.Score.ToString();

                int.TryParse(killCounterTxt.text, out _killCounter);
                PlayerPrefs.SetInt("KillCounter", _killCounter);              
                if (myGameobject.name.Contains("Mehran"))
                    pool.mehranQueue.Enqueue(myGameobject);
                else if (myGameobject.name.Contains("Rickshaw"))
                    pool.RickshawQueue.Enqueue(myGameobject);
                else
                    pool.truck.SetActive(false);

                AudioManager.Instance.Play("CarDestruction");
                FeelManager.Instance.enemyDestroyed.PlayFeedbacks();
                if (myGameobject.name.Contains("Rickshaw"))
                {
                    health = 100;
                }
                else if (myGameobject.name.Contains("Mehran"))
                {
                    health = 200;
                }
                else
                    health = 300;
                myGameobject.SetActive(false);
                 
                
            }
            if (health <= 50 && myCoount == 0) {
                myCoount++;
                _agent.speed = _agent.speed * 1.5f;
            }
        }

        void FixedUpdate()
        {
            if (canSeePlayers && _isShooting)
                StartCoroutine(Shoot());

            _distanceBetweenPlayer = Vector3.Distance(myTransform.position, player.position);
            rammingTime -= Time.deltaTime;

            if (rammingTime <= 0)
               Ramming();

            
            _agent.SetDestination(player.position);

            if(_distanceBetweenPlayer<=_agent.stoppingDistance)
            {
                myTransform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
                myTransform.LookAt(player);
            }
        }
   
        void Ramming()
        {
            playerObstacle.enabled = false;
            _agent.stoppingDistance = stopDist; 
           if(_distanceBetweenPlayer <= stopDist+0.5) { 
            StartCoroutine(MoveAway());
            _agent.stoppingDistance = myStop;
            rammingTime = Random.Range(10, 31);
            }
        }

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
            findPointPoint = new Vector3(player.transform.position.x + Random.Range(100, 300), yOffset,
                player.transform.position.z + Random.Range(100, 300));

            return findPointPoint;

        }

        IEnumerator MoveAway()
        {
            isMoving = true;
            moveAwayPoint = findPoint();
            _agent.SetDestination(moveAwayPoint);
            yield return moveAwayTime;
            isMoving = false;
            playerObstacle.enabled = true ;

        }
    }
}
