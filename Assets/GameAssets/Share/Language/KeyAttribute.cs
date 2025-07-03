using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace language
{
    public class KeyAttribute : PropertyAttribute
    {
        public List<string> keys = new List<string> ();
        public KeyAttribute ()
        {
            LanguageAssets languageAssets = null;
            var assets = Resources.LoadAll<LanguageAssets> ("");
            if (assets.Length > 0)
            {
                languageAssets = assets [0];
            }

            if (languageAssets)
            {
                if (languageAssets.DictionaryLanguage.Count > 0)
                {
                    List<string> k = new List<string> (languageAssets.DictionaryLanguage.Keys);
                    keys = new List<string> (languageAssets.DictionaryLanguage [k [0]].Keys);
                    keys.Sort ((x, y) => x.CompareTo (y));
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer (typeof (KeyAttribute)), CanEditMultipleObjects]
    public class KeyDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            KeyAttribute script = attribute as KeyAttribute;
            if (script.keys.Count > 0)
            {
                EditorGUI.LabelField (position, label);

                float d = position.max.x * 4f / 10;
                Rect positionButton = new Rect (position.position + new Vector2 (d, 0), position.size - new Vector2 (d, 0));

                if (EditorGUI.DropdownButton (positionButton, new GUIContent (property.stringValue), FocusType.Passive))
                {
                    SearchWindow.Display ("Select key", script.keys.IndexOf (property.stringValue), script.keys, OnSelect);
                    void OnSelect (int index)
                    {
                        property.stringValue = script.keys [Mathf.Max (0, index)];
                        property.serializedObject.ApplyModifiedProperties ();
                    };
                }
            }
            else
            {
                EditorGUI.LabelField (position, "ERROR:", "HAVE NO KEY");
            }
        }
    }
#endif
}
