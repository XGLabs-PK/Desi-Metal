using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheCameraClamping : MonoBehaviour
    {
        public float clampingAngleX = -20f;

        void Update()
        {
#if UNITY_EDITOR
            Vector3 rot = TransformUtils.GetInspectorRotation(gameObject.transform);

            if (rot.x < clampingAngleX)
            {
                TransformUtils.SetInspectorRotation(gameObject.transform, new Vector3(clampingAngleX, rot.y, rot.z));
            }     
#endif
        }
    }
}
