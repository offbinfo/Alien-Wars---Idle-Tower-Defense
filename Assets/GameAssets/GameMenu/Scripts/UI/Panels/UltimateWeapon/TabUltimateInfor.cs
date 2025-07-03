using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabUltimateInfor : GameMonoBehaviour
{
    [SerializeField] TMP_Text txt_title;
    [SerializeField] TMP_Text txt_description;
    [SerializeField] TMP_Text txt_stat0;
    [SerializeField] TMP_Text txt_info0;
    [SerializeField] TMP_Text txt_stat1;
    [SerializeField] TMP_Text txt_info1;
    [SerializeField] TMP_Text txt_stat2;
    [SerializeField] TMP_Text txt_info2;
    [SerializeField] TMP_Text txt_price;

    [SerializeField]
    private GameObject btnUnBuy;

    [SerializeField] Image icon;

    private SO_UW_Base _data;
    private int price;

    private void OnEnable()
    {
        SetPriceUW();
    }

    public void SetData(SO_UW_Base data)
    {
        _data = data;
        gameObject.SetActive(true);

        txt_title.text = data.Name;
        txt_description.text = data.Description;
        var names = data.statName;
        var properties = data.GetStatProperty();
        txt_stat0.text = names[0] + ":";
        txt_info0.text = properties[0].ToString();
        txt_stat1.text = names[1] + ":";
        txt_info1.text = properties[1].ToString();
        txt_stat2.text = names[2] + ":";
        txt_info2.text = properties[2].ToString();

        icon.sprite = data.iconUW;
        icon.SetNativeSize();

        SetPriceUW();
    }

    private void SetPriceUW()
    {
        var price = Cfg.UWCtrl.GetPriceUnlockUW();
        this.price = price;
        txt_price.text = string.Format("{0} <sprite name=powerstone> " + LanguageManager.GetText("unlock"), price);

        btnUnBuy.SetActive(price > GameDatas.PowerStone);
    }

    public void CLoseTab()
    {
        gameObject.SetActive(false);
    }

    public void OnBuy()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.POWER_STONE, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool success)
    {
        if (success)
        {
            _data.UnlockThisUW();
            gameObject.SetActive(false);
            int slot = GameDatas.IsUnlockSLotUltimateWeapon() + 1;
            GameDatas.UnlockSLotUltimateWeapon(slot);

            LogEventLongToUnlockUltimateWeapon(_data.Name);
        }
    }

    //log event unlock UW
    public static void LogEventLongToUnlockUltimateWeapon(string nameUW)
    {
        DateTime startTime = DateTime.Parse(GameDatas.StartTimeFirstInGame);
        DateTime now = DateTime.UtcNow;
        TimeSpan elapsedTime = now - startTime;

        GameAnalytics.LogEventLongToUnlockUltimateWeapon(elapsedTime, nameUW);
    }
}
