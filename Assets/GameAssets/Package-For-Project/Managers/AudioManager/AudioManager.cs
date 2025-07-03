using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void CreateInstance ()
    {
        if (!Instance)
        {
            new GameObject ("AudioManager-System", typeof (AudioManager));
        }
    }

    public static AudioManager Instance = null;

    GameObject soundParent = null;
    List<AudioSource> listAudioSourceSound = new List<AudioSource> ();

    GameObject musicParent = null;
    List<AudioSource> listAudioSourceMusic = new List<AudioSource> ();

    static Dictionary<string, AudioPackClip> dictionaryAudioClip = new Dictionary<string, AudioPackClip> ();

    public static float VolumeSound
    {
        set
        {
            if (Instance != null)
            {
                List<AudioSource> listAudioSource = Instance.listAudioSourceSound;

                for (int i = 0; i < listAudioSource.Count; i++)
                {
                    listAudioSource[i].volume = value;
                }
            }

            SavePrefs.SetFloat (GameKeys.Key_VolumeSound, value);
        }

        get => SavePrefs.GetFloat (GameKeys.Key_VolumeSound, 1f);
    }

    public static float VolumeMusic
    {
        set
        {
            if (Instance != null)
            {
                List<AudioSource> listAudioSource = Instance.listAudioSourceMusic;

                for (int i = 0; i < listAudioSource.Count; i++)
                {
                    listAudioSource[i].volume = value;
                }
            }

            SavePrefs.SetFloat (GameKeys.Key_VolumeMusic, value);
        }

        get => SavePrefs.GetFloat (GameKeys.Key_VolumeMusic, 1f);
    }

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad (this);

            CreateObject ();
            GetData ();
        }
        else
        {
            Destroy (gameObject);
        }
    }

    private void Start ()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        EventDispatcher.AddEvent(EventID.OnLoopSong, OnLoopSong);
    }

    private void OnLoopSong(object obj)
    {
        List<AudioSource> listAudioSource = Instance.listAudioSourceMusic;

        for (int i = 0; i < listAudioSource.Count; i++)
        {
            listAudioSource[i].loop = GameDatas.IsLoopSong == 1 ? true : false;
        }
    }

    private void OnDestroy ()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged (Scene arg0, Scene arg1)
    {
        Clean ();
    }

    void CreateObject ()
    {
        soundParent = new GameObject ("SoundParent");
        soundParent.transform.SetParent (this.transform);

        musicParent = new GameObject ("MusicParent");
        musicParent.transform.SetParent (this.transform);
    }

    public void LoadResource ()
    {
        listPackAudioClip = Resources.LoadAll<AudioPackClip> ("");
    }

    AudioPackClip [] listPackAudioClip = null;
    void GetData ()
    {
        if (listPackAudioClip == null || listPackAudioClip.Length == 0)
            LoadResource ();

        for (int i = 0; i < listPackAudioClip.Length; i++)
        {
            if (!dictionaryAudioClip.ContainsKey (listPackAudioClip [i].name))
                dictionaryAudioClip.Add (listPackAudioClip [i].name, listPackAudioClip [i]);
            else
                Debug.Log ("Exist key " + listPackAudioClip [i].name, listPackAudioClip [i]);
        }
    }

    public static AudioSource PlaySoundStatic (AudioPackClip audioPackClip)
    {
        if (Instance != null)
        {
            return Instance.PlaySound (audioPackClip.audioClip, audioPackClip.loop);
        }

        return null;
    }

    public static AudioSource PlaySoundStatic (string keyName)
    {
        return PlaySoundStatic (keyName, false);
    }

    public static AudioSource PlaySoundStatic (string keyName, bool overlap)
    {
        if (!Instance)
            return null;

        if (!overlap)
        {
            if (Instance.soundsPlaying.Contains (keyName))
                return null;
            else
            {
                Instance.soundsPlaying.Add (keyName);
                Instance.StartCoroutine (Instance.SoundMachine (keyName));
            }
        }

        if (!string.IsNullOrEmpty (keyName))
        {
            if (dictionaryAudioClip.ContainsKey (keyName))
                return PlaySoundStatic (dictionaryAudioClip [keyName]);
            else
            {
                //Debug.Log ("Not exist audioHellFire key " + keyName);
                return null;
            }
        }

        return null;
    }

    List<string> soundsPlaying = new List<string> ();
    IEnumerator SoundMachine (string keyName)
    {
        yield return new WaitForSecondsRealtime (.1f);
        soundsPlaying.Remove (keyName);
    }

    public static void PlayMusicStatic (AudioPackClip audioPackClip)
    {
        if (Instance != null)
        {
            if (audioPackClip != null)
                Instance.PlayMusic (audioPackClip.audioClip, audioPackClip.loop, 1);
            else
                Instance.PlayMusic (null, false, 1);
        }
    }

    public static float PlayMusicStatic (string keyName)
    {
        if (dictionaryAudioClip.ContainsKey (keyName))
        {
            PlayMusicStatic (dictionaryAudioClip [keyName]);
            return dictionaryAudioClip [keyName].audioClip.length;
        }
        else
        {
            Debug.Log ("Not exist key " + keyName);
            return 0f;
        }
    }

    public static void Pause ()
    {
        Instance?.AudioPauseHandle (Instance.soundParent, true);
    }

    public static void UnPause ()
    {
        Instance?.AudioPauseHandle (Instance.soundParent, false);
    }

    void AudioPauseHandle (GameObject parent, bool pause)
    {
        AudioSource [] audioSources = parent.GetComponents<AudioSource> ();
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources [i].loop)
            {
                if (pause)
                    audioSources [i].Pause ();
                else
                    audioSources [i].UnPause ();
            }
        }
    }

    public static void UnLoop ()
    {
        Instance?.AudioUnLoopHandle (Instance.soundParent);
        Instance?.AudioUnLoopHandle (Instance.musicParent);
    }

    void AudioUnLoopHandle (GameObject parent)
    {
        AudioSource [] audioSources = parent.GetComponents<AudioSource> ();
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources [i].loop = false;
        }
    }

    public static void Clean ()
    {
        Instance?.CleanAudio (Instance.soundParent);
    }

    void CleanAudio (GameObject parent)
    {
        AudioSource [] audioSources = parent.GetComponents<AudioSource> ();
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources [i].Stop ();
        }
    }

    public AudioSource PlaySound (AudioClip audioClip, bool loop)
    {
        return PlaySound (audioClip, loop, 1);
    }

    AudioSource PlaySound (AudioClip audioClip, bool loop, float volumeScale)
    {
        AudioSource audioSource = GetAudioFree (listAudioSourceSound, soundParent);
        audioSource.playOnAwake = false;
        audioSource.loop = loop;
        audioSource.volume = VolumeSound * volumeScale;
        audioSource.clip = audioClip;
        audioSource.Play ();

        return audioSource;
    }

    void PlayMusic (AudioClip audioClip, bool loop, float volumeScale)
    {
        AudioSource audioSource = GetAudioFree (listAudioSourceMusic, musicParent);
        audioSource.playOnAwake = false;
        audioSource.loop = loop;
        audioSource.clip = audioClip;
        audioSource.volume = VolumeMusic * volumeScale;

        StartCoroutine (SmoothChangeMusic (audioSource));
    }

    AudioSource oldAudioSourceMusic = null;
    IEnumerator SmoothChangeMusic (AudioSource audioSource)
    {
        AudioSource audioOld = oldAudioSourceMusic;
        oldAudioSourceMusic = audioSource;

        audioSource.volume = 0;
        audioSource.Play ();
        float volume = 0;
        float speed = 1f;

        while (volume < 1)
        {
            volume += speed * Time.unscaledDeltaTime;

            if (audioSource.clip != null)
            {
                audioSource.volume = volume * VolumeMusic;
            }

            if (audioOld != null && audioOld != audioSource)
            {
                if (audioOld.clip != null)
                {
                    audioOld.volume = (1 - volume) * VolumeMusic;
                }
            }
            yield return null;
        }

        if (audioOld != null && audioOld != audioSource)
        {
            audioOld.Stop ();
        }
    }

    AudioSource GetAudioFree (List<AudioSource> listAudioSource, GameObject parent)
    {
        AudioSource audioSource = null;

        for (int i = 0; i < listAudioSource.Count; i++)
        {
            if (!listAudioSource [i].isPlaying && listAudioSource [i].time == 0)
            {
                audioSource = listAudioSource [i];
                break;
            }
        }

        if (audioSource == null)
        {
            audioSource = parent.AddComponent<AudioSource> ();
            listAudioSource.Add (audioSource);
        }

        return audioSource;
    }
}
