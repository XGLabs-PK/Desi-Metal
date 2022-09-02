using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PG_Physics.Wheel
{
    [CustomEditor(typeof(PG_WheelCollider))]
    [CanEditMultipleObjects]
    public class PG_WheelColliderEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
                for (int i = 0; i < targets.Length; i++)
                    (targets[i] as PG_WheelCollider).UpdateConfig();
        }

        void OnEnable()
        {
            for (int i = 0; i < targets.Length; i++)
                if ((targets[i] as PG_WheelCollider).CheckFirstEnable())
                {
                    serializedObject.SetIsDifferentCacheDirty();
                    serializedObject.Update();
                }
        }
    }

    [CustomPropertyDrawer(typeof(FullField))]
    public class FullPropertyPG_WheelColliderConfigDrawer : PG_WheelColliderConfigDrawer
    {
        protected override bool IsFullProperty => true;
    }

    [CustomPropertyDrawer(typeof(PG_WheelColliderConfig))]
    public class PG_WheelColliderConfigDrawer : PropertyDrawer
    {
        protected virtual bool IsFullProperty => false;

        float LineHeight = 18;
        float Space = 4;
        float LineIndent = 8;

        Rect Rect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, GUIContent.none, EditorStyles.helpBox);

            float x = position.x + Space + LineIndent;
            float y = position.y + Space;
            float inspectorWidth = position.width - Space * 2 - LineIndent;

            Rect = new Rect(x, y, inspectorWidth, LineHeight);

            EditorGUI.BeginProperty(position, label, property);

            bool isFoldout = false;

            if (!IsFullProperty)
            {
                Rect.x += LineIndent;
                SerializedProperty isFoldoutField = property.FindPropertyRelative("IsFoldout");
                isFoldoutField.boolValue = !EditorGUI.Foldout(Rect, !isFoldoutField.boolValue, label);
                isFoldout = isFoldoutField.boolValue;
                Rect.y += LineHeight;
                Rect.x -= LineIndent;
            }
            else
                DrawLabel(label.text);

            if (isFoldout)
            {
                EditorGUI.EndProperty();
                return;
            }

            DrawSpace();

            SerializedProperty isFullConfig = property.FindPropertyRelative("IsFullConfig");

            SerializedProperty mass = property.FindPropertyRelative("Mass");
            SerializedProperty radius = property.FindPropertyRelative("Radius");
            SerializedProperty wheelDampingRate = property.FindPropertyRelative("WheelDampingRate");
            SerializedProperty suspensionDistance = property.FindPropertyRelative("SuspensionDistance");
            SerializedProperty forceAppPointDistance = property.FindPropertyRelative("ForceAppPointDistance");
            SerializedProperty center = property.FindPropertyRelative("Center");

            SerializedProperty spring = property.FindPropertyRelative("Spring");
            SerializedProperty damper = property.FindPropertyRelative("Damper");
            SerializedProperty targetPoint = property.FindPropertyRelative("TargetPoint");

            SerializedProperty sidewaysFriction = property.FindPropertyRelative("SidewaysFriction");
            SerializedProperty forwardFriction = property.FindPropertyRelative("ForwardFriction");

            bool needShowFullProperty;

            if (IsFullProperty)
            {
                needShowFullProperty = true;
                isFullConfig.boolValue = true;
            }
            else
            {
                DrawBoolField("Is Full Config", isFullConfig, true);
                needShowFullProperty = isFullConfig.boolValue;

                if (needShowFullProperty)
                    DrawSpace();
            }

            if (needShowFullProperty)
            {
                DrawFloatField("Mass", mass);
                DrawFloatField("Radius", radius);
                DrawFloatField("Wheel Damping Rate", wheelDampingRate);
                DrawFloatField("Suspension Distance", suspensionDistance);
                DrawFloatField("Force App Point Distance", forceAppPointDistance);
                DrawVector3Field("Center", center);

                DrawSpace();
                DrawLabel("Suspension Spring");

                DrawRangeFloatField("Spring", spring);
                DrawRangeFloatField("Damper", damper);
                DrawFloatField("TargetPoint", targetPoint);
            }

            DrawSpace();
            DrawLabel("Frictions");

            DrawRangeFloatField("Forward Friction", forwardFriction,
                toolTip: "0 - Minimum friction, 1 - Maximum friction");

            DrawRangeFloatField("Sideways Friction", sidewaysFriction,
                toolTip: "0 - Minimum friction, 1 - Maximum friction");

            EditorGUI.EndProperty();
        }

        void DrawFloatField(string fieldName, SerializedProperty property, bool withoutLineIndent = false)
        {
            if (withoutLineIndent)
                Rect.x -= LineIndent;

            GUIContent content = new GUIContent(fieldName);
            property.floatValue = EditorGUI.FloatField(Rect, content, property.floatValue);
            Rect.y += LineHeight;

            if (withoutLineIndent)
                Rect.x += LineIndent;
        }

        void DrawRangeFloatField(string fieldName, SerializedProperty property,
            bool withoutLineIndent = false, float minValue = 0, float maxValue = 1,
            string toolTip = "0 - Minimum value, 1 - Maximum value")
        {
            if (withoutLineIndent)
                Rect.x -= LineIndent;

            GUIContent content = new GUIContent(fieldName, toolTip);
            property.floatValue = EditorGUI.Slider(Rect, content, property.floatValue, minValue, maxValue);
            Rect.y += LineHeight;

            if (withoutLineIndent)
                Rect.x += LineIndent;
        }

        void DrawBoolField(string fieldName, SerializedProperty property, bool withoutLineIndent = false)
        {
            if (withoutLineIndent)
                Rect.x -= LineIndent;

            GUIContent content = new GUIContent(fieldName);
            property.boolValue = EditorGUI.Toggle(Rect, content, property.boolValue);
            Rect.y += LineHeight;

            if (withoutLineIndent)
                Rect.x += LineIndent;
        }

        void DrawVector3Field(string fieldName, SerializedProperty property, bool withoutLineIndent = false)
        {
            if (withoutLineIndent)
                Rect.x -= LineIndent;

            GUIContent content = new GUIContent(fieldName);
            property.vector3Value = EditorGUI.Vector3Field(Rect, content, property.vector3Value);
            Rect.y += LineHeight;

            if (withoutLineIndent)
                Rect.x += LineIndent;
        }

        void DrawSpace(int spaceCount = 1)
        {
            Rect.y += Space * spaceCount;
        }

        void DrawLabel(string LabelName, bool withoutLineIndent = true)
        {
            if (withoutLineIndent)
                Rect.x -= LineIndent;

            EditorGUI.LabelField(Rect, LabelName);
            Rect.y += LineHeight;

            if (withoutLineIndent)
                Rect.x += LineIndent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lines = 0;
            float allSpace = 0;

            if (IsFullProperty)
            {
                lines = LineHeight * 14;
                allSpace = Space * 5;
            }
            else
            {
                bool isFoldout = property.FindPropertyRelative("IsFoldout").boolValue;

                if (isFoldout)
                {
                    lines = LineHeight * 1;
                    allSpace = Space * 2;
                }
                else
                {
                    lines = LineHeight * 5;
                    allSpace = Space * 4;

                    bool isFullConfig = property.FindPropertyRelative("IsFullConfig").boolValue;

                    if (isFullConfig)
                    {
                        lines += LineHeight * 10;
                        allSpace += Space * 2;
                    }
                }
            }


            return lines + allSpace;
        }
    }
}
