using UnityEngine;

namespace EnhancedHierarchy.Icons
{
    public sealed class None : IconBase
    {
        public override float Width => 0f;
        public override string Name => "None";
        public override IconPosition Side => IconPosition.All;

        public override void DoGUI(Rect rect)
        {
        }
    }
}
