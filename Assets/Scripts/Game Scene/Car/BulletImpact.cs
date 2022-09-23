using UnityEngine;

namespace XGStudios
{
    public class BulletImpact : MonoBehaviour
    {
        void OnEnable()
        {
            Invoke(nameof(OnDisable), 2f);
        }

        void OnDisable()
        {
            transform.gameObject.SetActive(false);
        }
    }
}
