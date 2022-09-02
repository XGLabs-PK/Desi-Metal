using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1ChangeAllChildTextFonts : MonoBehaviour
    {
        [SerializeField]
        Font newFont;
        [SerializeField]
        bool changeFontOnStart;

        void Start()
        {
            if (changeFontOnStart) ChangeFonts();
        }

        [ContextMenu("ChangeFonts")]
        void ChangeFonts()
        {
            var canvasTexts = GetComponentsInChildren<Text>();
            for (int i = 0; i < canvasTexts.Length; i++) canvasTexts[i].font = newFont;
        }
    }
}
