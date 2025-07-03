#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEditor;

public class EditorDownloader
{
    public static void Download (string url, string title, Action<DownloadHandler> callBack)
    {
        UnityWebRequest www = UnityWebRequest.Get (url);
        www.SendWebRequest ();

        while (!www.isDone)
        {
            var progress = www.downloadProgress;
            bool cancel = EditorUtility.DisplayCancelableProgressBar (title, string.Format ("Processing... {0}%", (int)(progress * 100)), progress);

            if (cancel)
            {
                break;
            }
        }

        EditorUtility.ClearProgressBar ();

        if (www.error != null)
        {
            Debug.Log ("UnityWebRequest.error:" + www.error);
        }
        else if (www.downloadHandler.text == "" || www.downloadHandler.text.IndexOf ("<!DOCTYPE") != -1)
        {
            Debug.Log ("Uknown Format:" + www.downloadHandler.text);
        }
        else
        {
            callBack?.Invoke (www.downloadHandler);
        }
    }
}
#endif
