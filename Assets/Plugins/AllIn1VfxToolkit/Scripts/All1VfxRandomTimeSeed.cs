using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class All1VfxRandomTimeSeed : MonoBehaviour
    {
        [SerializeField]
        float minSeedValue = 0;
        [SerializeField]
        float maxSeedValue = 100f;

        void Start()
        {
            MaterialPropertyBlock properties = new MaterialPropertyBlock();
            properties.SetFloat("_TimingSeed", Random.Range(minSeedValue, maxSeedValue));
            GetComponent<Renderer>().SetPropertyBlock(properties);
        }
    }
}
