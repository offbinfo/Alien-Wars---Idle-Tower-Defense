using language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeginnerQuestElement : BaseUICellView
{
    [SerializeField]
    private TMP_Text txtNameQuest;
    [SerializeField]
    private TMP_Text txtGold;
    [SerializeField]
    private TMP_Text txtGem;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text txtCountQuest;
    private BeginnerQuestCellData cellData;

    [SerializeField]
    private GameObject ObjClaim;
    [SerializeField]
    private GameObject ObjClaimed;
    [SerializeField]
    private GameObject ObjUnClaimed;
    private bool isClaimed;
    private bool isUnlockQuest;
    private DailyQuestType dailyQuestType;
    private DailyQuestSpecialType specialType;

    private int amountGold;
    [SerializeField]
    private GameObject objSliderQuest;
    private SO_DailyQuestManager sO_DailyQuestManager;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshClaimDailyGift, OnRefreshClaimDailyGift);
        EventDispatcher.AddEvent(EventID.XBonusGoldDailyQuest, OnRefreshDailyGift);
    }

    private void OnEnable()
    {
        if (cellData != null)
            OnRefreshDailyGift(null);
    }

    private void OnRefreshDailyGift(object obj)
    {
        if(cellData == null) return;
        amountGold = cellData.sO_DailyQuest.GetRewardDailyQuestByWorld((WorldType)GameDatas.GetHighestWorld()).amountGold
            * GameDatas.XBonusGoldDailyQuest;

        txtNameQuest.text = string.Format(LanguageManager.GetText(SplitCamelCase(cellData.dailyQuestType.ToString())), cellData.countQuest);

        txtGold.text = Extensions.FormatNumber(amountGold);
        txtGem.text = cellData.amountReward.amountGem.ToString();

        dailyQuestType = cellData.dailyQuestType;
        specialType = cellData.specialType;

        ActiveCountQuest(!(cellData.countQuest == 0));

        slider.maxValue = cellData.countQuest;
        ChangeValueSliderQuest();

        IsUnlockQuestReward();
    }

    private void OnRefreshClaimDailyGift(object obj)
    {
        DailyQuestType type = (DailyQuestType)obj;

        if(type == dailyQuestType)
        {
            ChangeValueSliderQuest();
        }
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);

        cellData = data as BeginnerQuestCellData;
        this.sO_DailyQuestManager = cellData.sO_DailyQuest;

        OnRefreshDailyGift(null);
    }
    private string SplitCamelCase(string text)
    {
        return Regex.Replace(text, "(?<!^)([A-Z])", " $1");
    }


    private void ChangeValueSliderQuest()
    {
        int totalQuestDone = GameDatas.GetTotalQuestDone(dailyQuestType);
        slider.value = totalQuestDone;
        txtCountQuest.text = $"{totalQuestDone}/{cellData.countQuest}";

        if(totalQuestDone >= cellData.countQuest)
        {
            isUnlockQuest = true;
            ObjUnClaimed.SetActive(false);
        }
        else
        {
            ObjUnClaimed.SetActive(true);
        }
    }

    private void ActiveCountQuest(bool isActive)
    {
        objSliderQuest.SetActive(isActive);
    }

    public void OnClaim()
    {
        if (!isUnlockQuest) return;
        if (ObjClaimed.activeSelf) return;

        GameDatas.ClaimedQuestReward(cellData.dailyQuestType, true);

        TopUI_Currency topUI_Currency = TopUI_Currency.instance;

        GameDatas.Gold += (amountGold * GameDatas.XBonusGoldDailyQuest);
        GameDatas.Gem += cellData.amountReward.amountGem;

        Transform posGoldEnd = topUI_Currency.goldIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posGoldEnd.position, CurrencyType.GOLD);

        Transform posGemEnd = topUI_Currency.gemIcon.transform;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posGemEnd.position, CurrencyType.GEM);

        IsUnlockQuestReward();

        GameDatas.SetAccumulateDailyGift(GameDatas.GetAccumulateDailyGift() + 1);
        EventChallengeListenerManager.CompleteDailyMissions(1);
    }

    private void IsUnlockQuestReward()
    {
        if(GameDatas.IsClaimedQuestReward(cellData.dailyQuestType)) {
            ObjClaim.SetActive(false);
            ObjClaimed.SetActive(true);
        } else
        {
            ObjClaim.SetActive(true);
            ObjClaimed.SetActive(false);
        }
    }
}
