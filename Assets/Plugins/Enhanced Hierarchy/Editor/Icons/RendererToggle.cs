using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class RendererToggle : IconBase
    {
        Renderer renderer;

        public override IconPosition Side => IconPosition.All;
        public override string Name => "Renderer";

        public override Texture2D PreferencesPreview => Utility.GetBackground(Styles.rendererToggleStyle, true);

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        public override void Init()
        {
            renderer = EnhancedHierarchy.Components.FirstOrDefault(c => c is Renderer) as Renderer;
        }

        public override float Width => renderer ? base.Width : 0;

        public override void DoGUI(Rect rect)
        {
            if (!renderer)
                return;

            using (new GUIBackgroundColor(renderer.enabled
                       ? Styles.backgroundColorEnabled
                       : Styles.backgroundColorDisabled))
            {
                GUI.changed = false;
                GUI.Toggle(rect, renderer, Styles.rendererContent, Styles.rendererToggleStyle);

                if (!GUI.changed)
                    return;

                var objs = GetSelectedObjectsAndCurrent().SelectMany(go => go.GetComponents<Renderer>());
                bool active = !renderer.enabled;

                Undo.RecordObjects(objs.ToArray(), renderer.enabled ? "Disabled renderer" : "Enabled renderer");

                foreach (Renderer obj in objs)
                    obj.enabled = active;
            }
        }
    }
}
