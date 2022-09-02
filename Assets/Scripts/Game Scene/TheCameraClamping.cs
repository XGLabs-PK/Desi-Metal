using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class TheCameraClamping : MonoBehaviour
    {
        public float clampingAngleX = -20f;

        void Update()
        {
            /*Vector3 rot = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform);

            if (rot.x < clampingAngleX)
                UnityEditor.TransformUtils.SetInspectorRotation(gameObject.transform, new Vector3(clampingAngleX, rot.y, rot.z));*/
        }
    }
}
