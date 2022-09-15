using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace XGStudios
{
    public class AISpawner : MonoBehaviour
    {
        public GameObject mehranEnemy;
        public GameObject rickshawEnemy;
        public GameObject truckEnemy;
        public List<GameObject> enemies;
        public float yOffset;
        public int enemyCount = 2;
        public GameObject target;
        [SerializeField]
        Terrain terrain;
        List<NavmeshAi> _ai;
        int _wave;
        TextMeshProUGUI enemiesLeft;

        void Awake()
        {
            enemiesLeft = GameObject.FindGameObjectWithTag("LeftCounter").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            _wave = 1;
            enemies = new List<GameObject>();
            _ai = new List<NavmeshAi>();
            target = GameObject.FindGameObjectWithTag("RealCar");

            StartCoroutine(Delay());

            for (int i = 0; i < enemyCount; i++)
                if (NavMesh.SamplePosition(findPoint(), out NavMeshHit hit, 300f, NavMesh.AllAreas))
                    enemies.Add(Instantiate(rickshawEnemy, hit.position, Quaternion.identity));

            for (int i = 0; i < enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        void Update()
        {
            if (enemiesLeft != null)
                enemiesLeft.text = enemies.Count.ToString();

            if (enemies.Count != 0)
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (_ai[i] == null) continue;
                    if (_ai[i].health > 0) continue;
                    enemies.RemoveAt(i);
                    _ai.RemoveAt(i);
                }

            if (enemies.Count != 0) return;

            Debug.Log("Enemies empty");
            Debug.Log(_wave);
            enemies.Clear();
            _ai.Clear();

            switch (_wave)
            {
                case 1:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy, 2, 0);
                    _wave++;
                    break;
                case 2:
                    InstantiateEnemies(rickshawEnemy, 2, 5);
                    _wave++;
                    break;
                case 3:
                    InstantiateEnemies(mehranEnemy, 3, 2);
                    _wave++;
                    break;
                case 4:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy, 5, 2);
                    _wave++;
                    break;
                case 5:
                    InstantiateEnemies(truckEnemy, 0, 1);
                    _wave = 1;
                    break;
            }

        }

        void InstantiateEnemies(GameObject enemy, int moreEnemy, int enemyPlus)
        {
            for (int i = 0; i < moreEnemy + enemyPlus; i++)
                if (NavMesh.SamplePosition(findPoint(), out NavMeshHit hit, 300f, NavMesh.AllAreas))
                    enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));

            for (int i = 0; i < enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        void InstantiateEnemies(GameObject enemy, GameObject enemy2, int moreEnemy, int enemyPlus)
        {
            for (int i = 0; i < moreEnemy + enemyPlus; i++)
            {
                NavMeshHit hit;

                if (RandomBoolean())
                {
                    if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                        enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
                }
                else
                {
                    if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                        enemies.Add(Instantiate(enemy2, hit.position, Quaternion.identity));
                }
            }

            for (int i = 0; i < enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        static bool RandomBoolean()
        {
            return Random.value > 0.5;
        }

        Vector3 findPoint()
        {
            Vector3 point = new Vector3(target.transform.position.x + Random.Range(100, 300), yOffset,
                target.transform.position.z + Random.Range(100, 300));

            return point;

        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(5);

        }
    }
}
