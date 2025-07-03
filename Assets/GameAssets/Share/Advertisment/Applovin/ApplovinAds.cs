using System;
using System.Collections;
using UnityEngine;
using Firebase.Analytics;

public class ApplovinAds : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void CreateInstance ()
    {
        if (!Instance)
        {
            new GameObject ("ApplovinAds-System", typeof (ApplovinAds));
        }
    }

    public static ApplovinAds Instance;

    string rewardID => ApplovinAdsID.rewardID;
    string interstitialID => ApplovinAdsID.interstitialID;

    int countReloadReward = 3;
    int countReloadInterstitial = 3;

    Action onCloseRewardedVideo;
    Action<bool> onCompleteRewardedVideo;

    //---------//
    public void ShowRewardedVideo (Action<bool> onComplete, Action onClose)
    {
        onCompleteRewardedVideo = onComplete;
        onCloseRewardedVideo = onClose;
        MaxSdk.ShowRewardedAd (rewardID);
    }

    public bool IsReadyRewardedVideo ()
    {
        if (!MaxSdk.IsInitialized ())
            return false;

        return MaxSdk.IsRewardedAdReady (rewardID);
    }

    //---------//
    public void ShowInterstitialVideo ()
    {
        MaxSdk.ShowInterstitial (interstitialID);
    }

    public bool IsReadyInterstitialVideo ()
    {
        if (!MaxSdk.IsInitialized ())
            return false;

        return MaxSdk.IsInterstitialReady (interstitialID);
    }

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

    private void Start ()
    {
        AdsInit ();
    }

    private void AdsInit ()
    {
        Debug.Log ("applovin ads init");

        MaxSdk.InitializeSdk ();
        MaxSdkCallbacks.OnSdkInitializedEvent += (initStatus) => {
            RewardedInit ();
            InterstitialInit ();
        };
    }

    #region rewarded
    private void RewardedInit ()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += Rewarded_OnAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += Rewarded_OnAdLoadFailedEvent; ;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += Rewarded_OnAdDisplayFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += Rewarded_OnAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Rewarded_OnAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += Rewarded_OnAdRevenuePaidEvent;

        LoadRewardedAd ();
    }

    private void Rewarded_OnAdRevenuePaidEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        StartCoroutine (IETrackAdRevenue (arg2));
    }

    private void Rewarded_OnAdHiddenEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log ("hidden");
        StartCoroutine (IEClosedRewarded ());
    }

    private void Rewarded_OnAdReceivedRewardEvent (string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log ("received rewarded");
        StartCoroutine (IEEarnedRewarded ());
    }

    private void Rewarded_OnAdDisplayFailedEvent (string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log ("fail show");
        StartCoroutine (IEFailToShowRewarded ());
    }

    private void Rewarded_OnAdLoadFailedEvent (string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log ("fail load");
        StartCoroutine (IEFailToLoadRewarded ());
    }

    private void Rewarded_OnAdLoadedEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log ("loaded");
        EventDispatcher.PostEvent (EventID.AdsChanged, null);
    }

    private void LoadRewardedAd ()
    {
        MaxSdk.LoadRewardedAd (rewardID);
    }

    private IEnumerator IEFailToLoadRewarded ()
    {
        yield return new WaitForEndOfFrame ();
        if (countReloadReward > 0)
        {
            countReloadReward--;
            Invoke ("LoadRewardedAd", 10f);
        }
    }

    private IEnumerator IEFailToShowRewarded ()
    {
        yield return new WaitForEndOfFrame ();
        onCompleteRewardedVideo?.Invoke (false);
        EventDispatcher.PostEvent (EventID.AdsChanged, null);
    }

    private IEnumerator IEClosedRewarded ()
    {
        yield return new WaitForEndOfFrame ();
        onCloseRewardedVideo?.Invoke ();
        LoadRewardedAd ();
    }

    private IEnumerator IEEarnedRewarded ()
    {
        yield return new WaitForEndOfFrame ();
        onCompleteRewardedVideo?.Invoke (true);
        EventDispatcher.PostEvent (EventID.AdsChanged, null);
    }
    #endregion

    #region interstitial
    private void InterstitialInit ()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += Interstitial_OnAdLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += Interstitial_OnAdLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += Interstitial_OnAdHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += Interstitial_OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += Interstitial_OnAdDisplayFailedEvent;

        LoadInterstitialAd ();
    }

    private void Interstitial_OnAdDisplayFailedEvent (string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log ("interstitial display failed!");
        StartCoroutine (IEWaitToLoadInterstitial ());
    }

    private void Interstitial_OnAdLoadedEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        countReloadInterstitial = 3;
        Debug.Log ("interstitial loaded!");
    }

    private void Interstitial_OnAdRevenuePaidEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        StartCoroutine (IETrackAdRevenue (arg2));
    }

    private void Interstitial_OnAdHiddenEvent (string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log ("interstitial hidden!");
        StartCoroutine (IEWaitToLoadInterstitial ());
    }

    private void Interstitial_OnAdLoadFailedEvent (string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log ("interstitial load failed!");

        StartCoroutine (IEFailToLoadInterstitial ());
    }

    private void LoadInterstitialAd ()
    {
        Debug.Log ("start load interstitial " + interstitialID);
        MaxSdk.LoadInterstitial (interstitialID);
    }

    private IEnumerator IEFailToLoadInterstitial ()
    {
        yield return new WaitForEndOfFrame ();

        if (countReloadInterstitial > 0)
        {
            countReloadInterstitial--;
            Invoke ("LoadInterstitialAd", 10f);
        }
    }

    private IEnumerator IEWaitToLoadInterstitial ()
    {
        yield return new WaitForEndOfFrame ();
        LoadInterstitialAd ();
    }

    #endregion

    private IEnumerator IETrackAdRevenue (MaxSdkBase.AdInfo adInfo)
    {
        yield return new WaitForEndOfFrame ();
        TrackAdRevenue (adInfo);
    }

    private static void TrackAdRevenue (MaxSdkBase.AdInfo adInfo)
    {
        //AdjustTracking (adInfo);
        FirebaseTracking (adInfo);
        //AppsflyerTracking (adInfo);
    }

    //private static void AdjustTracking (MaxSdkBase.AdInfo adInfo)
    //{
    //    AdjustAdRevenue ad = new AdjustAdRevenue (AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
    //    ad.setRevenue (adInfo.Revenue, "USD");
    //    ad.setAdRevenueNetwork (adInfo.NetworkName);
    //    ad.setAdRevenueUnit (adInfo.AdUnitIdentifier);
    //    ad.setAdRevenuePlacement (adInfo.Placement);
    //    Adjust.trackAdRevenue (ad);
    //}

    private static void FirebaseTracking (MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        var impressionParameters = new []
        {
            new Parameter("ad_platform", "AppLovin"),
            new Parameter("ad_source", adInfo.NetworkName),
            new Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
            new Parameter("ad_format", adInfo.AdFormat),
            new Parameter("value", revenue),
            new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        };

        FirebaseAnalytics.LogEvent ("ad_impression", impressionParameters);
    }

    //private static void AppsflyerTracking (MaxSdkBase.AdInfo adInfo)
    //{
    //    AppsFlyerAdRevenue.logAdRevenue (
    //        adInfo.NetworkName,
    //        AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
    //        adInfo.Revenue, 
    //        "USD", 
    //        null);
    //}
}
