using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UW_PropertyElementUI : GameMonoBehaviour
{

    [SerializeField] TMP_Text txt_title;
    [SerializeField] TMP_Text txt_stat;
    [SerializeField] TMP_Text txt_price;
    private int index = -1;
    private SO_UW_Base data;
    private int price;
    private UW_ID id;
    private bool isDuration;
    [SerializeField]
    private GameObject btnUnBuy;

    public void SetData(SO_UW_Base data, int index)
    {
        this.data = data;
        this.index = index;
        id = data.id;
        Refresh();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, OnUpgradeSubjectSuccessLab);
    }

    private void OnEnable()
    {
        CheckUpgrade();
    }

    private void CheckUpgrade()
    {
        if (data == null)
            return;
        price = data.GetPrice()[index];
        btnUnBuy.SetActive(price > GameDatas.PowerStone);
    }

    private void OnUpgradeSubjectSuccessLab(object obj)
    {
        Refresh();
    }

    private void Refresh()
    {

        if (data == null)
            return;
        txt_title.text = data.statName[index];

        float property =float.Parse(data.GetStatProperty()[index].ToString());

        if (isDuration)
        {
            if (id == UW_ID.GOLDEN_SANCTUARY)
            {
                property += Cfg.labCtrl.
                    LapManager.GetSingleSubjectById(IdSubjectType.GOLDEN_SANCTUARY_DURATION).GetCurrentProperty();
            }
            else if (id == UW_ID.ANTI_FORCE)
            {
                property += Cfg.labCtrl.
                    LapManager.GetSingleSubjectById(IdSubjectType.ANTI_FORCE_DURATION).GetCurrentProperty();
            }
        }

        txt_stat.text = data.GetFormat[index] == "s"
            ? property.ToString() + data.GetFormat[index]
            : data.GetFormat[index] + property.ToString();
        isDuration = data.GetFormat[index] == "s";

        price = data.GetPrice()[index];
        txt_price.text = price + "<sprite name=powerstone>";

        if (data.Level[index] >= data.LevelMax[index])
            txt_price.text = "Level Max";

        CheckUpgrade();
    }

    public void BtnBuy()
    {
        if (data.Level[index] >= data.LevelMax[index])
            return;
        GameDatas.BuyUsingCurrency(CurrencyType.POWER_STONE, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool iSuccess)
    {
        if (iSuccess)
        {
            data.GetActionUpgrade()[index]?.Invoke();
            Refresh();
            QuestEventManager.Upgrade(1);
        }
    }
}
