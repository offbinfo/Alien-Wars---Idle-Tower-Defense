using UnityEngine;

namespace gcenterSdk
{
    public class GCenterAdsStruct
    {
        public Sprite icon;
        public Sprite largeImage;
        public string clipUrl;

        public string title;
        public string url;
        public string platForm => Application.platform.ToString ();

        public float time;

        public int id;
    }
}