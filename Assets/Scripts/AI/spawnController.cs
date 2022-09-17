using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class spawnController : MonoBehaviour
    {
        public GameObject Spawner; 
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(EnableSpawner());
        }

        IEnumerator EnableSpawner() {
            yield return new WaitForSeconds(5f);
            Spawner.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
