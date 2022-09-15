using System;
using System.Collections.Generic;
using UnityEngine;

namespace XG.Studios
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
        }
    }
}
