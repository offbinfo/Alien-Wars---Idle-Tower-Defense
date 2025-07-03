using System;
using System.Collections.Generic;

namespace gcenterSdk
{
    [Serializable]
    public class ConfigStruct
    {
        public string urlIcon;

        public string urlImageHorizontal;
        public string urlImageVertical;

        public string urlVideoHorizontal;
        public string urlVideoVertical;

        public string urlAndroid;
        public string urlIOS;

        public string title;
        public float time;
    }

    [Serializable]
    public class DataStruct
    {
        public List<ConfigStruct> games;
    }
}