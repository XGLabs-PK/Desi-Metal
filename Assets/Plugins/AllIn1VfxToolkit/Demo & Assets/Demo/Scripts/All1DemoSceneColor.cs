using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(Dropdown))]
    public class All1DemoSceneColor : MonoBehaviour
    {
        [SerializeField]
        Color[] sceneColors;
        [SerializeField]
        Camera targetCamera;
        [SerializeField]
        float cameraColorMult = 1f;
        [SerializeField]
        MeshRenderer floorMeshRenderer;
        [SerializeField]
        float floorColorMult = 1f;
        [SerializeField]
        float fogColorMult = 1f;

        Dropdown sceneColorDropdown;
        Material floorMaterial;

        void Start()
        {
            sceneColorDropdown = GetComponent<Dropdown>();
            floorMaterial = floorMeshRenderer.material;
            DropdownValueChanged();
        }

        public void DropdownValueChanged()
        {
            SetSceneColor(sceneColorDropdown.value);
        }

        void SetSceneColor(int nIndex)
        {
            targetCamera.backgroundColor = sceneColors[nIndex] * cameraColorMult;
            floorMaterial.color = sceneColors[nIndex] * floorColorMult;
            RenderSettings.fogColor = sceneColors[nIndex] * fogColorMult;
        }
    }
}
