using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class AISpawner : MonoBehaviour
    {
        public GameObject Menemy;
        public GameObject Renemy;
        public GameObject TEnemy;
        public List<GameObject> enemies;
        public int enemyCount = 2;
        List<NavmeshAi> AI;
        private float terrainWidth;
        [SerializeField]Terrain terrain;
        [SerializeField] float yOffset;
        [SerializeField] GameObject deathParticle;
        float terrainlength;
        float xTerrainPos;
        float zTerrainPos;
        float xRand;
        float zRand;
        int wave = 0;
        
        
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
            wave++;
            enemies = new List<GameObject>();
            AI = new List<NavmeshAi>();

            for (int i = 0; i < enemyCount; ++i)
            {
                xRand = Random.Range(xTerrainPos,xTerrainPos+terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);   
                enemies.Add(Instantiate(Renemy, new Vector3(xRand,yOffset,zRand), Quaternion.identity));
                
            }

            for (int i = 0; i < enemies.Count; ++i)
                AI.Add(enemies[i].GetComponent<NavmeshAi>());

        }

        // Update is called once per frame
        void Update()
        {
            if (enemies.Count != 0)
            {
                for (int i = 0; i < enemies.Count; ++i)
                {
                    if (AI[i].isFlipped())
                    {
                        GameObject obj = enemies[i];
                        Destroy(Instantiate(deathParticle, transform.position, Quaternion.identity), 1.5f);
                        Destroy(obj);
                        enemies.RemoveAt(i);
                        AI.RemoveAt(i);
                        Debug.Log(enemies.Count);
                    }
                }
            }
            if (enemies.Count == 0)
            {
                wave++;

                Debug.Log("Enemies empty");
                enemies.Clear();
                AI.Clear();
                switch (wave) {
                    case 1:
                        instantiateEnemies(Menemy, Renemy,0);
                        break;
                    case 2:
                        instantiateEnemies(Renemy, 5);
                        break;
                    case 3:
                        instantiateEnemies(Menemy, 3);
                        break;
                    case 4:
                        instantiateEnemies(Menemy, Renemy, 5);
                        break;
                    case 5:
                        wave = 1;
                        instantiateEnemies(TEnemy, -(enemyCount + 1));
                        break;
                }
          

            }

        }
        void instantiateEnemies(GameObject enemy,int moreEnemy) {
            for (int i = 0; i < enemyCount + moreEnemy; ++i)
            {
                xRand = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                enemies.Add(Instantiate(enemy, new Vector3(xRand, yOffset, zRand), Quaternion.identity));
            }
            for (int i = 0; i < enemyCount +moreEnemy; ++i)
                AI.Add(enemies[i].GetComponent<NavmeshAi>());
        }
        void instantiateEnemies(GameObject enemy, GameObject enemy2,int moreEnemy) {
            for (int i = 0; i < enemyCount +moreEnemy; ++i)
            {
                xRand = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                new Vector3(xRand, yOffset, zRand);
                if (randomBoolean())
                {
                    enemies.Add(Instantiate(enemy, new Vector3(xRand, yOffset, zRand), Quaternion.identity));
                }
                else
                {
                    enemies.Add(Instantiate(enemy2, new Vector3(xRand, yOffset, zRand), Quaternion.identity));
                }
            }
            for (int i = 0; i < enemyCount +moreEnemy; ++i)
                AI.Add(enemies[i].GetComponent<NavmeshAi>());
        }
        bool randomBoolean()
        {
            if (Random.value > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
   
}
