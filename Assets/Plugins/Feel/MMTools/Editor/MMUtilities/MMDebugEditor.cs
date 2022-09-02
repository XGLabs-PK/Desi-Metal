using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEditor;

namespace MoreMountains.Tools
{
    /// <summary>
    /// An editor class used to display menu items 
    /// </summary>
    public class MMDebugEditor
    {
        /// <summary>
        /// Adds a menu item to enable debug logs
        /// </summary>
        [MenuItem("Tools/More Mountains/Enable Debug Logs", false, 100)]
        static void EnableDebugLogs()
        {
            MMDebug.SetDebugLogsEnabled(true);
        }

        /// <summary>
        /// Conditional method to determine if the "enable debug log" entry should be greyed or not
        /// </summary>
        [MenuItem("Tools/More Mountains/Enable Debug Logs", true)]
        static bool EnableDebugLogsValidation()
        {
            return !MMDebug.DebugLogsEnabled;
        }

        /// <summary>
        /// Adds a menu item to disable debug logs
        /// </summary>
        [MenuItem("Tools/More Mountains/Disable Debug Logs", false, 101)]
        static void DisableDebugLogs()
        {
            MMDebug.SetDebugLogsEnabled(false);
        }

        /// <summary>
        /// Conditional method to determine if the "disable debug log" entry should be greyed or not
        /// </summary>
        [MenuItem("Tools/More Mountains/Disable Debug Logs", true)]
        static bool DisableDebugLogsValidation()
        {
            return MMDebug.DebugLogsEnabled;
        }

        /// <summary>
        /// Adds a menu item to enable debug logs
        /// </summary>
        [MenuItem("Tools/More Mountains/Enable Debug Draws", false, 102)]
        static void EnableDebugDraws()
        {
            MMDebug.SetDebugDrawEnabled(true);
        }

        [MenuItem("Tools/More Mountains/Enable Debug Draws", true)]
        /// <summary>
        /// Conditional method to determine if the "enable debug log" entry should be greyed or not
        /// </summary>
        static bool EnableDebugDrawsValidation()
        {
            return !MMDebug.DebugDrawEnabled;
        }

        [MenuItem("Tools/More Mountains/Disable Debug Draws", false, 103)]
        /// <summary>
        /// Adds a menu item to disable debug logs
        /// </summary>
        static void DisableDebugDraws()
        {
            MMDebug.SetDebugDrawEnabled(false);
        }

        [MenuItem("Tools/More Mountains/Disable Debug Draws", true)]
        /// <summary>
        /// Conditional method to determine if the "disable debug log" entry should be greyed or not
        /// </summary>
        static bool DisableDebugDrawsValidation()
        {
            return MMDebug.DebugDrawEnabled;
        }
    }
}
