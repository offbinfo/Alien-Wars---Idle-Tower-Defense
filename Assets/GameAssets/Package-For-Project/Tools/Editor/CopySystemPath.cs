using UnityEngine;
using UnityEditor;

public static class CopySystemPath
{
    [MenuItem("Assets/Copy System Path")]
    public static void Action()
    {
        var selected = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath (selected);
        path = Application.dataPath + path.Remove(0, "Assets".Length);

        EditorGUIUtility.systemCopyBuffer = path;
    }

    [MenuItem ("Project-Tools/Show Persistent-Data-Folder")]
    public static void ShowPersistentDataPathFolder()
    {
        Application.OpenURL (Application.persistentDataPath);
    }
}
