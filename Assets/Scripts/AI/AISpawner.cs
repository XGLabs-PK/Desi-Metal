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
        public int enemyCount = 2;
        List<NavmeshAi> _ai;
        [SerializeField]Terrain terrain;
        [SerializeField] float yOffset;
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
            enemies = new List<GameObject>();
            _ai = new List<NavmeshAi>();

            for (int i = 0; i < enemyCount; ++i)
            {
                _xRand = Random.Range(_xTerrainPos,_xTerrainPos+_terrainWidth);
                _zRand = Random.Range(_zTerrainPos, _zTerrainPos + _terrainLength);
                if (NavMesh.SamplePosition(new Vector3(_xRand,0,_zRand),out NavMeshHit hit, 100f, NavMesh.AllAreas)){
                    enemies.Add(Instantiate(rickshawEnemy, hit.position, Quaternion.identity));
                }
            }
            for (int i = 0; i < enemyCount; ++i)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        void Update()
        {
            if (_wave > 5)
                _wave = 0;
            
            if (enemies.Count != 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (_ai[i] == null) continue;
                    if (_ai[i].health > 0) continue;
                    enemies.RemoveAt(i);
                    _ai.RemoveAt(i);
                }
            }

            if (enemies.Count != 0) return;
            _wave++;
            Debug.Log("Enemies empty");
            Debug.Log(_wave);
            enemies.Clear();
            _ai.Clear();
            switch (_wave) {
                case 1:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy,2,0);
                    break;
                case 2:
                    InstantiateEnemies(rickshawEnemy,2, 5);
                    break;
                case 3:
                    InstantiateEnemies(mehranEnemy, 3,2);
                    break;
                case 4:
                    InstantiateEnemies(mehranEnemy, rickshawEnemy, 5,2);
                    break;
                case 5:
                    InstantiateEnemies(truckEnemy,0,1);
                    break;
            }

        }
        
        void InstantiateEnemies(GameObject enemy,int moreEnemy, int enemyPlus) {
            for (int i = 0; i < moreEnemy+ enemyPlus; ++i)
            {
                _xRand = Random.Range(_xTerrainPos, _xTerrainPos + _terrainWidth);
                _zRand = Random.Range(_zTerrainPos, _zTerrainPos + _terrainLength);
                if (NavMesh.SamplePosition(new Vector3(_xRand, 0, _zRand), out NavMeshHit hit, 100f, NavMesh.AllAreas))
                    enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
            }
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }
        
        void InstantiateEnemies(GameObject enemy, GameObject enemy2,int moreEnemy, int enemyPlus) {
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
            {
                _xRand = Random.Range(_xTerrainPos, _xTerrainPos + _terrainWidth);
                _zRand = Random.Range(_zTerrainPos, _zTerrainPos + _terrainLength);
                new Vector3(_xRand, yOffset, _zRand);
                NavMeshHit hit;
                if (RandomBoolean())
                {
                    if (NavMesh.SamplePosition(new Vector3(_xRand, 0, _zRand), out hit, 100f, NavMesh.AllAreas))
                        enemies.Add(Instantiate(enemy, hit.position, Quaternion.identity));
                }
                else
                {
                    if (NavMesh.SamplePosition(new Vector3(_xRand, 0, _zRand), out hit, 100f, NavMesh.AllAreas))
                        enemies.Add(Instantiate(enemy2, hit.position, Quaternion.identity));
                }
            }
            for (int i = 0; i < moreEnemy +enemyPlus; ++i)
                _ai.Add(enemies[i].GetComponent<NavmeshAi>());
        }

        static bool RandomBoolean()
        {
            return Random.value > 0.5;
        }
    }
   
}
