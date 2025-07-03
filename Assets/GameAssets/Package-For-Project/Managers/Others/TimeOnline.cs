using UnityEngine;
using System;
using System.Net;
using System.Globalization;

public class TimeOnline
{
    static DateTime UTCValueStart, ValueStart;

    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init ()
    {
        UTCValueStart = GetTime (DateTimeStyles.None);
        ValueStart = GetTime (DateTimeStyles.AssumeUniversal);
    }

    // Date time online
    public static DateTime UTCValue => UTCValueStart.AddSeconds (Time.realtimeSinceStartup);

    public static DateTime Value => ValueStart.AddSeconds (Time.realtimeSinceStartup);

    public static void GetValueUTC (Action<DateTime> onLoaded)
    {
        GetTimeAsync (onLoaded, DateTimeStyles.None);
    }

    static void GetTimeAsync (Action<DateTime> onLoaded, DateTimeStyles timeStyles)
    {
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create ("http://www.google.com");
        myHttpWebRequest.BeginGetResponse (GetResponseCallBack, null);

        void GetResponseCallBack (IAsyncResult ar)
        {
            if (myHttpWebRequest.HaveResponse)
            {
                var response = myHttpWebRequest.EndGetResponse (ar);
                string todaysDates = response.Headers ["date"];
                response.Close ();
                DateTime time = DateTime.ParseExact (todaysDates,
                                            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                            CultureInfo.InvariantCulture.DateTimeFormat,
                                            timeStyles);
                onLoaded?.Invoke (time);
            }
            else
                onLoaded?.Invoke (DateTime.Now);
        }
    }

    static DateTime GetTime (DateTimeStyles timeStyles)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return timeStyles == DateTimeStyles.None ? DateTime.UtcNow : DateTime.Now;
        }
        else
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create ("http://www.google.com");
            var response = myHttpWebRequest.GetResponse ();
            string todaysDates = response.Headers ["date"];
            response.Close ();
            DateTime time = DateTime.ParseExact (todaysDates,
                                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                        CultureInfo.InvariantCulture.DateTimeFormat,
                                        timeStyles);

            return time;
        }
    }
}
