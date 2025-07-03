using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemMusicChange : MonoBehaviour
{
    [SerializeField]
    private GameObject lockItem;
    [SerializeField]
    private GameObject selected;
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TypeSong typeSong;
    [SerializeField]
    private AudioSource audioSource;
    public bool isPlay;
    public Action<GameObject> OnSelected;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CheckUnlockThemeSong();
    }

    public void SetUp(TypeSong typeSong, ThemeSongData themeSongData)
    {
        this.typeSong = typeSong;
        txtName.text = themeSongData.actor + " - " + LanguageManager.GetText(typeSong.ToString());

        CheckUnlockThemeSong();

        if(GameDatas.IsThemeSongUsing() == (int)typeSong)
        {
            OnSelected?.Invoke(selected);
        }
    }

    private void CheckUnlockThemeSong()
    {
        if (typeSong == TypeSong.None) return;
        if (GameDatas.IsThemeSongUnlock(typeSong))
        {
            lockItem.SetActive(false);
        }
        else
        {
            lockItem.SetActive(true);
        }

        if (typeSong == TypeSong.DefaultMusic)
        {
            lockItem.SetActive(false);
        }
    }

    public void Selected()
    {
        GameDatas.ThemeSongUsing(typeSong);
        OnSelected?.Invoke(selected);
    }

    public void PlayMusic()
    {
        isPlay = !isPlay;
        if (isPlay)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
