using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class Lock : IconBase
    {
        public override IconPosition Side => IconPosition.All;

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.lockToggleStyle, false);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void DoGUI(Rect rect)
        {
            bool locked = (EnhancedHierarchy.CurrentGameObject.hideFlags & HideFlags.NotEditable) != 0;

            using (new GUIBackgroundColor(locked ? Styles.backgroundColorEnabled : Styles.backgroundColorDisabled))
            {
                GUI.changed = false;
                GUI.Toggle(rect, locked, Styles.lockContent, Styles.lockToggleStyle);

                if (!GUI.changed)
                    return;

                var selectedObjects = GetSelectedObjectsAndCurrent();

                ChildrenChangeMode changeMode = AskChangeModeIfNecessary(selectedObjects, Preferences.LockAskMode.Value,
                    "Lock Object",
                    "Do you want to " + (!locked ? "lock" : "unlock") + " the children objects as well?");

                switch (changeMode)
                {
                    case ChildrenChangeMode.ObjectOnly:
                        foreach (GameObject obj in selectedObjects)
                            Undo.RegisterCompleteObjectUndo(obj, locked ? "Unlock Object" : "Lock Object");

                        foreach (GameObject obj in selectedObjects)
                            if (!locked)
                                Utility.LockObject(obj);
                            else
                                Utility.UnlockObject(obj);

                        break;

                    case ChildrenChangeMode.ObjectAndChildren:
                        foreach (GameObject obj in selectedObjects)
                            Undo.RegisterFullObjectHierarchyUndo(obj, locked ? "Unlock Object" : "Lock Object");

                        foreach (GameObject obj in selectedObjects)
                        foreach (Transform transform in obj.GetComponentsInChildren<Transform>(true))
                            if (!locked)
                                Utility.LockObject(transform.gameObject);
                            else
                                Utility.UnlockObject(transform.gameObject);

                        break;
                }

                InternalEditorUtility.RepaintAllViews();
            }
        }
    }
}
