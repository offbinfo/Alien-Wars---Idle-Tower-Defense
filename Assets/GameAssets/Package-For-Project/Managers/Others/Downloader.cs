using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Downloader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void CreateInstance ()
    {
        new GameObject ("Downloader", typeof (Downloader));
    }

    public static Coroutine Download (string url, Action<float> onProcess, Action<DownloadHandler> callBack)
    {
        if (!Instance)
            return null;

        return Instance.StartCoroutine (Instance.IEDownload (url, onProcess, callBack));
    }

    private IEnumerator IEDownload (string url, Action<float> onProcess, Action<DownloadHandler> callBack)
    {
        UnityWebRequest www = UnityWebRequest.Get (url);
        www.SendWebRequest ();

        while (!www.isDone)
        {
            var progress = www.downloadProgress;
            onProcess?.Invoke (progress);
            yield return null;
        }

        if (www.error != null)
        {
            callBack?.Invoke (null);
        }
        else if (www.downloadHandler.text == "" || www.downloadHandler.text.IndexOf ("<!DOCTYPE") != -1)
        {
            callBack?.Invoke (null);
        }
        else
        {
            callBack?.Invoke (www.downloadHandler);
        }
    }

    public static Coroutine Upload (string url, WWWForm form, Action<bool> callback)
    {
        if (!Instance)
            return null;

        return Instance.StartCoroutine (Instance.IEUpload (url, form, callback));
    }

    private IEnumerator IEUpload (string url, WWWForm form, Action<bool> callback)
    {
        UnityWebRequest request = UnityWebRequest.Post (url, form);
        request.SendWebRequest ();

        yield return new WaitUntil (() => request.isDone);

        callback?.Invoke (request.error != null);
    }

    static Downloader Instance;
    private void Awake ()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad (this);
        }
        else
        {
            Destroy (gameObject);
        }
    }
}
