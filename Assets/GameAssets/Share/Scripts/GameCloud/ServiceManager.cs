using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

using UnityEngine.SocialPlatforms;
using System;
using ProjectTools;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using System.Text;

public class ServiceManager : MonoBehaviour
{
    public static ServiceManager instance = null;

    [RuntimeInitializeOnLoadMethod]
    static void CreateInstance ()
    {
        if (!instance)
        {
            new GameObject ("Service-System", typeof (ServiceManager));
        }
    }

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            Init ();
            DontDestroyOnLoad (this);
        }
        else
            Destroy (gameObject);
    }

    private void Init ()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Activate ();
#endif

        SignIn (null);
    }

    public static void SignInService (Action<bool> onSuccess)
    {
        if (instance)
            instance.SignIn (onSuccess);
        else
        {
            if (onSuccess != null)
                onSuccess.Invoke (false);
        }
    }

    private void SignIn (Action<bool> onSuccess)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            onSuccess?.Invoke (false);
            return;
        }

#if UNITY_ANDROID
        if (PlayGamesPlatform.Instance == null)
        {
            onSuccess?.Invoke (false);
            return;
        }
#endif

        Social.localUser.Authenticate ((bool success) =>
       {
           StartCoroutine (IEWait (onSuccess, success));
           if (success)
           {
               // Sau khi đăng nhập thành công, đồng bộ dữ liệu
               SyncCloudData();
           }
           else
           {
               LoadLocalData(); // Tải dữ liệu cục bộ nếu đăng nhập thất bại
           }
       });
    }

    IEnumerator IEWait (Action<bool> onSuccess, bool success)
    {
        yield return new WaitForEndOfFrame ();
        onSuccess?.Invoke (success);
    }

    // Leader board
    public static void ReportScore (string idLeaderboard, int score, Action<bool> callBack)
    {
        if (!instance)
            return;

        if (instance.IsSignIn ())
            Social.ReportScore (score, idLeaderboard, callBack);
    }

    public static void ShowLeaderboardUI ()
    {
        if (!instance)
            return;

        if (instance.IsSignIn ())
            Social.ShowLeaderboardUI ();
        else
            instance.SignIn (delegate
           {
               Social.ShowLeaderboardUI ();
           }
        );
    }

    public static void ShowLeaderboardUI (string idLeadboard)
    {
        if (!instance)
            return;

        if (instance.IsSignIn ())
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowLeaderboardUI (idLeadboard);
#else
            Social.ShowLeaderboardUI ();
#endif
        }
        else
            instance.SignIn (delegate
           {
#if UNITY_ANDROID
               PlayGamesPlatform.Instance.ShowLeaderboardUI (idLeadboard);
#else
                Social.ShowLeaderboardUI ();
#endif
           }
        );
    }

    public static bool IsSignInService ()
    {
        return instance && instance.IsSignIn ();
    }

    bool IsSignIn ()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        return GetUser ().authenticated;
    }

    public static ILocalUser GetUser ()
    {
        return GameSettings.TestService ? TestUser.instance : Social.localUser;
    }

    // Đồng bộ dữ liệu với đám mây
    private void SyncCloudData()
    {
#if UNITY_ANDROID
        if (!IsSignIn())
        {
            DebugCustom.LogWarning("Cannot sync cloud data: User not authenticated");
            LoadLocalData();
            return;
        }

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (savedGameClient == null)
        {
            DebugCustom.LogError("SavedGameClient is null. Ensure Google Play Games is properly initialized.");
            LoadLocalData();
            return;
        }

        // Open the saved game file with automatic conflict resolution
        savedGameClient.OpenWithAutomaticConflictResolution(
            "GameDatas",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    // Read the saved game data
                    savedGameClient.ReadBinaryData(game, (readStatus, data) =>
                    {
                        if (readStatus == SavedGameRequestStatus.Success && data != null && data.Length > 0)
                        {
                            // Parse cloud data
                            /*string cloudJson = Encoding.UTF8.GetString(data);
                            GameDatasWrapper cloudData = JsonUtility.FromJson<GameDatasWrapper>(cloudJson);
                            GameDatasWrapper localData = GameDatasSerializer.LoadFromJson();

                            if (cloudData != null)
                            {
                                // Compare timestamps
                                if (localData == null || cloudData.lastModified > localData.lastModified)
                                {
                                    GameDatasSerializer.ApplyLoadedData(cloudData);
                                    Debug.Log("Applied cloud data");
                                    // Save locally to ensure consistency
                                    GameDatasSerializer.SaveToJson();
                                }
                                else
                                {
                                    Debug.Log("Local data is newer or equal, uploading to cloud");
                                    SaveToCloud();
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Invalid cloud data, using local data");
                                if (localData != null)
                                {
                                    GameDatasSerializer.ApplyLoadedData(localData);
                                    SaveToCloud();
                                }
                            }*/
                        }
                        else
                        {
                            /*Debug.Log("No cloud data found, using local data");
                            LoadLocalData();
                            SaveToCloud();*/
                        }
                    });
                }
                else
                {
                    /*Debug.LogWarning($"Failed to open saved game: {status}");
                    LoadLocalData();*/
                }
            });
#else
        LoadLocalData(); // Non-Android platforms use local data only
#endif
    }

    // Tải dữ liệu cục bộ
    private void LoadLocalData()
    {
        /*GameDatasWrapper loadedData = GameDatasSerializer.LoadFromJson();
        if (loadedData != null)
        {
            GameDatasSerializer.ApplyLoadedData(loadedData);
            DebugCustom.Log("Applied local data");
        }
        else
        {
            DebugCustom.Log("No local data found, starting fresh");
        }*/
    }

    // Lưu dữ liệu lên đám mây
    public static void SaveToCloud()
    {
#if UNITY_ANDROID
        if (!instance || !instance.IsSignIn())
        {
            DebugCustom.LogWarning("Cannot save to cloud: User not authenticated");
            return;
        }

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (savedGameClient == null)
        {
            DebugCustom.LogError("SavedGameClient is null. Ensure Google Play Games is properly initialized.");
            return;
        }

        // Open the saved game file
        savedGameClient.OpenWithAutomaticConflictResolution(
            "GameDatas",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    // Create and serialize GameDatasWrapper
                    /*GameDatasWrapper wrapper = new GameDatasWrapper();
                    wrapper.lastModified = DateTime.UtcNow;
                    string json = JsonUtility.ToJson(wrapper);
                    byte[] data = Encoding.UTF8.GetBytes(json);

                    // Update metadata
                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                        .WithUpdatedDescription("Game data for " + GetUser().id)
                        .WithUpdatedPlayedTime(TimeSpan.FromSeconds(0))
                        .Build();

                    // Commit the update
                    savedGameClient.CommitUpdate(game, update, data, (commitStatus, updatedGame) =>
                    {
                        if (commitStatus == SavedGameRequestStatus.Success)
                        {
                            DebugCustom.Log("Successfully saved to cloud");
                        }
                        else
                        {
                            DebugCustom.LogError($"Failed to save to cloud: {commitStatus}");
                        }
                    });*/
                }
                else
                {
                    DebugCustom.LogError($"Failed to open saved game for saving: {status}");
                }
            });
#endif
    }

}
