using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1VfxAutoDestroy : MonoBehaviour
    {
        [SerializeField]
        float destroyTime = 1f;

        void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}
