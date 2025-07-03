using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace language
{
    public class LanguageAttribute : PropertyAttribute
    {
        public List<string> languages = new List<string> ();
        public LanguageAttribute ()
        {
            LanguageAssets languageAssets = null;
            var assets = Resources.LoadAll<LanguageAssets> ("");
            if (assets.Length > 0)
            {
                languageAssets = assets [0];
            }

            if (languageAssets && languageAssets.DictionaryLanguage.Count > 0)
                languages = new List<string> (languageAssets.DictionaryLanguage.Keys);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer (typeof (LanguageAttribute))]
    public class LanguageDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            LanguageAttribute script = attribute as LanguageAttribute;
            if (script.languages.Count > 0)
            {
                int index = EditorGUI.Popup (position, property.displayName, script.languages.IndexOf (property.stringValue), script.languages.ToArray ());
                property.stringValue = script.languages [Mathf.Max (0, index)];
            }
            else
            {
                EditorGUI.LabelField (position, "ERRO:", "HAVE NO LANGUAGE");
            }
        }
    }
#endif
}
