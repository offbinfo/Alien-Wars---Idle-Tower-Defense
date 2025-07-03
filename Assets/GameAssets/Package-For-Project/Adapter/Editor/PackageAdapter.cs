using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PackageAdapter : EditorWindow
{
    [MenuItem("Project-Tools/Adapter", priority = 0)]
    static void CreateWindow ()
    {
        var adapter = GetWindow<PackageAdapter> (false);
        adapter.Show ();
        adapter.LoadVersion ();
    }

    string version;
    string versionCurrent => AssetDatabase.LoadAssetAtPath<TextAsset> ("Assets/GameAssets/Package-For-Project/Adapter/version.txt").text;

    private void LoadVersion ()
    {
        var url = "https://gcenterstudio.com/hieube/package-for-project/version.txt";
        EditorDownloader.Download (url, "Load version", OnLoaded);
        void OnLoaded (DownloadHandler obj)
        {
            version = obj.text;
        }
    }

    private void LoadPackage ()
    {
        var url = "https://gcenterstudio.com/hieube/package-for-project/package.unitypackage";

        EditorDownloader.Download (url, "Load package", OnLoaded);
        void OnLoaded (DownloadHandler obj)
        {
            var datas = obj.data;
            var path = Application.persistentDataPath + "/" + "package-for-project.unitypackage";
            File.WriteAllBytes (path, datas);
            AssetDatabase.ImportPackage (path, true);
        }
    }

    private void OnGUI ()
    {
        GUILayout.Label (string.Format ("Version: {0}", string.IsNullOrEmpty (versionCurrent) ? "---" : versionCurrent));

        if (!string.IsNullOrEmpty (version))
        {
            var ver = Version.Parse (version);
            var verCurrent = Version.Parse (versionCurrent);

            if (verCurrent < ver)
            {
                if (GUILayout.Button (string.Format ("{0}({1})", "Upgrade", ver)))
                {
                    LoadPackage ();
                }
            }
        }
    }
}
