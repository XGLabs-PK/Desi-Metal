using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1VfxBounceAnimation : MonoBehaviour
    {
        [SerializeField]
        Vector3 targetOffset = Vector3.up;
        [SerializeField]
        float speed = 1f;

        Vector3 startPosition, animationMovementVector;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            animationMovementVector = targetOffset * ((Mathf.Sin(Time.time * speed) + 1f) / 2f);
            transform.position = startPosition + animationMovementVector;
        }
    }
}
