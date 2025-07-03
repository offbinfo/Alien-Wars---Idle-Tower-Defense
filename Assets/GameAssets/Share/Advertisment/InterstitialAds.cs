using UnityEngine;

public class InterstitialAds
{
    static bool canShow => !GameDatas.RemoveAdsForever && CheckTime ();

    static float timeShow = 60f;
    static float time;

    public static void Show ()
    {
        if (!canShow)
            return;

        WatchAds.WatchInterstitialVideo ();
    }

    static bool CheckTime ()
    {
        if (Time.time - time < timeShow)
        {
            return false;
        }

        time = Time.time;
        return true;
    }
}
