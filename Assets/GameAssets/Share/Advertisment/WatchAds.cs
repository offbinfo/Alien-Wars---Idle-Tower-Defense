using UnityEngine;
using System;
//using language;

public class WatchAds
{
    public static bool IsLoadedVideoRewarded ()
    {
        return IsLoadedVideoRewardedApplovin ();
    }

    public static void WatchRewardedVideo(Action onComplete, string placeEvent)
    {
        WatchRewardedVideo(onComplete, null, placeEvent);
    }
    public static void WatchRewardedVideo(Action onComplete, Action onClose, string placeEvent)
    {
        WatchRewardedVideo(() =>
        {
            QuestEventManager.AdsWatched(1);
            EventChallengeListenerManager.AdsWatched(1);

            onComplete?.Invoke();
            GameAnalytics.LogEvent_rewardAds(placeEvent);
        },onClose);
    }
    public static void WatchRewardedVideo(Action onComplete, Action onClose)
    {
        if (GameDatas.RemoveAdsForever)
        {
            onComplete?.Invoke ();
            onClose?.Invoke ();
            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //GameNotice.PushNotice (LanguageManager.GetText ("notice_noInternet"));
            return;
        }

        if (IsLoadedVideoRewardedApplovin ())
            ShowVideoRewardedApplovin (onComplete, onClose);
        //else
        //    GameNotice.PushNotice (LanguageManager.GetText ("notice_no_ads_loaded"));
    }

    public static bool IsLoadedVideoInterstitial ()
    {
        return IsLoadedVideoInterstitialApplovin ();
    }

    public static void WatchInterstitialVideo ()
    {
        ShowVideoInterstitialApplovin ();
    }

    #region applovin
    static bool IsLoadedVideoRewardedApplovin ()
    {
        if (GameDatas.RemoveAdsForever)
            return true;

        return ApplovinAds.Instance && ApplovinAds.Instance.IsReadyRewardedVideo ();
    }

    public static void ShowVideoRewardedApplovin (Action onComplete, Action onClose)
    {
        if (GameDatas.RemoveAdsForever)
        {
            onComplete?.Invoke ();
            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //GameNotice.PushNotice (LanguageManager.GetText ("notice_noInternet"));
            return;
        }

        if (IsLoadedVideoRewardedApplovin ())
        {
            GameLayout.ShowProcess (true);
            TimeGame.TimeScaleAds = 0;
            AudioListener.pause = true;
            ApplovinAds.Instance.ShowRewardedVideo (delegate (bool success)
            {
                if (success)
                {
                    onComplete?.Invoke ();
                }
            }, () =>
            {
                GameLayout.ShowProcess (false);
                TimeGame.TimeScaleAds = 1;
                AudioListener.pause = false;
                onClose?.Invoke ();
            });
        }
    }

    static bool IsLoadedVideoInterstitialApplovin ()
    {
        if (GameDatas.RemoveAdsForever)
            return true;

        return ApplovinAds.Instance && ApplovinAds.Instance.IsReadyInterstitialVideo ();
    }

    public static void ShowVideoInterstitialApplovin ()
    {
        if (GameDatas.RemoveAdsForever)
        {
            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }

        if (IsLoadedVideoInterstitialApplovin ())
        {
            ApplovinAds.Instance.ShowInterstitialVideo ();
        }
    }
    #endregion
}
