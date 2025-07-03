using UnityEngine;
using UnityEngine.Android;

public class GameInit
{
    [RuntimeInitializeOnLoadMethod]
    public static void Init ()
    {
        Application.targetFrameRate = 50;
        QualitySettings.vSyncCount = 0;

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission ("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission ("android.permission.POST_NOTIFICATIONS");
        }
#endif
    }
}
