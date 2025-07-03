using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeekendQuestElement : MonoBehaviour
{
    public int indexWeekend;
    private RewardDailyQuestWeekend cellData;
    [SerializeField]
    private GameObject objClaimed;
    [SerializeField]
    private TMP_Text txtPoint;
    [SerializeField]
    private GameObject objBlur;

    [SerializeField]
    private TMP_Text txtGoldBonus;
    [SerializeField]
    private TMP_Text txtGem;
    [SerializeField]
    private GameObject objInfor;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshDailyGift, OnRefreshDailyGift);
    }

    private void OnRefreshDailyGift(object obj)
    {
        CheckCLaimed();
    }

    public void SetData(int indexWeekend, RewardDailyQuestWeekend data)
    {
        this.indexWeekend = indexWeekend;
        cellData = data;

        txtPoint.text = indexWeekend.ToString();
        CheckCLaimed();

        txtGoldBonus.text = cellData.amountGold.ToString();
        txtGem.text = cellData.amountGem.ToString();
    }

    private void CheckCLaimed()
    {
        objClaimed.SetActive(GameDatas.IsClaimedAccumulateDailyGift(indexWeekend));
        if(GameDatas.IsClaimedAccumulateDailyGift(indexWeekend))
        {
            objBlur.SetActive(false);
        }
        if (indexWeekend <= GameDatas.GetAccumulateDailyGift()) {
            objBlur.SetActive(true);
        } else
        {
            objBlur.SetActive(false);
        }
    }

    private void HandlePowerStone()
    {
        int amount = cellData.amountPowerstone;
        GameDatas.PowerStone += amount;
        ActiveAnimFlyCurrency(CurrencyType.POWER_STONE);
    }
    private void HandleGem()
    {
        int amount = cellData.amountGem;
        GameDatas.Gem += amount/* * GameDatas.XBonusGoldDailyQuest)*/;
        ActiveAnimFlyCurrency(CurrencyType.GEM);
    }

    private void ActiveAnimFlyCurrency(CurrencyType currency)
    {
        Transform posEnd = null;
        TopUI_Currency topUI_Currency = TopUI_Currency.instance;
        switch (currency)
        {
            case CurrencyType.GOLD:
                posEnd = topUI_Currency.goldIcon.transform;
                break;
            case CurrencyType.GEM:
                posEnd = topUI_Currency.gemIcon.transform;
                break;
            case CurrencyType.POWER_STONE:
                posEnd = topUI_Currency.powerStoneIcon.transform;
                break;
            default:
                break;
        }
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posEnd.position, currency);
    }

    public void OnClaim()
    {
        StartCoroutine(DelayOpenInfor());

        if (GameDatas.IsClaimedAccumulateDailyGift(indexWeekend)) return;
        if (indexWeekend > GameDatas.GetAccumulateDailyGift()) return;

        HandlePowerStone();
        HandleGem();
        GameDatas.XBonusGoldDailyQuest = (int)cellData.amountGold;
        GameDatas.ClaimedAccumulateDailyGift(indexWeekend, true);

        CheckCLaimed();
    }

    private IEnumerator DelayOpenInfor()
    {
        objInfor.gameObject.SetActive(true);
        yield return Yielders.Get(0.6f);
        objInfor.gameObject.SetActive(false);
    }
}
