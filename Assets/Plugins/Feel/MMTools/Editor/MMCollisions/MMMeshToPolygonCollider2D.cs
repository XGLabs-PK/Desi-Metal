using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A class that lets you create polygon collider 2D out of mesh filters
    /// </summary>
    public class MMMeshToPolygonCollider2D : MonoBehaviour
    {
        /// <summary>
        /// Generates a PolygonCollider2D out of a meshfilter
        /// </summary>
        /// <param name="meshFilter"></param>
        static void GeneratePolygonCollider2D(MeshFilter meshFilter)
        {
            // we validate our mesh
            if (!ValidateMesh(meshFilter))
                return;

            // we grab or create our PolygonCollider2D
            PolygonCollider2D polygonCollider2D = InitializePolygonCollider2D(meshFilter);

            if (polygonCollider2D == null)
                return;

            var vectors = MeshFilterToVectors(meshFilter);
            var newPoints = VectorsToPoints(vectors);
            EditorUtility.SetDirty(polygonCollider2D);
            polygonCollider2D.SetPath(0, newPoints);
        }

        /// <summary>
        /// Takes an array of vectors and outputs points
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        static Vector2[] VectorsToPoints(Vector3[] vectors)
        {
            var newColliderVertices = new List<Vector2>();

            for (int i = 0; i < vectors.Length; i++)
                newColliderVertices.Add(new Vector2(vectors[i].x, vectors[i].y));

            var newPoints = newColliderVertices.Distinct().ToArray();
            return newPoints;
        }

        /// <summary>
        /// Turns a meshfilter into an array of vectors
        /// </summary>
        /// <param name="meshFilter"></param>
        /// <returns></returns>
        static Vector3[] MeshFilterToVectors(MeshFilter meshFilter)
        {
            var vertices = new List<Vector3>();
            meshFilter.sharedMesh.GetVertices(vertices);

            var boundaryPath = MMGeometry.GetEdges(meshFilter.sharedMesh.triangles).FindBoundary().SortEdges();

            var vectors = new Vector3[boundaryPath.Count];

            for (int i = 0; i < boundaryPath.Count; i++)
                vectors[i] = vertices[boundaryPath[i].Vertice1];

            return vectors;
        }

        /// <summary>
        /// Grabs or creates a polygon collider 2D
        /// </summary>
        /// <param name="meshFilter"></param>
        /// <returns></returns>
        static PolygonCollider2D InitializePolygonCollider2D(MeshFilter meshFilter)
        {
            PolygonCollider2D polygonCollider2D = meshFilter.GetComponent<PolygonCollider2D>();

            if (polygonCollider2D == null)
                polygonCollider2D = meshFilter.gameObject.AddComponent<PolygonCollider2D>();

            polygonCollider2D.pathCount = 1;
            return polygonCollider2D;
        }

        /// <summary>
        /// Makes sure that 
        /// </summary>
        /// <param name="meshFilter"></param>
        /// <returns></returns>
        static bool ValidateMesh(MeshFilter meshFilter)
        {
            if (meshFilter.sharedMesh == null)
            {
                Debug.LogWarning("[MMMeshToPolygonCollider2D] "
                                 + meshFilter.gameObject.name
                                 + " needs to have at least a mesh set on its mesh filter component.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// A method meant to be called via the Tools menu, that will go through all mesh colliders on an object and generate a polygon collider2D out of it
        /// </summary>
        [MenuItem("Tools/More Mountains/Collisions/Generate PolygonCollider2D", false, 601)]
        public static void GeneratePolygonCollider2DMenu()
        {
            Transform activeTransform = Selection.activeTransform;

            if (activeTransform == null)
            {
                Debug.LogWarning("[MMMeshToPolygonCollider2D] You need to select a gameobject first.");
                return;
            }

            EditorSceneManager.MarkSceneDirty(activeTransform.gameObject.scene);
            var meshFilters = activeTransform.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters)
                GeneratePolygonCollider2D(meshFilter);
        }
    }
}
