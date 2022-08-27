using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public GameManager instance;

        public bool gamePaused;
        public bool carDestroyed;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            if (gamePaused || carDestroyed)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
