using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeGame
{
    static float timeScaleAds;
    public static float TimeScaleAds
    {
        get => timeScaleAds; set
        {
            timeScaleAds = value;
            SetTimeScale();
        }
    }
    static float timeScale = 1f;

    public static float TimeScale
    {
        get { return timeScale; }
        set
        {
            timeScale = value;
            SetTimeScale();
        }
    }
    static bool pause;
    public static bool Pause
    {
        get { return pause; }
        set
        {
            pause = value;
            SetTimeScale();
        }
    }
    static float timeGameplay = 1f;

    public static float TimeGameplay
    {
        get { return timeGameplay; }
        set
        {
            timeGameplay = value;
            SetTimeScale();
        }
    }
    static bool pauseTutorial;
    public static bool PauseTutorial
    {
        get { return pauseTutorial; }
        set
        {
            pauseTutorial = value;
            SetTimeScale();
        }
    }



    [RuntimeInitializeOnLoadMethod]
    public static void AddEventScene()
    {
        //SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }
    private static void SceneLoaded(Scene arg, LoadSceneMode arg1)
    {
        timeScale = 1f;
        pause = false;
        timeScaleAds = 1f;
    }
    private static void SceneUnloaded(Scene arg)
    {
        TimeScale = 1f;
        Pause = false;
        PauseTutorial = false;
        TimeScaleAds = 1f;
        TimeGameplay = 1f;
    }
    private static void SetTimeScale()
    {
        Time.timeScale = timeScale * (pause ? 0f : 1f) * timeGameplay * (pauseTutorial ? 0f : 1f) * TimeScaleAds;
    }
}
