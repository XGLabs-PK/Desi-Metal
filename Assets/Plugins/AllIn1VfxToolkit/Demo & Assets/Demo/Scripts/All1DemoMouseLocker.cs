using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(AllIn1MouseRotate))]
    public class All1DemoMouseLocker : MonoBehaviour
    {
        [SerializeField]
        KeyCode mouseLockerKey = KeyCode.L;
        [SerializeField]
        Image lockButtonImage;
        [SerializeField]
        Color lockButtonColor;

        AllIn1MouseRotate allIn1MouseRotate;
        AllIn1DemoScaleTween lockedTween;
        Text pausedButtonText;
        bool currentlyLocked = false;

        void Start()
        {
            allIn1MouseRotate = GetComponent<AllIn1MouseRotate>();
            lockedTween = lockButtonImage.GetComponent<AllIn1DemoScaleTween>();
            pausedButtonText = lockButtonImage.GetComponentInChildren<Text>();
        }

        void Update()
        {
            if (Input.GetKeyDown(mouseLockerKey)) DoMouseLockToggle();
        }

        public void DoMouseLockToggle()
        {
            currentlyLocked = !currentlyLocked;
            allIn1MouseRotate.enabled = !currentlyLocked;
            lockedTween.ScaleUpTween();

            if (currentlyLocked)
            {
                pausedButtonText.text = "Unlock Camera";
                lockButtonImage.color = lockButtonColor;
            }
            else
            {
                pausedButtonText.text = "Lock Camera";
                lockButtonImage.color = Color.white;
            }
        }
    }
}
