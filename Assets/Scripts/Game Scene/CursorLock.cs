using UnityEngine;

namespace XGStudios
{
    public class CursorLock : MonoBehaviour
    {
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
