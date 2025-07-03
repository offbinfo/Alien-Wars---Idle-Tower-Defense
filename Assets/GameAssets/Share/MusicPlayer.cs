using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    [SerializedDictionary("TypeMusic", "Music")]
    private SerializedDictionary<TypeSong, AudioPackClip> musicPacks;

    void Start()
    {
        OnChangeSongTheme(null);
        OnLoopSong(null);
        EventDispatcher.AddEvent(EventID.OnChangeSongTheme, OnChangeSongTheme);
        EventDispatcher.AddEvent(EventID.OnLoopSong, OnLoopSong);
    }

    private void OnLoopSong(object obj)
    {
        foreach (var kvp in musicPacks)
        {
            TypeSong type = kvp.Key;
            AudioPackClip audioClip = kvp.Value;

            audioClip.loop = GameDatas.IsLoopSong == 1 ? true : false;
        }
        OnChangeSongTheme(null);
    }

    private void OnChangeSongTheme(object obj)
    {
        AudioPackClip audioPackClip = musicPacks[(TypeSong)GameDatas.IsThemeSongUsing()];
        AudioManager.PlayMusicStatic(audioPackClip.name);
    }

    private void OnDisable()
    {
    }
}
