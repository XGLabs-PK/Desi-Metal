using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace XGStudios
{
    public class NavmeshAi : MonoBehaviour
    {
        TextMeshProUGUI killCounterTxt;
        NavMeshAgent _agent;
        [HideInInspector]
        public Transform player;
        [SerializeField] float rotationSpeed;
        bool _isCircling;
        [SerializeField] float rammingTime;
        float  _distanceBetweenPlayer;
        
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
        [SerializeField]AiBullet bullet;
        [SerializeField]GameObject bulletPrefab;
        bool _isShooting;
        [SerializeField] Transform shootPoint;
        [SerializeField] GameObject[] Carpoints;
        float _rateOfFire;
        [Header("Health")]
        [SerializeField] public int health = 100;
        [SerializeField] GameObject deathParticle;
        public LayerMask isGround;
        public float groundRadius;
        static int _addScore;
        int _killCounter;
        [Header("Slope")]
        [SerializeField]
        Vector3 rayCastOffset;
        [SerializeField]
        float maxRay;
        [SerializeField]
        float slopSpeed = 10f;
        RaycastHit slopeHit;

        void Awake()
        {
            killCounterTxt = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
            _addScore = 0;
            _agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Car").transform;
            _isCircling = true;
            rammingTime = Random.Range(10, 31);
            _isShooting = true;
            Carpoints = new GameObject[3];
            Carpoints = GameObject.FindGameObjectsWithTag("ShotPoint");
            StartCoroutine(FOV());
        }
      
        
        void FixedUpdate()
        {
            if (canSeePlayers && _isShooting)
            {
                StartCoroutine(Shoot());
            }
            _distanceBetweenPlayer = Vector3.Distance(_agent.transform.position, player.position);
            rammingTime = rammingTime - Time.deltaTime;
            if (rammingTime <= 0) {
                StartCoroutine(Ramming());
            }
            if (!(_distanceBetweenPlayer <= _agent.stoppingDistance) || !_isCircling)
                _agent.SetDestination(player.position);
            else
            {
                transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
                transform.LookAt(player);
            }
            
        
        }
        
        void Update()
        {
            if (Physics.Raycast(transform.position + rayCastOffset, Vector3.down, out slopeHit, maxRay, isGround))
            {
                Debug.DrawLine(transform.position + rayCastOffset, slopeHit.point, Color.red);
                Quaternion newRot = Quaternion.FromToRotation(transform.up, slopeHit.normal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot,slopSpeed);
            }
            if (health > 0) return;
            
            _addScore++;
            if (killCounterTxt != null)
                killCounterTxt.text = _addScore.ToString();
            int.TryParse(killCounterTxt.text, out _killCounter);
            PlayerPrefs.SetInt("KillCounter", _killCounter);
            
            Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
            gameObject.SetActive(false);
            Destroy(gameObject, 3f);
            FeelManager.Instance.enemyDestroyed.PlayFeedbacks();
        }
        
        IEnumerator Ramming() {
            _agent.stoppingDistance = 0;
            Vector3 position = player.position;
            _agent.SetDestination(position);
            yield return new WaitUntil(()=> _distanceBetweenPlayer<=5);
            _agent.stoppingDistance = 20;
            _agent.SetDestination(position);
            rammingTime = Random.Range(10, 31);
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
                        distanceToTarget,occlusionLayers);
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
        
        IEnumerator Shoot() {
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
        public void TakeDamage(int damageAmount) {
            health -= damageAmount;
        }
    }
}
