using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class Active : IconBase
    {
        public override IconPosition Side => IconPosition.All;

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.activeToggleStyle, true);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void DoGUI(Rect rect)
        {
            using (new GUIBackgroundColor(EnhancedHierarchy.CurrentGameObject.activeSelf
                       ? Styles.backgroundColorEnabled
                       : Styles.backgroundColorDisabled))
            {
                GUI.changed = false;

                GUI.Toggle(rect, EnhancedHierarchy.CurrentGameObject.activeSelf, Styles.activeContent,
                    Styles.activeToggleStyle);

                if (!GUI.changed)
                    return;

                var objs = GetSelectedObjectsAndCurrent();
                bool active = !EnhancedHierarchy.CurrentGameObject.activeSelf;

                Undo.RecordObjects(objs.ToArray(),
                    EnhancedHierarchy.CurrentGameObject.activeSelf ? "Disabled GameObject" : "Enabled GameObject");

                foreach (GameObject obj in objs)
                    obj.SetActive(active);
            }
        }
    }
}
