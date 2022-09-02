using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class AISpawner : MonoBehaviour
    {
        public GameObject enemy;
        //GameObject[] enemies;
        //EnemyAI[] AI;
        public List<GameObject> enemies;
        List<EnemyAI> AI;
        public int enemyCount = 2;
        // Start is called before the first frame update

        private void Start()
        {
            enemies = new List<GameObject>();
            AI = new List<EnemyAI>();
            for (int i = 0; i < enemyCount; ++i) {
                Vector2 startPos = Random.insideUnitCircle * 10 + new Vector2(transform.position.x, transform.position.z);
                enemies.Add(Instantiate(enemy, startPos, Quaternion.identity));
            } 
            for (int i = 0; i < enemies.Count; ++i)
            {
                AI.Add(enemies[i].GetComponent<EnemyAI>());
            }

        }
        // Update is called once per frame
        void Update()
        {
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Count; ++i)
                {
                    if (AI[i].isFlipped())
                    {
                        GameObject obj = enemies[i];
                        Destroy(obj);
                        enemies.RemoveAt(i);
                        AI.RemoveAt(i);
                    }

                }
            }
            if (enemies == null) {
                Debug.Log("Enemies empty");
                for (int i = 0; i < enemyCount; ++i) {
                    Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 10;
                    enemies.Add(Instantiate(enemy, pos, Quaternion.identity));
               
                }
                for (int i = 0; i < enemyCount; ++i) {
                    AI[i] = enemies[i].GetComponent<EnemyAI>();
                }

            }
         
        }
    }
}
