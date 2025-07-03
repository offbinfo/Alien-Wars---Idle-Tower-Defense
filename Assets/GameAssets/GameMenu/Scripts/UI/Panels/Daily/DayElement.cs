using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DayElement : BaseUICellView
{
    [SerializeField]
    private Image iconDay;
    [SerializeField]
    private GameObject objQuestion;
    [SerializeField]
    private GameObject objReceived;
    [SerializeField]
    private GameObject iconReward;
    public GameObject iconRewardSuccess;
    [SerializeField]
    private TMP_Text txtDay;
    [SerializeField]
    private TMP_Text txtAmount;
    public int indexDay;
    public int amountReward;

    private DayCellData cellData;

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);

        cellData = data as DayCellData;
        if(cellData.Data.currencyType == CurrencyType.GOLD)
        {
            iconDay.sprite = cellData.icon;
        }
        indexDay = cellData.index;

        txtDay.text = LanguageManager.GetText("day") + " "+indexDay;
        //objQuestion.SetActive(true);
        amountReward = cellData.Data.goldRewrad;
        txtAmount.text = Extensions.FormatNumber(cellData.Data.goldRewrad);

        CheckReceived();
    }

    public void CheckIsClaimDay(bool isClaim)
    {
        //iconReward.SetActive(isClaim);
        objReceived.SetActive(isClaim);
        //objQuestion.SetActive(!isClaim);
    }

    public void ResetDailyCalaimed()
    {
        GameDatas.DayDailyReceived(indexDay, 0);
        CheckReceived();
    }

    public void CheckReceived()
    {
        bool isReceived = GameDatas.IsDayDailyReceived(indexDay);

        //iconReward.SetActive(isReceived);
        objReceived.SetActive(isReceived);
        iconRewardSuccess.SetActive(isReceived);
        //objQuestion.SetActive(!isReceived);
    }

    public int BonusGoldByWorld(int amount)
    {
        return (int)(amount * cellData.DailyRewardSO.GetGoldBonusWorld((WorldType)GameDatas.GetHighestWorld()));
    }

    public void OnClaim()
    {
        if(iconRewardSuccess.activeSelf) return;

        GameDatas.DayDailyReceived(indexDay, 1);
        GameDatas.SetAccumulateDailyReward(GameDatas.GetAccumulateDailyReward() + 1);

        TopUI_Currency topUI_Currency = TopUI_Currency.instance;

        GameDatas.Gold += BonusGoldByWorld(amountReward);
        GameAnalytics.LogEvent_EarnGold("daily_gift", BonusGoldByWorld(amountReward));

        Transform posGoldEnd = topUI_Currency.goldIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posGoldEnd.position, CurrencyType.GOLD);

        CheckReceived();
    }

}
