#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SearchWindow : EditorWindow
{
    List<string> displayNames;
    System.Action<int> onSelect;
    int indexSelected;

    string search = "";
    Vector2 pos;

    public static void Display (string title, int indexSelected, List<string> displayNames, System.Action<int> onSelect)
    {
        SearchWindow searchWindow = GetWindow<SearchWindow> (true, title);
        searchWindow.indexSelected = indexSelected;
        searchWindow.displayNames = displayNames;
        searchWindow.onSelect = onSelect;
        searchWindow.BeginWindows ();
    }

    private void OnGUI ()
    {
        search = EditorGUILayout.TextField ("Search", search);
        pos = GUILayout.BeginScrollView (pos);

        List<string> names = new List<string> ();

        for (int i = 0; i < displayNames.Count; i++)
        {
            var displayName = displayNames [i];

            string search = "";
            for (int j = 0; j < this.search.Length; j++)
            {
                var s = this.search [j];
                if (char.IsUpper (s) && j > 0)
                    search += " " + s;
                else
                    search += s;
            }

            string [] parts = search.Split (' ');

            bool match = true;
            for (int j = 0; j < parts.Length; j++)
            {
                match = match && displayName.Contains (parts [j]);
            }

            if (match)
                names.Add (displayName);
        }

        for (int i = 0; i < names.Count; i++)
        {
            GUIStyle style = new GUIStyle (GUI.skin.button);

            int index = displayNames.IndexOf (names [i]);
            if (index == indexSelected)
                style.normal.textColor = Color.red;

            if (GUILayout.Button (names [i], style))
            {
                onSelect?.Invoke (index);
                Close ();
            }
        }
        GUILayout.EndScrollView ();
    }
}
#endif
