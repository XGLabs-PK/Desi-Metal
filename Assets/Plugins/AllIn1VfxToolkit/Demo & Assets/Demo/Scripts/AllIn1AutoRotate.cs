using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1AutoRotate : MonoBehaviour
    {
        [SerializeField]
        float rotationSpeed = 30f;
        [SerializeField]
        Vector3 rotationAxis = Vector3.up;

        void Update()
        {
            transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime), Space.Self);
        }
    }
}
