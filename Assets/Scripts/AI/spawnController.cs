using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGStudios
{
    public class SpawnController : MonoBehaviour
    {
        public GameObject Spawner; 
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(EnableSpawner());
        }

        IEnumerator EnableSpawner() {
            yield return new WaitForSeconds(8f);
            Spawner.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
