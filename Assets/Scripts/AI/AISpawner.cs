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
        TextMeshProUGUI _enemiesLeft;
        List<NavMeshHit> _hitList;
        NavMeshHit _hit;
        GameObject _death;
        Vector3 position;
        Vector3 newPoint;
        bool is3;
        void Start()
        {
            is3 = false;
            newPoint = Vector3.zero;
            _enemiesLeft = GameObject.FindGameObjectWithTag("LeftCounter").GetComponent<TextMeshProUGUI>();
            _wave = 1;
            enemies = new List<GameObject>();
            _ai = new List<NavmeshAi>();

            _hitList = new List<NavMeshHit>();

            for (int i = 0; i < enemyCount; i++)
                if (NavMesh.SamplePosition(FindPoint(), out _hit, 300f, NavMesh.AllAreas))
                {
                    enemies.Add(pool.RickshawQueue.Dequeue());
                    _hitList.Add(_hit);
                }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = _hitList[i].position;
                enemies[i].SetActive(true);
            }

            foreach (GameObject t in enemies)
                _ai.Add(t.GetComponent<NavmeshAi>());
        }

        void Update()
        {
            if (_enemiesLeft != null)
                _enemiesLeft.text = enemies.Count.ToString();

            if (enemies.Count != 0)
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (_ai[i] == null) continue;
                    if (_ai[i].health > 0) continue;

                    enemies.RemoveAt(i);
                    StartCoroutine(DeathEffect(_ai[i].transform.position));
                    _ai.RemoveAt(i);
                    _hitList.RemoveAt(i);
                }

            if (enemies.Count != 0) return;
            enemies.Clear();
            _ai.Clear();
            _hitList.Clear();
            
            switch (_wave)
            {
                case 1:
                    is3 = false;
                    InstantiateEnemies(pool.RickshawQueue, pool.mehranQueue, 2, 0);
                    _wave++;
                    break;
                case 2:
                    is3 = false;
                    InstantiateEnemies(pool.RickshawQueue, 2, 5);
                    _wave++;
                    break;
                case 3:
                    is3 = true;
                    InstantiateEnemies(pool.mehranQueue, 3, 2);
                    _wave++;
                    break;
                case 4:
                    is3 = true;
                    InstantiateEnemies(pool.mehranQueue, pool.RickshawQueue, 5, 2);
                    _wave++;
                    break;
                case 5:
                    is3 = false;
                    if (NavMesh.SamplePosition(FindPoint(), out _hit, 200f, NavMesh.AllAreas))
                    {
                        enemies.Add(pool.truck);
                        enemies[0].transform.position = _hit.position;
                        enemies[0].SetActive(true);
                    }
                    
                    foreach (GameObject t in enemies)
                        _ai.Add(t.GetComponent<NavmeshAi>());

                    _wave = 1;
                    break;
            }

        }
        IEnumerator DeathEffect(Vector3 pos) {
            _death = pool.deathQueue.Dequeue();
            _death.transform.position = pos;
            _death.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _death.SetActive(false);
            pool.deathQueue.Enqueue(_death);
        }
        
        void InstantiateEnemies(Queue<GameObject> enemy, int moreEnemy, int enemyPlus)
        {
            for (int i = 0; i < moreEnemy + enemyPlus; i++)
                if (NavMesh.SamplePosition(FindPoint(), out _hit, 300f, NavMesh.AllAreas) && !is3)
                {
                    enemies.Add(enemy.Dequeue());
                    _hitList.Add(_hit);
                }
                else if (NavMesh.SamplePosition(Findpoint2(), out _hit, 300f, NavMesh.AllAreas) && is3) {
                    enemies.Add(enemy.Dequeue());
                    _hitList.Add(_hit);
                }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = _hitList[i].position;
                enemies[i].SetActive(true);
            }

            foreach (GameObject t in enemies)
                _ai.Add(t.GetComponent<NavmeshAi>());
        }

        void InstantiateEnemies(Queue<GameObject> enemy, Queue<GameObject> enemy2, int moreEnemy, int enemyPlus)
        {
            for (int i = 0; i < moreEnemy + enemyPlus; i++)
            {
                if (RandomBoolean())
                {
                    if (NavMesh.SamplePosition(FindPoint(), out _hit, 300f, NavMesh.AllAreas) &&!is3)
                    {
                        enemies.Add(enemy.Dequeue());
                        _hitList.Add(_hit);
                    }
                    else if (NavMesh.SamplePosition(Findpoint2(), out _hit, 300f, NavMesh.AllAreas) && is3)
                    {
                        enemies.Add(enemy.Dequeue());
                        _hitList.Add(_hit);
                    }

                }
                else
                {
                    if (NavMesh.SamplePosition(FindPoint(), out _hit, 300f, NavMesh.AllAreas) && !is3)
                    {
                        enemies.Add(enemy2.Dequeue());
                        _hitList.Add(_hit);
                    }
                    else if (NavMesh.SamplePosition(Findpoint2(), out _hit, 300f, NavMesh.AllAreas) && is3)
                    {
                        enemies.Add(enemy2.Dequeue());
                        _hitList.Add(_hit);
                    }
                }
            }

            for (int k = 0; k < enemies.Count; k++)
            {
                enemies[k].transform.position = _hitList[k].position;
                enemies[k].SetActive(true);
            }

            foreach (GameObject t in enemies)
                _ai.Add(t.GetComponent<NavmeshAi>());
        }

        static bool RandomBoolean()
        {
            return Random.value > 0.5;
        }

        Vector3 FindPoint()
        {
            position = target.transform.position;
            //return new Vector3(position.x + Random.Range(100, 300), yOffset,
            //    position.z + Random.Range(100, 300));
            newPoint.x = position.x + Random.Range(100, 300);
            newPoint.y = yOffset;
            newPoint.z = position.z + Random.Range(100, 300);
            return newPoint;
        }
        Vector3 Findpoint2() {
            position = target.transform.position;
            newPoint.x = position.x + Random.Range(Random.Range(200,300), Random.Range(300,400));
            newPoint.y = yOffset;
            newPoint.z = position.z + Random.Range(Random.Range(200,300), Random.Range(300,400));
            return newPoint;


        }
    }
}
