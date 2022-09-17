using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace XGStudios
{
    public class AISpawner : MonoBehaviour
    {
        public List<GameObject> enemies;
        public PoolManager pool;
        public float yOffset;
        public int enemyCount = 2;
        public GameObject target;
        List<NavmeshAi> _ai;
        int _wave;
        TextMeshProUGUI enemiesLeft;
        List<NavMeshHit> hitList;
        NavMeshHit hit;
        GameObject death;
        void Start()
        {
            enemiesLeft = GameObject.FindGameObjectWithTag("LeftCounter").GetComponent<TextMeshProUGUI>();
            _wave = 1;
            enemies = new List<GameObject>();
            _ai = new List<NavmeshAi>();
            hitList = new List<NavMeshHit>();

            for (int i = 0; i < enemyCount; i++)
                if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                {
                    enemies.Add(pool.RickshawQueue.Dequeue());
                    hitList.Add(hit);

                }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = hitList[i].position;
                enemies[i].SetActive(true);
            }

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
                    StartCoroutine(DeathEffect(_ai[i].transform.position));
                    _ai.RemoveAt(i);
                    hitList.RemoveAt(i);
                }

            if (enemies.Count != 0) return;


            Debug.Log("Enemies empty");
            Debug.Log(_wave);
            enemies.Clear();
            _ai.Clear();
            hitList.Clear();
                

            switch (_wave)
            {
                case 1:
                    InstantiateEnemies(pool.RickshawQueue, pool.mehranQueue, 2, 0);
                    _wave++;
                    break;
                case 2:
                    InstantiateEnemies(pool.RickshawQueue, 2, 5);
                    _wave++;
                    break;
                case 3:
                    InstantiateEnemies(pool.mehranQueue, 3, 2);
                    _wave++;
                    break;
                case 4:
                    InstantiateEnemies(pool.mehranQueue, pool.RickshawQueue, 5, 2);
                    _wave++;
                    break;
                case 5:
                    if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                    {
                        enemies.Add(pool.truck);
                        enemies[0].transform.position = hit.position;
                        enemies[0].SetActive(true);

                    }

                    for (int i = 0; i < enemies.Count; i++)
                        _ai.Add(enemies[i].GetComponent<NavmeshAi>());

                    _wave = 1;
                    break;
            }

        }
        IEnumerator DeathEffect(Vector3 pos) {
            death = pool.deathQueue.Dequeue();
            death.transform.position = pos;
            death.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            death.SetActive(false);
            pool.deathQueue.Enqueue(death);
        }
        void InstantiateEnemies(Queue<GameObject> enemy, int moreEnemy, int enemyPlus)
        {
            for (int i = 0; i < moreEnemy + enemyPlus; i++)
                if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                {
                    enemies.Add(enemy.Dequeue());
                    hitList.Add(hit);

                }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = hitList[i].position;
                enemies[i].SetActive(true);
            }

            for (int i = 0; i < enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        void InstantiateEnemies(Queue<GameObject> enemy, Queue<GameObject> enemy2, int moreEnemy, int enemyPlus)
        {

            for (int i = 0; i < moreEnemy + enemyPlus; i++)
            {

                if (RandomBoolean())
                {
                    if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                    {
                        enemies.Add(enemy.Dequeue());
                        hitList.Add(hit);
                    }

                }
                else
                {
                    if (NavMesh.SamplePosition(findPoint(), out hit, 300f, NavMesh.AllAreas))
                    {
                        enemies.Add(enemy2.Dequeue());
                        hitList.Add(hit);
                    }
                }
            }

            for (int k = 0; k < enemies.Count; k++)
            {
                enemies[k].transform.position = hitList[k].position;
                enemies[k].SetActive(true);
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
            return new Vector3(target.transform.position.x + Random.Range(100, 300), yOffset,
                target.transform.position.z + Random.Range(100, 300));

        }
    }
}
