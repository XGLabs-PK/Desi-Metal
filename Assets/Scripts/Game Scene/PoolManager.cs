using System;
using System.Collections.Generic;
using UnityEngine;

namespace XG.Studios
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public int spawnCount;
        public List<GameObject> bulletsList;


        void Start()
        {
            //Spawn "100" bullets, add them to the bulletsList
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform, true);
                bulletsList.Add(bullet);
                bullet.SetActive(false);
            }
        }
    }
}
