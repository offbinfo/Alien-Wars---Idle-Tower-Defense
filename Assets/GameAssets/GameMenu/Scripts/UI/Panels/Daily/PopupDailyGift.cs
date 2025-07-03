using EasyUI.PickerWheelUI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDailyGift : UIPanel, IBoard
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private List<DayElement> dayElements;
    [SerializeField]
    private DailyRewardSO dailyRewardSO;
    [SerializeField]
    private List<AccumulateDailyReward> accumulateElements;

    [SerializeField]
    private WorldTimeAPI worldTimeAPI;
    [SerializeField]
    private Slider sliderReward;
    [SerializeField]
    private TMP_Text txtPoint;

    [SerializeField]
    private TMP_Text txtReward;
    private int currentDay;

    [SerializeField]
    private GameObject objClaimed;
    [SerializeField]
    private GameObject objClaimedUsingAds;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupDailyGift;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    private void SetSliderReward()
    {
        sliderReward.value = GameDatas.GetAccumulateDailyReward();
        txtPoint.text = GameDatas.GetAccumulateDailyReward().ToString();
    }

    private void Start()
    {
        sliderReward.maxValue = 30;
        BuildData();
        BuildDataPointWeekend();
        CheckAndUpdateDay(DateTime.Now);
        SetSliderReward();
        IsClaimedReward();

        EventDispatcher.AddEvent(EventID.OnRefreshDailyReward, OnRefreshDailyReward);
        txtReward.text = dayElements[currentDay - 1].amountReward.ToString();
    }

    private void OnRefreshDailyReward(object obj)
    {
        SetSliderReward();
    }

    public DateTime date;
    [Button("Test")]
    public void Test()
    {
        CheckAndUpdateDay(date);
    }

    private void CheckAndUpdateDay(DateTime dateTime)
    {
        string lastLogin = GameDatas.IsLastLoginDateDailyReward();
        currentDay = GameDatas.IsCurrentDayDailyReward();

        string today = dateTime.ToString("yyyy-MM-dd");

        if (lastLogin != today)
        {
            DebugCustom.LogColor(currentDay);
            currentDay++;

            if (currentDay > 35)
            {
                //reset quest
                GameDatas.SetAccumulateDailyGift(0);
                for (int i = 0; i < 36; i++)
                {
                    if (i % 5 == 0)
                    {
                        GameDatas.ClaimedAccumulateDailyGift(i, false);
                    }
                }
            }

            if (currentDay > 7)
            {
                currentDay = 1;

                foreach (DayElement dayElement in dayElements)
                {
                    dayElement.ResetDailyCalaimed();
                }
                GameDatas.XBonusGoldDailyQuest = 1;
            }

            GameDatas.LastLoginDateDailyReward(today);
            GameDatas.CurrentDayDailyReward(currentDay);
        }

        foreach (DayElement dayElement in dayElements)
        {
            dayElement.CheckIsClaimDay(currentDay >= dayElement.indexDay);
        }
        DebugCustom.Log("currentDay: " + currentDay);
    }

    private void BuildDataPointWeekend()
    {
        int[] keysArray = dailyRewardSO.accumulateDatas.Keys.ToArray();

        for (int i = 0; i < keysArray.Length; i++)
        {
            accumulateElements[i].SetData(keysArray[i]
                , dailyRewardSO.accumulateDatas[keysArray[i]], dailyRewardSO);
        }
    }

    public int BonusGoldByWorld(int amount)
    {
        return (int)(amount * dailyRewardSO.GetGoldBonusWorld((WorldType)GameDatas.GetHighestWorld()));
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private void Init()
    {
    }


    private void BuildData()
    {
        for(int i = 0;i < dailyRewardSO.dailyDatas.Count; i++)
        {
            DayCellData cellData = new(dailyRewardSO.dailyDatas[i], 
                dailyRewardSO.imgIcon, i + 1, dailyRewardSO);
            dayElements[i].SetData(cellData);
        }
    }

    private void IsClaimedReward()
    {
        if(dayElements[currentDay - 1].iconRewardSuccess.activeSelf)
        {
            objClaimed.SetActive(true);
            objClaimedUsingAds.SetActive(true);
        } else
        {
            objClaimed.SetActive(false);
            objClaimedUsingAds.SetActive(false);
        }
    }

    public void BtnClaimNow()
    {
        SetSliderReward();
        dayElements[currentDay - 1].OnClaim();
        IsClaimedReward();
        //if (!Extensions.HasInternet()) return;
        //StartCoroutine(worldTimeAPI.GetRealDateTimeFromAPI(OnGetTimeSuccess));
    }

    public void OnGetTimeSuccess()
    {
        CheckAndUpdateDay(worldTimeAPI.GetCurrentDateTime());
        DebugCustom.LogColor(worldTimeAPI.GetCurrentDateTime());
    }

    public void BtnClaimX2()
    {
        if (objClaimedUsingAds.activeSelf) return;
        WatchAds.WatchRewardedVideo(() => {
            SetSliderReward();
            dayElements[currentDay - 1].amountReward *= 2;
            dayElements[currentDay - 1].OnClaim();
            IsClaimedReward();
        }, "ClaimX2DailyGift");
        //if (!Extensions.HasInternet()) return;
        //StartCoroutine(worldTimeAPI.GetRealDateTimeFromAPI(OnGetTimeSuccess));
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}
