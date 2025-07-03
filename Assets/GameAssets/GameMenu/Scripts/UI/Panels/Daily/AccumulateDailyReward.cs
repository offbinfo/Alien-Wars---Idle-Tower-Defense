using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccumulateDailyReward : MonoBehaviour
{
    public int index;
    private AccumulateData cellData;
    private DailyRewardSO dailyRewardSO;
    [SerializeField]
    private GameObject objClaimed;
    [SerializeField]
    private TMP_Text txtPointReward;
    [SerializeField]
    private GameObject objBlur;

    [SerializeField]
    private TMP_Text txtGoldBonus;
    [SerializeField]
    private TMP_Text txtGem;
    [SerializeField]
    private TMP_Text txtPowerstone;
    [SerializeField]
    private GameObject objInfor;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshDailyReward, OnRefreshDailyReward);
    }

    private void OnRefreshDailyReward(object obj)
    {
        CheckCLaimed();
    }

    public void SetData(int index, AccumulateData data, DailyRewardSO dailyRewardSO)
    {
        this.index = index;
        cellData = data;
        this.dailyRewardSO = dailyRewardSO;

        txtGoldBonus.text = Extensions.FormatNumber(BonusGoldByWorld(cellData.gold)).ToString();    
        txtGem.text = BonusGoldByWorld(cellData.gem).ToString();    
        txtPowerstone.text = BonusGoldByWorld(cellData.powerstone).ToString();    
        ResetAccumulateClaim();
        CheckCLaimed();

        txtPointReward.text = index.ToString();
    }

    private void CheckCLaimed()
    {
        objClaimed.SetActive(GameDatas.IsClaimedAccumulateDailyReward(index));
        if (GameDatas.IsClaimedAccumulateDailyReward(index))
        {
            objBlur.SetActive(false);
        }
        if (index <= GameDatas.GetAccumulateDailyReward())
        {
            objBlur.SetActive(true);
        }
        else
        {
            objBlur.SetActive(false);
        }
    }

    private void ActiveAnimFlyCurrency()
    {
        TopUI_Currency topUI_Currency = TopUI_Currency.instance;

        GameDatas.Gold += BonusGoldByWorld(cellData.gold);
        GameDatas.Gem += BonusGoldByWorld(cellData.gem);
        GameDatas.PowerStone += BonusGoldByWorld(cellData.powerstone);

        Transform posGoldEnd = topUI_Currency.goldIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posGoldEnd.position, CurrencyType.GOLD);

        Transform posGemEnd = topUI_Currency.gemIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posGemEnd.position, CurrencyType.GEM);

        Transform posPowerEnd = topUI_Currency.powerStoneIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posPowerEnd.position, CurrencyType.POWER_STONE);
    }

    private void ResetAccumulateClaim()
    {
        if(GameDatas.GetAccumulateDailyReward() == 0)
        {
            GameDatas.ClaimedAccumulateDailyReward(index, false);
        }
    }

    public int BonusGoldByWorld(int amount)
    {
        return (int)(amount * dailyRewardSO.GetGoldBonusWorld((WorldType)GameDatas.GetHighestWorld()));
    }

    public void OnClaim()
    {
        StartCoroutine(DelayOpenInfor());

        if (GameDatas.IsClaimedAccumulateDailyReward(index)) return;
        if (index > GameDatas.GetAccumulateDailyReward()) return;
        ActiveAnimFlyCurrency();
        GameDatas.ClaimedAccumulateDailyReward(index, true);

        CheckCLaimed();
    }

    private IEnumerator DelayOpenInfor()
    {
        objInfor.gameObject.SetActive(true);
        yield return Yielders.Get(0.6f);
        objInfor.gameObject.SetActive(false);
    }
}
