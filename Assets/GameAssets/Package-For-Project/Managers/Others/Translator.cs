using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Translator : MonoBehaviour
{
    public static void Translate (string input, string codeOut, Action<string> callback)
    {
        var url = string.Format ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", "auto", codeOut, input);

        Downloader.Download (url, null, OnLoaded);

        void OnLoaded (DownloadHandler downloadHandler)
        {
            if (downloadHandler != null)
            {
                var text = downloadHandler.text;
                var list = JsonParse.FromJson<JArray> (text);

                if (list != default)
                {
                    var listText = list [0];
                    var s = "";
                    foreach (var item in listText)
                    {
                        s += item [0].ToString ();
                    }
                    callback?.Invoke (s);
                }
            }
            else
            {
                callback?.Invoke (input);
            }
        }
    }
}
