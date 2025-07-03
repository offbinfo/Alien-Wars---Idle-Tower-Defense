using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MaxSdkCallbacks;

public class PopupBannerReward : UIPanel, IBoard
{
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private TMP_Text txtNamePack;
    [SerializeField]
    private TypeBannerPack TypeBannerPack;
    [SerializeField] private BannerPackUI[] bannerStonePack = new BannerPackUI[2];
    [SerializeField] private BannerPackUI[] bannerBeginnerPack = new BannerPackUI[2];

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupBannerReward;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    private void Start()
    {
        BuildData();
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
        DateTime now = DateTime.UtcNow;

        if ((now - GameDatas.FirstOpenDate).TotalDays < 3)
        {
            bool showStone = GameDatas.LastBannerCycle % 2 == 0;
            TypeBannerPack = showStone ? TypeBannerPack.StonePack : TypeBannerPack.BeginnerPack;
            txtNamePack.text = TypeBannerPack.ToString();
            ShowBannerPack(showStone);
        }
    }

    private void Update()
    {
        UpdateBannerIconAndTimer(DateTime.UtcNow, GameDatas.FirstOpenDate);
    }

    private void UpdateBannerIconAndTimer(DateTime now, DateTime firstOpen)
    {
        double daysRemaining = 3 - (now - firstOpen).TotalDays;

        if (daysRemaining > 0)
        {
            TimeSpan timeLeft = firstOpen.AddDays(3) - now;
            txtTime.text = LanguageManager.GetText("ends_in_") + $"<color=#FFFF00> {timeLeft.Days}d.{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2} </color>";
        }
    }

    private void ShowBannerPack(bool showStone)
    {
        foreach (var banner in bannerStonePack)
            banner.gameObject.SetActive(showStone);
        foreach (var banner in bannerBeginnerPack)
            banner.gameObject.SetActive(!showStone);

        if (TypeBannerPack == TypeBannerPack.BeginnerPack)
        {
            //check Show beginnerPack1
            if (GameDatas.GetHighestWorld() > 0 || (GameDatas.countUpgraderTime >= 2 || GameDatas.countLoseGame >= 2))
            {
                bannerBeginnerPack[0].gameObject.SetActive(true);
                bannerBeginnerPack[1].gameObject.SetActive(false);
            }
            //check Show beginnerPack2
            if (GameDatas.GetHighestWorld() > 0 || (GameDatas.countDoneQuest >= 1 && GameDatas.countLoseGame >= 5))
            {
                bannerBeginnerPack[0].gameObject.SetActive(true);
                bannerBeginnerPack[1].gameObject.SetActive(false);
            }
        }
    }

    public void BtnResumeRound()
    {
        Gm.PlayGame(true);
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
