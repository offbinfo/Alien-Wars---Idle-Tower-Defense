using UnityEngine;
using UnityEditor;

namespace ProjectTools
{
    [InitializeOnLoad]
    public class UnityAnimateEditorUtility : AssetPostprocessor
    {
        private static string iconPath;

        private static void OnPostprocessAllAssets (string [] importedAssets, string [] deletedAssets, string [] movedAssets, string [] movedFromAssetPaths, bool didDomainReload)
        {
            string [] assets = AssetDatabase.FindAssets ("t:script UnityAnimateEditorUtility");

            if (assets.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath (assets [0]);
                string path = System.IO.Path.GetDirectoryName (assetPath).Replace ("\\", "/");

                iconPath = path.Replace ("Scripts", "Icons");
                Icons.Init ();
            }
        }

        public static class Icons
        {
            public static Texture2D animation;
            public static Texture2D userEvent;

            static Texture2D LoadIcon (string filename)
            {
                return (Texture2D)AssetDatabase.LoadMainAssetAtPath (iconPath + "/" + filename);
            }

            public static void Init ()
            {
                animation = LoadIcon ("icon-animation.png");
                userEvent = LoadIcon ("icon-event.png");
            }
        }
    }
}