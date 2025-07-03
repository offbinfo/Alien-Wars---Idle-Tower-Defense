using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupBonusGold : UIPanel, IBoard
{
    [SerializeField] TMP_Text txt_status;
    [SerializeField] private TMP_Text txtTimeBonus;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupBonusGold;
    }
    private void OnEnable()
    {
        OnAppear();
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
        var target = GameDatas.timeTargetBonusGold;
        txt_status.text = (target > DateTime.Now) ? LanguageManager.GetText("active") : LanguageManager.GetText("inactive");
        txtTimeBonus.text = "+" + GameDatas.TimeBonusGold + " MIN";
    }


    public void BtnClaimNow()
    {
        //+20 min vào 
        WatchAds.WatchRewardedVideo(() =>
        {
            var present = GameDatas.timeTargetBonusGold;
            if (present > DateTime.Now)
            {
                GameDatas.timeTargetBonusGold.AddMinutes(GameDatas.TimeBonusGold);
            }
            else
            {
                GameDatas.timeTargetBonusGold = DateTime.Now.AddMinutes(GameDatas.TimeBonusGold);
            }
            GameDatas.TimeBonusGold += 15;
            if (GameDatas.TimeBonusGold > 120)
            {
                GameDatas.TimeBonusGold = 120;
            }

            EventDispatcher.PostEvent(EventID.OnAddTimeBonusGold, null);
            Close();
        }, "BonusGold");
    }

    public override void Close()
    {
        base.Close();
        TimeGame.Pause = false;
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
