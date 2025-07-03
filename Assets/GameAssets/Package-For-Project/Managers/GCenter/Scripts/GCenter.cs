using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace gcenterSdk
{
    public class GCenter
    {
        static Dictionary<int, GCenterAdsStruct> adsStructs = new Dictionary<int, GCenterAdsStruct> ();

        static GCenterAdsStruct adsStruct;

        public static Action onLoaded;
        public static Action onClose;

        [RuntimeInitializeOnLoadMethod]
        public static void Request ()
        {
            var urlConfig = "https://gcenterstudio.com/gcenter-ads-data-config.json";
            Downloader.Download (urlConfig, null, OnConfigLoaded);
            void OnConfigLoaded (DownloadHandler handler)
            {
                var json = handler.text;
                var dataStruct = JsonParse.FromJson<DataStruct> (json);

                if (dataStruct == default)
                    return;

                var configs = dataStruct.games;

                if (configs == null || configs.Count == 0)
                    return;

                var index = Random.Range (0, configs.Count);
                var config = configs [index];

                if (!adsStructs.ContainsKey (index))
                    LoadWithConfig (index, config);
                else
                    SetAdsStruct (adsStructs [index]);
            }
        }

        private static void LoadWithConfig (int index, ConfigStruct config)
        {
            var adsStruct = new GCenterAdsStruct ();
            var loadProcess = 0;
            var vetical = Screen.width < Screen.height;

            var urlIcon = config.urlIcon;
            Download (urlIcon, (data) => adsStruct.icon = GetTexture (data));

            var urlImage = vetical ? config.urlImageVertical : config.urlImageHorizontal;
            if (!string.IsNullOrEmpty (urlImage))
                Download (urlImage, (data) => adsStruct.largeImage = GetTexture (data));

            var urlVideo = vetical ? config.urlVideoVertical : config.urlVideoHorizontal;
            if (!string.IsNullOrEmpty (urlVideo))
                Download (urlVideo, (data) => GetUrlVideo ("video_" + index, data, (path) => adsStruct.clipUrl = path));

#if UNITY_ANDROID
            adsStruct.url = config.urlAndroid;
#else
            adsStruct.url = config.urlIOS;
#endif

            adsStruct.title = config.title;

            adsStruct.time = config.time;

            void Download (string url, Action<byte []> callback)
            {
                loadProcess++;
                Downloader.Download (url, null, OnLoaded);
                void OnLoaded (DownloadHandler handle)
                {
                    callback?.Invoke (handle.data);
                    CheckProcess ();
                }

                void CheckProcess ()
                {
                    loadProcess--;
                    if (loadProcess == 0)
                    {
                        adsStructs.Add (index, adsStruct);
                        SetAdsStruct (adsStruct);
                    }
                }
            }
        }

        private static Sprite GetTexture (byte [] data)
        {
            var tex = new Texture2D (0, 0);
            tex.LoadImage (data);
            tex.Apply ();

            return Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), Vector2.one / 2f);
        }

        private static void GetUrlVideo (string fileName, byte [] data, Action<string> callback)
        {
            var path = Path.Combine (Application.persistentDataPath, fileName + ".mp4");
            File.WriteAllBytesAsync (path, data).ContinueWith ((task) => callback?.Invoke (path));
        }

        private static void SetAdsStruct (GCenterAdsStruct adsStruct)
        {
            GCenter.adsStruct = adsStruct;
            onLoaded?.Invoke ();
        }

        public static bool Loaded ()
        {
            return adsStruct != null;
        }

        public static void Show ()
        {
            if (!Loaded ())
                return;

            if (!GCenterAdsViewer.instance)
                Object.Instantiate (Resources.Load ("GCenterAdsView"));
            else if (GCenterAdsViewer.instance.gameObject.activeSelf)
                return;

            var adView = GCenterAdsViewer.instance;

            adView.Setup (adsStruct);

            Request ();
        }
    }
}