using System;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class PoolManager : MonoBehaviour
    {
        [Header("For Bullets")]
        public GameObject bulletPrefab;
        public int bulletSpawnCount;
        public List<GameObject> bulletsList;

        [Header("For Bullet Impact")]
        public GameObject bulletImpactPrefab;
        public int impactSpawnCount;
        public List<GameObject> bulletImpactList;

        [Header("For Mehran")]
        public GameObject mehranEnemyPrefab;
        public int mehranCount;
        public Queue<GameObject> mehranQueue;
        [Header("For Rickshaw")]
        public GameObject RickshawPrefab;
        public int RickShawCount;
        public Queue<GameObject> RickshawQueue;
        public GameObject truckPrefab;
        public GameObject truck;

        void Awake()
        {
            mehranQueue = new Queue<GameObject>();
            RickshawQueue = new Queue<GameObject>();

        }

        void Start()
        {
            //Spawn "25" bullets, add them to the bulletsList
            for (int i = 0; i < bulletSpawnCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform, true);
                bulletsList.Add(bullet);
                bullet.SetActive(false);
            }

            //Spawn "25" bullet Impacts, add them to the bulletsList
            for (int i = 0; i < impactSpawnCount; i++)
            {
                GameObject bulletImpact = Instantiate(bulletImpactPrefab, transform, true);
                bulletImpactList.Add(bulletImpact);
                bulletImpact.SetActive(false);
            }

            for (int i = 0; i < mehranCount; i++)
            {
                GameObject mehran = Instantiate(mehranEnemyPrefab, transform, true);
                mehran.SetActive(false);
                mehranQueue.Enqueue(mehran);
            }

            for (int i = 0; i < RickShawCount; i++)
            {
                GameObject rickshaw = Instantiate(RickshawPrefab, transform, true);
                rickshaw.SetActive(false);
                RickshawQueue.Enqueue(rickshaw);

            }

            truck = Instantiate(truckPrefab, transform, true);
            truck.SetActive(false);

        }
    }
}
