using System.Collections.Generic;
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
        GameObject target;
        List<NavmeshAi> _ai;
        [SerializeField]Terrain terrain;
        float _terrainLength;
        float _terrainWidth;
        float _xTerrainPos;
        float _zTerrainPos;
        float _xRand;
        float _zRand;
        int _wave;
          
        void Awake()
        {
            _terrainWidth = terrain.terrainData.size.x;
            _terrainLength = terrain.terrainData.size.z;
            _xTerrainPos = terrain.transform.position.x;
            _zTerrainPos = terrain.transform.position.z;
        }
        void Start()
        {
            _wave = 1;
            enemies = new List<GameObject>();
            _ai = new List<NavmeshAi>();
            target = GameObject.FindGameObjectWithTag("Car");

            for (int i = 0; i <enemyCount; i++)
            {
                if (NavMesh.SamplePosition(findPoint(),out NavMeshHit hit, 300f, NavMesh.AllAreas)){
                    enemies.Add(Instantiate(rickshawEnemy, hit.position, Quaternion.identity));
                }
            }
            for (int i = 0; i <enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        void Update()
        {
            
            if (enemies.Count != 0)
            {
                for (int i = 0; i <enemies.Count; i++)
                {
                    if (_ai[i] == null) continue;
                    if (_ai[i].health > 0) continue;
                    enemies.RemoveAt(i);
                    _ai.RemoveAt(i);
                }
            }

            if (enemies.Count != 0) return;
            
            Debug.Log("Enemies empty");
            Debug.Log(_wave);
            enemies.Clear();
            _ai.Clear();
            switch (_wave) {
                case 1:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy,2,0);
                    _wave++;
                    break;
                case 2:
                    InstantiateEnemies(rickshawEnemy,2, 5);
                    _wave++;
                    break;
                case 3:
                    InstantiateEnemies(mehranEnemy, 3,2);
                    _wave++;
                    break;
                case 4:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy, 5,2);
                    _wave++;
                    break;
                case 5:
                    InstantiateEnemies(truckEnemy,0,1);
                    _wave = 1;
                    break;
            }

        }
        
        void InstantiateEnemies(GameObject enemy,int moreEnemy, int enemyPlus) {
            for (int i = 0; i<moreEnemy+ enemyPlus; i++)
            {
                if (NavMesh.SamplePosition(findPoint(), out NavMeshHit hit, 300f, NavMesh.AllAreas))
                    enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
            }
            for (int i = 0; i <enemies.Count; i++)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }
        
        void InstantiateEnemies(GameObject enemy, GameObject enemy2,int moreEnemy, int enemyPlus) {
            for (int i = 0; i <moreEnemy +enemyPlus; i++)
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
        Vector3 findPoint() {
            _xRand = Random.Range(_xTerrainPos, _xTerrainPos + _terrainWidth);
            _zRand = Random.Range(_zTerrainPos, _zTerrainPos + _terrainLength);
            Vector3 point = new Vector3(_xRand, yOffset, _zRand);
            if (Vector3.Distance(target.transform.position, point) > 200)
            {
                findPoint();
            }
            else
                return point;

        }
    }
   
}
