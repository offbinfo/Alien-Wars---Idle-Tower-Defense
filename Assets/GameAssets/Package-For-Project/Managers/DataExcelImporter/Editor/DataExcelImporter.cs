using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectTools
{
    public class DataExcelImporter
    {
        private static string fileLink = "data-excel-editor-link";

        [MenuItem ("Project-Tools/Data Importer/Import From Buffer &V")]
        public static void ImportDatas ()
        {
            var buffer = GUIUtility.systemCopyBuffer;
            var parts = buffer.Split ("\n");

            var folder = "";
            var properties = new List<string> ();

            foreach (var part in parts)
            {
                if (part.Contains ("Folder"))
                {
                    folder = part.Split ("\t") [1];
                }
                else if (part.Contains ("Properties"))
                {
                    var separate = part.Split ("\t");
                    properties.Clear ();
                    for (int i = 1; i < separate.Length; i++)
                        properties.Add (separate [i]);
                }
                else if (!string.IsNullOrEmpty (folder))
                {
                    var file = part.Split ("\t") [0];

                    ImportToAsset (properties, part, folder, file);
                    ImportToPrefab (properties, part, folder, file);
                }
            }

            Debug.Log ("import data complete!");
        }

        private static void ImportToAsset (List<string> properties, string part, string folder, string file)
        {
            var path = folder + "/" + file + ".asset";

            var data = AssetDatabase.LoadAssetAtPath<Object> (path);

            if (data)
            {
                var values = new List<(string, string)> ();

                var separate = part.Split ("\t");
                for (int i = 0; i < properties.Count; i++)
                {
                    var key = properties [i];
                    var s = separate [i + 1];

                    values.Add ((key, s));
                }

                data.ImportData (values);
            }
        }

        private static void ImportToPrefab (List<string> properties, string part, string folder, string file)
        {
            var parts = file.Split ("/");
            var path = folder + "/" + parts [0] + ".prefab";

            var g = AssetDatabase.LoadAssetAtPath<GameObject> (path);

            if (g)
            {
                var values = new List<(string, string)> ();

                var separate = part.Split ("\t");
                for (int i = 0; i < properties.Count; i++)
                {
                    var key = properties [i];
                    var s = separate [i + 1];

                    values.Add ((key, s));
                }

                var c = g.GetComponent (parts [1]);
                c.ImportData (values);
            }
        }

        [MenuItem ("Project-Tools/Data Importer/Edit &D")]
        public static void Edit ()
        {
            var text = Resources.Load<TextAsset> (fileLink);
            if (!text)
            {
                CreateFileLink (Edit);
            }
            else
            {
                var url = text.text;
                Application.OpenURL (url);
            }
        }

        [MenuItem ("Project-Tools/Data Importer/Edit Link")]
        public static void EditLink ()
        {
            CreateFileLink (null);
        }

        private static void CreateFileLink (Action callback)
        {
            TextInputField.CreateWindow (OnSetupText);
            void OnSetupText (string text)
            {
                var assetFolder = "Assets";
                var resoucesFolder = "Resources";
                var resourcesPath = Path.Combine (assetFolder, resoucesFolder);

                if (!AssetDatabase.IsValidFolder (resourcesPath))
                    AssetDatabase.CreateFolder (assetFolder, resoucesFolder);

                var path = Path.Combine (resourcesPath, fileLink + ".txt");
                File.WriteAllText (path, text);
                AssetDatabase.Refresh ();

                callback?.Invoke ();
            }
        }
    }

    public class TextInputField : EditorWindow
    {
        public static void CreateWindow (Action<string> callback)
        {
            var window = GetWindow<TextInputField> (false, "");
            window.callback = callback;
            window.Show ();
        }

        Action<string> callback;
        string text = "";

        private void OnGUI ()
        {
            text = EditorGUILayout.TextField ("Input URL: ", text);

            GUILayout.FlexibleSpace ();

            if (GUILayout.Button ("Continue"))
            {
                if (!string.IsNullOrEmpty (text))
                {
                    callback?.Invoke (text);
                    Close ();
                }
            }
        }
    }
}