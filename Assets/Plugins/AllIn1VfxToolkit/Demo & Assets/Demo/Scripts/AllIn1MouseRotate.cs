using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1MouseRotate : MonoBehaviour
    {
        [SerializeField]
        Transform objectToRotate;
        [SerializeField]
        float rotationSpeedHorizontal = 10f;
        [SerializeField]
        float translateVerticalSpeed = 5f;
        [SerializeField]
        float translateScrollSpeed = 2f;

        [Space]
        [Header("Lock Cursor")]
        [SerializeField]
        bool lockCursor;
        [SerializeField]
        KeyCode lockCursorKeyCode;
        [SerializeField]
        AllIn1DemoScaleTween hideUiButtonTween;
        [SerializeField]
        Image lockedButtonImage;
        [SerializeField]
        Text lockedButtonText;
        [SerializeField]
        Color lockedButtonColor;
        bool cursorIsLocked;

        [Space]
        [Header("Movement Bounds")]
        [SerializeField]
        float maxHeight = 40f;
        [SerializeField]
        float maxZoom = 2f;
        [SerializeField]
        float minZoom = 40f;

        Vector3 currPosition = Vector3.zero;
        float dt;

        void Start()
        {
            if (lockCursor) cursorIsLocked = false;
            else cursorIsLocked = true;

            ToggleCursorLocked();
        }

        void Update()
        {
            if (Time.timeSinceLevelLoad < 0.5f) return; //We wait a few moments to allow scene to fully load up

            dt = Time.unscaledDeltaTime;

            CamRotateAroundYAxis();

            currPosition = objectToRotate.position;

            CamHeightTranslate();

            CamZoom();

            if (Input.GetKeyDown(lockCursorKeyCode)) ToggleCursorLocked();
        }

        void CamRotateAroundYAxis()
        {
            float mouseInputX = Input.GetAxis("Mouse X") * dt * 10f * rotationSpeedHorizontal;
            objectToRotate.RotateAround(transform.position, Vector3.up, mouseInputX);
        }

        void CamHeightTranslate()
        {
            float mouseInputY = Input.GetAxis("Mouse Y") * dt * translateVerticalSpeed;
            currPosition.y = Mathf.Clamp(currPosition.y + mouseInputY, 0.25f, maxHeight);
            objectToRotate.position = currPosition;
            objectToRotate.LookAt(transform);
        }

        void CamZoom()
        {
            float mouseInputWheel = Input.GetAxis("Mouse ScrollWheel") * dt * 100f * translateScrollSpeed;
            Vector3 currZoomVector = objectToRotate.forward * mouseInputWheel;

            if (mouseInputWheel > 0 && Vector3.Distance(transform.position, objectToRotate.position) <= maxZoom)
                currZoomVector = Vector3.zero;
            else if (mouseInputWheel < 0 && Vector3.Distance(transform.position, objectToRotate.position) >= minZoom)
                currZoomVector = Vector3.zero;

            currPosition += currZoomVector;
            objectToRotate.position = currPosition;
        }

        public void ToggleCursorLocked()
        {
            cursorIsLocked = !cursorIsLocked;
            hideUiButtonTween.ScaleUpTween();

            if (cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                lockedButtonImage.color = lockedButtonColor;
                lockedButtonText.text = "Unlock Cursor";
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                lockedButtonImage.color = Color.white;
                lockedButtonText.text = "Lock Cursor";
            }
        }
    }
}
