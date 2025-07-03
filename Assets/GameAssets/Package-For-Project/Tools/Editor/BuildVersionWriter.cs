using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildVersionWriter : MonoBehaviour, IPreprocessBuildWithReport
{
    public int callbackOrder { get; }

    public void OnPreprocessBuild (BuildReport report)
    {
        WriteVersion ();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void WriteVersion ()
    {
        var path = Path.Combine (Application.dataPath, "Resources", "build-version.txt");
        var pathAsset = Path.Combine ("Assets", "Resources", "build-version.txt");
#if UNITY_ANDROID
        var text = string.Format ("Version: {0} - Build: {1}", Application.version, PlayerSettings.Android.bundleVersionCode);
#else
        var text = string.Format ("Version: {0} - Build: {1}", Application.version, PlayerSettings.iOS.buildNumber);
#endif
        File.WriteAllText (path, text);
        AssetDatabase.ImportAsset (pathAsset);
        AssetDatabase.SaveAssets ();
    }
}
