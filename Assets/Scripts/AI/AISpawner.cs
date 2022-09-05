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
        private float terrainWidth;
        [SerializeField]Terrain terrain;
        [SerializeField] float yOffset;
        float terrainlength;
        float xTerrainPos;
        float zTerrainPos;
        float xRand;
        float zRand;
        float yVal;
        
        // Start is called before the first frame update
        private void Awake()
        {
            terrainWidth = terrain.terrainData.size.x;
            terrainlength = terrain.terrainData.size.z;
            xTerrainPos = terrain.transform.position.x;
            zTerrainPos = terrain.transform.position.z;
        }
        void Start()
        {
            enemies = new List<GameObject>();
            AI = new List<EnemyAI>();

            for (int i = 0; i < enemyCount; ++i)
            {
                xRand = Random.Range(xTerrainPos,xTerrainPos+terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                yVal = terrain.SampleHeight(new Vector3(xRand, 0, zRand));
                yVal = yVal + yOffset;
                enemies.Add(Instantiate(enemy, new Vector3(xRand,yVal,zRand), Quaternion.identity));
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
                    Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 10;
                    enemies.Add(Instantiate(enemy, pos, Quaternion.identity));

                }

                for (int i = 0; i < enemyCount; ++i)
                    AI.Add(enemies[i].GetComponent<EnemyAI>());

            }

        }
    }
}
