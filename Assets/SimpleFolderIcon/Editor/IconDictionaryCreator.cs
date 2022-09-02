using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SimpleFolderIcon.Editor
{
    public class IconDictionaryCreator : AssetPostprocessor
    {
        const string AssetsPath = "SimpleFolderIcon/Icons";
        internal static Dictionary<string, Texture> IconDictionary;

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (!ContainsIconAsset(importedAssets) &&
                !ContainsIconAsset(deletedAssets) &&
                !ContainsIconAsset(movedAssets) &&
                !ContainsIconAsset(movedFromAssetPaths))
                return;

            BuildDictionary();
        }

        static bool ContainsIconAsset(string[] assets)
        {
            foreach (string str in assets)
                if (ReplaceSeparatorChar(Path.GetDirectoryName(str)) == "Assets/" + AssetsPath)
                    return true;

            return false;
        }

        static string ReplaceSeparatorChar(string path)
        {
            return path.Replace("\\", "/");
        }

        internal static void BuildDictionary()
        {
            var dictionary = new Dictionary<string, Texture>();

            DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + AssetsPath);
            var info = dir.GetFiles("*.png");

            foreach (FileInfo f in info)
            {
                Texture texture =
                    (Texture)AssetDatabase.LoadAssetAtPath($"Assets/SimpleFolderIcon/Icons/{f.Name}",
                        typeof(Texture2D));

                dictionary.Add(Path.GetFileNameWithoutExtension(f.Name), texture);
            }

            IconDictionary = dictionary;
        }
    }
}
