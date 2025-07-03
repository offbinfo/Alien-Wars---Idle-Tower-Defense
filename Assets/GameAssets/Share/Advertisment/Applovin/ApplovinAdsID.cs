public class ApplovinAdsID
{
    public static string rewardID
    {
        get
        {
#if UNITY_ANDROID
            return "09388499cfa0a415";
#else
            return "";
#endif
        }
    }
    public static string interstitialID
    {
        get
        {
#if UNITY_ANDROID
            return "e8fab02f1f3e1de9";
#else
            return "";
#endif
        }
    }
}
