using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        List<GameObject> obj;
          
        
        
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
            AI = new List<NavmeshAi>();

            for (int i = 0; i < enemyCount; ++i)
            {
                xRand = Random.Range(xTerrainPos,xTerrainPos+terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(new Vector3(xRand,0,zRand),out hit, 100f, NavMesh.AllAreas)){
                    enemies.Add(Instantiate(Renemy, hit.position, Quaternion.identity));
                }
                
            }

            for (int i = 0; i < enemies.Count; ++i)
                AI.Add(enemies[i].GetComponent<NavmeshAi>());

        }

        // Update is called once per frame
        void Update()
        {
            if (wave > 5) {
                wave = 0;
            }
            if (enemies.Count != 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (AI[i] != null)
                    {
                        if ((AI[i].health <= 0))
                        {
                            
                            enemies.RemoveAt(i);
                            AI.RemoveAt(i);
                            
                        }
                    }
                }
            }
            if (enemies.Count == 0)
            {
                wave++;

                Debug.Log("Enemies empty");
                Debug.Log(wave);
                enemies.Clear();
                AI.Clear();
                switch (wave) {
                    case 1:
                        instantiateEnemies(Menemy, Renemy,2,0);
                        break;
                    case 2:
                        instantiateEnemies(Renemy,2, 5);
                        break;
                    case 3:
                        instantiateEnemies(Menemy, 3,2);
                        break;
                    case 4:
                        instantiateEnemies(Menemy, Renemy, 5,2);
                        break;
                    case 5:
                        instantiateEnemies(TEnemy,0,1);
                        
                        break;
                }
          

            }

        }
        
        void instantiateEnemies(GameObject enemy,int moreEnemy, int enemyPlus) {
            for (int i = 0; i < moreEnemy+ enemyPlus; ++i)
            {
                xRand = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(new Vector3(xRand, 0, zRand), out hit, 100f, NavMesh.AllAreas))
                {
                    enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
                }
            }
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
                AI.Add(enemies[i].GetComponent<NavmeshAi>());
        }
        void instantiateEnemies(GameObject enemy, GameObject enemy2,int moreEnemy, int enemyPlus) {
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
            {
                xRand = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
                zRand = Random.Range(zTerrainPos, zTerrainPos + terrainlength);
                new Vector3(xRand, yOffset, zRand);
                NavMeshHit hit;
                if (randomBoolean())
                {
                    
                    if (NavMesh.SamplePosition(new Vector3(xRand, 0, zRand), out hit, 100f, NavMesh.AllAreas))
                    {
                        enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
                    }
                }
                else
                {
                    if (NavMesh.SamplePosition(new Vector3(xRand, 0, zRand), out hit, 100f, NavMesh.AllAreas))
                    {
                        enemies.Add(Instantiate(enemy2, hit.position, Quaternion.identity));
                    }
                }
            }
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
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
