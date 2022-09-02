using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class PrefabApply : IconBase
    {
        public override string Name => "Apply Prefab";
        public override IconPosition Side => IconPosition.All;

        public override void DoGUI(Rect rect)
        {

#if UNITY_2018_3_OR_NEWER
            bool isPrefab = PrefabUtility.IsPartOfAnyPrefab(EnhancedHierarchy.CurrentGameObject);
#else
            var isPrefab =
 PrefabUtility.GetPrefabType(EnhancedHierarchy.CurrentGameObject) == PrefabType.PrefabInstance;
#endif

            using (new GUIContentColor(isPrefab ? Styles.backgroundColorEnabled : Styles.backgroundColorDisabled))
            {
                if (GUI.Button(rect, Styles.prefabApplyContent, Styles.applyPrefabStyle))
                {
                    var objs = GetSelectedObjectsAndCurrent();

                    foreach (GameObject obj in objs)
                        Utility.ApplyPrefabModifications(obj, objs.Count <= 1);
                }
            }
        }
    }
}
