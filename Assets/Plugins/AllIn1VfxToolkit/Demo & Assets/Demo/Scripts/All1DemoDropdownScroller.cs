using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(Dropdown))]
    public class All1DemoDropdownScroller : MonoBehaviour
    {
        [SerializeField]
        int dropdownElementCount;
        [SerializeField]
        KeyCode nextDropdownElementKey = KeyCode.M;

        Dropdown dropdown;

        void Start()
        {
            dropdown = GetComponent<Dropdown>();
        }

        void Update()
        {
            if (Input.GetKeyDown(nextDropdownElementKey)) NextDropdownElements();
        }

        void NextDropdownElements()
        {
            int nextValue = dropdown.value + 1;
            if (nextValue < 0) nextValue = dropdownElementCount - 1;
            if (nextValue >= dropdownElementCount) nextValue = 0;
            dropdown.value = nextValue;
        }
    }
}
