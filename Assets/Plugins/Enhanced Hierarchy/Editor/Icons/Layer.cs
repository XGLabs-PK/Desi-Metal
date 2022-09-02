using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class Layer : IconBase
    {
        public override IconPosition Side => IconPosition.All;

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.layerStyle, true);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void DoGUI(Rect rect)
        {
            GUI.changed = false;

            EditorGUI.LabelField(rect, Styles.layerContent);
            int layer = EditorGUI.LayerField(rect, EnhancedHierarchy.CurrentGameObject.layer, Styles.layerStyle);

            if (GUI.changed)
                ChangeLayerAndAskForChildren(GetSelectedObjectsAndCurrent(), layer);
        }

        public static void ChangeLayerAndAskForChildren(List<GameObject> objs, int newLayer)
        {
            ChildrenChangeMode changeMode = AskChangeModeIfNecessary(objs, Preferences.LayerAskMode, "Change Layer",
                "Do you want to change the layers of the children objects as well?");

            switch (changeMode)
            {
                case ChildrenChangeMode.ObjectOnly:
                    foreach (GameObject obj in objs)
                    {
                        Undo.RegisterCompleteObjectUndo(obj, "Layer changed");
                        obj.layer = newLayer;
                    }

                    break;

                case ChildrenChangeMode.ObjectAndChildren:
                    foreach (GameObject obj in objs)
                    {
                        Undo.RegisterFullObjectHierarchyUndo(obj, "Layer changed");

                        obj.layer = newLayer;

                        foreach (Transform transform in obj.GetComponentsInChildren<Transform>(true))
                            transform.gameObject.layer = newLayer;
                    }

                    break;
            }
        }
    }
}
