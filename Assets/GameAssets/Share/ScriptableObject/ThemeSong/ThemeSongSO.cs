using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeSongSO", menuName = "Data/ThemeSong/ThemeSongSO", order = 0)]
public class ThemeSongSO : SerializedScriptableObject
{

    public Dictionary<TypeSong, ThemeSongData> themeSongDicts = new();

    public ThemeSongData GetThemeSong(TypeSong type)
    {
        return themeSongDicts[type];    
    }
}

[Serializable]
public class ThemeSongData
{
    public string actor;
    public string name;
    public AudioClip audioClip;
}
