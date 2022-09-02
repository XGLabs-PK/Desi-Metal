using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class Static : IconBase
    {
        public override IconPosition Side => IconPosition.All;

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.staticToggleStyle, false);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void DoGUI(Rect rect)
        {
            using (new GUIBackgroundColor(EnhancedHierarchy.CurrentGameObject.isStatic
                       ? Styles.backgroundColorEnabled
                       : Styles.backgroundColorDisabled))
            {
                GUI.changed = false;

                GUI.Toggle(rect, EnhancedHierarchy.CurrentGameObject.isStatic, Styles.staticContent,
                    Styles.staticToggleStyle);

                if (!GUI.changed)
                    return;

                bool isStatic = !EnhancedHierarchy.CurrentGameObject.isStatic;
                var selectedObjects = GetSelectedObjectsAndCurrent();

                ChildrenChangeMode changeMode = AskChangeModeIfNecessary(selectedObjects,
                    Preferences.StaticAskMode.Value, "Change Static Flags",
                    "Do you want to " + (!isStatic ? "enable" : "disable") +
                    " the static flags for all child objects as well?");

                switch (changeMode)
                {
                    case ChildrenChangeMode.ObjectOnly:
                        foreach (GameObject obj in selectedObjects)
                        {
                            Undo.RegisterCompleteObjectUndo(obj, "Static Flags Changed");
                            obj.isStatic = isStatic;
                        }

                        break;

                    case ChildrenChangeMode.ObjectAndChildren:
                        foreach (GameObject obj in selectedObjects)
                        {
                            Undo.RegisterFullObjectHierarchyUndo(obj, "Static Flags Changed");

                            var transforms = obj.GetComponentsInChildren<Transform>(true);

                            foreach (Transform transform in transforms)
                                transform.gameObject.isStatic = isStatic;
                        }

                        break;
                }
            }
        }
    }
}
