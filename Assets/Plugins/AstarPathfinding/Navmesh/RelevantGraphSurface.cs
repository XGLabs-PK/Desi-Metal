using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Pruning of recast navmesh regions.
    /// A RelevantGraphSurface component placed in the scene specifies that
    /// the navmesh region it is inside should be included in the navmesh.
    ///
    /// See: Pathfinding.RecastGraph.relevantGraphSurfaceMode
    /// </summary>
    [AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_relevant_graph_surface.php")]
    public class RelevantGraphSurface : VersionedMonoBehaviour
    {
        static RelevantGraphSurface root;

        public float maxRange = 1;

        RelevantGraphSurface prev;
        RelevantGraphSurface next;
        Vector3 position;

        public Vector3 Position => position;

        public RelevantGraphSurface Next => next;

        public RelevantGraphSurface Prev => prev;

        public static RelevantGraphSurface Root => root;

        public void UpdatePosition()
        {
            position = transform.position;
        }

        void OnEnable()
        {
            UpdatePosition();

            if (root == null)
                root = this;
            else
            {
                next = root;
                root.prev = this;
                root = this;
            }
        }

        void OnDisable()
        {
            if (root == this)
            {
                root = next;
                if (root != null) root.prev = null;
            }
            else
            {
                if (prev != null) prev.next = next;
                if (next != null) next.prev = prev;
            }

            prev = null;
            next = null;
        }

        /// <summary>
        /// Updates the positions of all relevant graph surface components.
        /// Required to be able to use the position property reliably.
        /// </summary>
        public static void UpdateAllPositions()
        {
            RelevantGraphSurface c = root;

            while (c != null)
            {
                c.UpdatePosition();
                c = c.Next;
            }
        }

        public static void FindAllGraphSurfaces()
        {
            var srf = FindObjectsOfType(typeof(RelevantGraphSurface)) as RelevantGraphSurface[];

            for (int i = 0; i < srf.Length; i++)
            {
                srf[i].OnDisable();
                srf[i].OnEnable();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(57 / 255f, 211 / 255f, 46 / 255f, 0.4f);
            Gizmos.DrawLine(transform.position - Vector3.up * maxRange, transform.position + Vector3.up * maxRange);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(57 / 255f, 211 / 255f, 46 / 255f);
            Gizmos.DrawLine(transform.position - Vector3.up * maxRange, transform.position + Vector3.up * maxRange);
        }
    }
}
