using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Scripts
{
    public class AllIn1GraphicMaterialDuplicate : MonoBehaviour
    {
        void Awake()
        {
            Graphic graphic = GetComponent<Graphic>();
            graphic.material = new Material(graphic.material);
        }
    }
}
