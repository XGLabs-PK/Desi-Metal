using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class Tag : IconBase
    {
        public override IconPosition Side => IconPosition.All;

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.tagStyle, true);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void DoGUI(Rect rect)
        {
            GUI.changed = false;

            EditorGUI.LabelField(rect, Styles.tagContent);
            string tag = EditorGUI.TagField(rect, Styles.tagContent, EnhancedHierarchy.GameObjectTag, Styles.tagStyle);

            if (GUI.changed && tag != EnhancedHierarchy.GameObjectTag)
                ChangeTagAndAskForChildren(GetSelectedObjectsAndCurrent(), tag);
        }

        public static void ChangeTagAndAskForChildren(List<GameObject> objs, string newTag)
        {
            ChildrenChangeMode changeMode = AskChangeModeIfNecessary(objs, Preferences.TagAskMode, "Change Layer",
                "Do you want to change the tags of the children objects as well?");

            switch (changeMode)
            {
                case ChildrenChangeMode.ObjectOnly:
                    foreach (GameObject obj in objs)
                    {
                        Undo.RegisterCompleteObjectUndo(obj, "Tag changed");
                        obj.tag = newTag;
                    }

                    break;

                case ChildrenChangeMode.ObjectAndChildren:
                    foreach (GameObject obj in objs)
                    {
                        Undo.RegisterFullObjectHierarchyUndo(obj, "Tag changed");

                        obj.tag = newTag;

                        foreach (Transform transform in obj.GetComponentsInChildren<Transform>(true))
                            transform.tag = newTag;
                    }

                    break;
            }
        }
    }
}
