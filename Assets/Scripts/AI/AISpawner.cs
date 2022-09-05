using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class AISpawner : MonoBehaviour
    {
        public GameObject enemy;
        public List<GameObject> enemies;
        public int enemyCount = 2;
        List<EnemyAI> AI;
        [SerializeField] Terrain terrain;
        [SerializeField] float yOffset;
        float terrainWidth;
        float terrainlength;
        float xTerrainPos;
        float zTerrainPos;
        float randX, randZ, yVal;
        private void Awake()
        {
            terrainWidth = terrain.terrainData.size.x;
            terrainlength = terrain.terrainData.size.z;
            xTerrainPos = terrain.transform.position.x;
            zTerrainPos = terrain.transform.position.z;
        }
        // Start is called before the first frame update

        void Start()
        {

            enemies = new List<GameObject>();
            AI = new List<EnemyAI>();
            for (int i = 0; i < enemyCount; ++i)
            {
                randX = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                randZ = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                yVal = terrain.SampleHeight(new Vector3(randX, 0, randZ));
                yVal = yVal + yOffset;
                enemies.Add(Instantiate(enemy, new Vector3(randX,yVal,randZ), Quaternion.identity));
            }

            for (int i = 0; i < enemies.Count; ++i)
                AI.Add(enemies[i].GetComponent<EnemyAI>());

        }

        // Update is called once per frame
        void Update()
        {
            if (enemies.Count != 0)
                for (int i = 0; i < enemies.Count; ++i)
                    if (AI[i].isFlipped())
                    {
                        GameObject obj = enemies[i];
                        Destroy(obj);
                        enemies.RemoveAt(i);
                        AI.RemoveAt(i);
                        Debug.Log(enemies.Count);
                    }

            if (enemies.Count == 0)
            {
                Debug.Log("Enemies empty");
                enemies.Clear();
                AI.Clear();

                for (int i = 0; i < enemyCount; ++i)
                {
                    randX = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                    randZ = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                    yVal = terrain.SampleHeight(new Vector3(randX, 0, randZ));
                    yVal = yVal + yOffset;
                    enemies.Add(Instantiate(enemy, new Vector3(randX,yVal,randZ), Quaternion.identity));

                }

                for (int i = 0; i < enemyCount; ++i)
                    AI.Add(enemies[i].GetComponent<EnemyAI>());

            }

        }
    }
}
