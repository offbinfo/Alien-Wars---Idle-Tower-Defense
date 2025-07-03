using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSongChallenge : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtNameSong;
    [SerializeField] private TMP_Text txtPrice;
    [SerializeField]
    private float priceBadeges;
    [SerializeField]
    private GameObject btnBuy;
    private TypeSong typeSong;

    public void SetUp(TypeSong typeSong , ThemeSongData themeSongData)
    {
        this.typeSong = typeSong;
        txtPrice.text = priceBadeges.ToString();
        txtNameSong.text = themeSongData.actor+ " - "+ LanguageManager.GetText(typeSong.ToString());

        CheckUnlockThemeSong();
    }

    public void Buy()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.BADGES, priceBadeges, OnBuySuccess);
    }

    private void OnBuySuccess(bool obj)
    {
       if(obj)
       {
            GameDatas.ThemeSongUnlock(typeSong);
            CheckUnlockThemeSong();
       }
    }

    private void CheckUnlockThemeSong()
    {
        if (typeSong == TypeSong.None) return;
        if (GameDatas.IsThemeSongUnlock(typeSong))
        {
            btnBuy.SetActive(false);
        }
        else
        {
            btnBuy.SetActive(true);
        }
    }
}
