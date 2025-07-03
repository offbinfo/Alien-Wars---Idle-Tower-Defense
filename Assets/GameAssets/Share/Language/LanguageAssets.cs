using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;


#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace language
{
    [CreateAssetMenu (menuName = "Language/LanguageAssets")]
    public class LanguageAssets : ScriptableObject
    {
        [SerializeField] public string sheetLink = null;
        [SerializeField] string path = null;
        [SerializeField] TextAsset [] textAssets = null;
        [SerializeField, Language] string languageDefault = null;

        [Header ("Language System")]
        [SerializeField] List<RegionDefault> regionDefaults = null;

        [Serializable]
        public class RegionDefault
        {
            public SystemLanguage region;
            [Language] public string language;
        }

        public string LanguageDefault
        {
            get
            {
                string languageDefault = this.languageDefault;

                RegionDefault regionDefault = regionDefaults.Find (x => x.region == Application.systemLanguage);

                if (regionDefault != null)
                {
                    languageDefault = regionDefault.language;
                }

                return languageDefault;
            }
        }

        public string pathSource
        {
            get
            {
                return path;
            }
        }

        Dictionary<string, Dictionary<string, string>> dictionaryLanguage = new Dictionary<string, Dictionary<string, string>> ();
        public Dictionary<string, Dictionary<string, string>> DictionaryLanguage
        {
            get
            {
                if (dictionaryLanguage.Count == 0)
                    LoadData ();
                return dictionaryLanguage;
            }
        }

#if UNITY_EDITOR
        [Button("Load Text Assets")]
        public void LoadTextAssets ()
        {
            if (!Directory.Exists (pathSource))
                return;

            dictionaryLanguage.Clear ();
            List<string> allFile = new List<string> (Directory.EnumerateFiles (pathSource, "*.txt"));
            textAssets = new TextAsset [allFile.Count];
            for (int i = 0; i < allFile.Count; i++)
            {
                textAssets [i] = AssetDatabase.LoadAssetAtPath<TextAsset> (allFile [i]);
            }

            EditorUtility.SetDirty (this);
            AssetDatabase.SaveAssets ();
            LoadData ();

            EditorUtility.DisplayDialog ("Language", "Load success!", "Close");
        }
#endif

        public void LoadData ()
        {
            if (textAssets.Length == 0)
                return;

            dictionaryLanguage.Clear ();

            for (int i = 0; i < textAssets.Length; i++)
            {
                TextAsset textAsset = textAssets [i];
                Dictionary<string, Dictionary<string, string>> dic = GetTextFromFile (textAsset);

                List<string> keys = new List<string> (dic.Keys);
                for (int k = 0; k < keys.Count; k++)
                {
                    string key = keys [k];

                    if (!dictionaryLanguage.ContainsKey (key))
                    {
                        dictionaryLanguage.Add (key, new Dictionary<string, string> ());
                    }

                    dictionaryLanguage [key].AddRange (dic [key]);
                }
            }
        }

        // Get text from file
        Dictionary<string, Dictionary<string, string>> GetTextFromFile (TextAsset textAsset)
        {
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>> ();
            string [] lines = ReadAllLines (textAsset);

            string line0 = lines [0].Trim ();
            List<string> listWord = new List<string> (line0.Split ('\t'));
            for (int i = 1; i < listWord.Count; i++)
            {
                string word = listWord [i];

                if (!dic.ContainsKey (word))
                    dic.Add (word, new Dictionary<string, string> ());
                else
                    DebugCustom.Log ("Exist key " + word);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty (lines [i]) || string.IsNullOrWhiteSpace (lines [i]))
                {
                    continue;
                }

                string line = lines [i].Trim ();
                string [] part = line.Split ('\t');

                for (int c = 1; c < listWord.Count; c++)
                {
                    if (!dic [listWord [c]].ContainsKey (part [0]))
                    {
                        try
                        {
                            dic [listWord [c]].Add (part [0], part [c].Replace ("\\n", "\n").Replace ("\"", ""));
                        }
                        catch (Exception e)
                        {
                            DebugCustom.Log (textAsset, textAsset);
                            DebugCustom.Log (part [0]);
                            DebugCustom.Log (e.Message);
                        }
                    }
                    else
                    {
                        DebugCustom.Log (string.Format ("Exist key {0}; line {1}", part [0], line), textAsset);
                    }
                }
            }
            return dic;
        }

        string [] ReadAllLines (TextAsset textAsset)
        {
            return textAsset.text.Split ('\n');
        }
    }

    public static class Extension
    {
        public static void AddRange (this Dictionary<string, string> dic, Dictionary<string, string> d)
        {
            List<string> keys = new List<string> (d.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (!dic.ContainsKey (keys [i]))
                    dic.Add (keys [i], d [keys [i]]);
                else
                    DebugCustom.Log (keys [i]);
            }
        }
    }

#if UNITY_EDITOR
    public static class MenuLanguageTool
    {
        [MenuItem ("LanguageTool/Settings")]
        public static void Select ()
        {
            var assets = Resources.LoadAll<LanguageAssets> ("");
            if (assets.Length == 0)
            {
                DebugCustom.Log ("Missing language assets.");
                return;
            }

            Selection.activeObject = assets [0];
        }

        [MenuItem ("LanguageTool/Edit File")]
        static void Edit ()
        {
            Application.OpenURL (LanguageManager.LanguageAssets.sheetLink);
        }

        [MenuItem ("LanguageTool/Load File From Buffer")]
        static void LoadFileFromBuffer ()
        {
            var assets = Resources.LoadAll<LanguageAssets> ("");
            if (assets.Length == 0)
            {
                DebugCustom.Log ("Missing language assets.");
                return;
            }

            var languageAsset = assets [0];
            SaveText (languageAsset);
            //CreateEnum (languageAsset);

            languageAsset.LoadTextAssets ();
        }

        private static void SaveText (LanguageAssets languageAsset)
        {
            var text = GUIUtility.systemCopyBuffer;

            var path = languageAsset.pathSource + "/languageAssets.txt";
            File.WriteAllText (path, text);
            AssetDatabase.ImportAsset (path);
        }

        private static void CreateEnum (LanguageAssets languageAsset)
        {
            var text = GUIUtility.systemCopyBuffer;

            var keys = "";

            var parts = text.Split ("\n");
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty (part))
                {
                    var collums = part.Split ("\t");

                    if (!string.IsNullOrEmpty (collums [0]))
                        keys += collums [0] + ",";
                }
            }

            var code = "public enum LanguageKey{**KEYS**}";
            code = code.Replace ("**KEYS**", keys);

            var path = languageAsset.pathSource + "/LanguageKey.cs";
            File.WriteAllText (path, code);
            AssetDatabase.ImportAsset (path);
        }
    }
#endif
}
