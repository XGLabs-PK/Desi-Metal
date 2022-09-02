using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using Object = UnityEngine.Object;


namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// A helper class to copy and paste feedback properties
    /// </summary>
    internal static class MMF_PlayerCopy
    {
        // Single Copy --------------------------------------------------------------------

        public static Type Type { get; private set; }
        static List<SerializedProperty> Properties = new List<SerializedProperty>();

        public static readonly List<MMF_Feedback> CopiedFeedbacks = new List<MMF_Feedback>();

        public static List<MMF_Player> ShouldKeepChanges = new List<MMF_Player>();

        static string[] IgnoreList = new string[]
        {
            "m_ObjectHideFlags",
            "m_CorrespondingSourceObject",
            "m_PrefabInstance",
            "m_PrefabAsset",
            "m_GameObject",
            "m_Enabled",
            "m_EditorHideFlags",
            "m_Script",
            "m_Name",
            "m_EditorClassIdentifier"
        };

        public static bool HasCopy()
        {
            return CopiedFeedbacks != null && CopiedFeedbacks.Count == 1;
        }

        public static bool HasMultipleCopies()
        {
            return CopiedFeedbacks != null && CopiedFeedbacks.Count > 1;
        }

        public static void Copy(MMF_Feedback feedback)
        {
            Type feedbackType = feedback.GetType();
            MMF_Feedback newFeedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
            EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
            CopiedFeedbacks.Clear();
            CopiedFeedbacks.Add(newFeedback);
        }

        public static void CopyAll(MMF_Player sourceFeedbacks)
        {
            CopiedFeedbacks.Clear();

            foreach (MMF_Feedback feedback in sourceFeedbacks.FeedbacksList)
            {
                Type feedbackType = feedback.GetType();
                MMF_Feedback newFeedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
                EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
                CopiedFeedbacks.Add(newFeedback);
            }
        }

        // Multiple Copy ----------------------------------------------------------


        public static void PasteAll(MMF_PlayerEditor targetEditor)
        {
            foreach (MMF_Feedback feedback in CopiedFeedbacks)
                targetEditor.TargetMmfPlayer.AddFeedback(feedback);

            CopiedFeedbacks.Clear();
        }
    }
}
