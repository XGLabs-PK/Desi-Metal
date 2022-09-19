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
        Quaternion rickshawRot;
        public int RickShawCount;
        public Queue<GameObject> RickshawQueue;
        public GameObject truckPrefab;
        public GameObject truck;
        public GameObject deathAnimation;
        public int deathCount;
        public Queue<GameObject> deathQueue;
        Transform myTransform;
        Vector3 myTransformPos;
        GameObject placeHolder;
        void Start()
        {
            myTransform = transform;
            myTransformPos = transform.position;
            mehranQueue = new Queue<GameObject>();
            RickshawQueue = new Queue<GameObject>();
            deathQueue = new Queue<GameObject>();

            //Spawn "25" bullets, add them to the bulletsList
            for (int i = 0; i < bulletSpawnCount; i++)
            {
                placeHolder = Instantiate(bulletPrefab, myTransform, true);
                bulletsList.Add(placeHolder);
                placeHolder.SetActive(false);
            }

            //Spawn "25" bullet Impacts, add them to the bulletsList
            for (int i = 0; i < impactSpawnCount; i++)
            {
                placeHolder = Instantiate(bulletImpactPrefab, myTransform, true);
                bulletImpactList.Add(placeHolder);
                placeHolder.SetActive(false);
            }

            for (int i = 0; i < mehranCount; i++)
            {
                placeHolder = Instantiate(mehranEnemyPrefab, myTransformPos, Quaternion.identity);
                placeHolder.SetActive(false);
                mehranQueue.Enqueue(placeHolder);
            }

            for (int i = 0; i < RickShawCount; i++)
            {
                placeHolder = Instantiate(RickshawPrefab, myTransformPos, Quaternion.identity);
                placeHolder.SetActive(false);
                RickshawQueue.Enqueue(placeHolder);

            }
            for (int i = 0; i < deathCount; i++) {
                placeHolder = Instantiate(deathAnimation, myTransform, true);
                placeHolder.SetActive(false);
                deathQueue.Enqueue(placeHolder);
            }

            truck = Instantiate(truckPrefab, myTransform, true);
            truck.SetActive(false);

        }
    }
}
