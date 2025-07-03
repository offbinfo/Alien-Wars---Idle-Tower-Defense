using UnityEngine;
using UnityEditor;
using System.IO;

public class ShowProjectFolder
{
    [MenuItem("Project-Tools/Open Project Folder")]
    public static void Show()
    {
        string path = Directory.GetCurrentDirectory ();
        Application.OpenURL (path);
    }
}
