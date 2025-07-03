using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class PopupClaimReward : UIPanel, IBoard
{
    [SerializeField]
    private RewardTimeHome rewardTimeHome;
    [SerializeField]
    private RewardTimeHome rewardHomeMain;
    [SerializeField]
    private GameObject btnUnClaim;
    [SerializeField]
    private GameObject btnUnClaimx2;
    private int purchaseCount;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupClaimReward;
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
        string lastPurchaseDate = GameDatas.GetLastClaimdX2OfflineRewardDate();

        if (lastPurchaseDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            purchaseCount = 0;
            GameDatas.SetClaimedX2OfflineReward(purchaseCount);
            GameDatas.SetLastClaimdX2OfflineRewardDate();
        }

        CheckClaim();
    }

    private void Update()
    {
        CheckClaim();
    }

    private void CheckClaim()
    {
        int seconds = GameDatas.SecondsAccumulate;
        bool hasReward = rewardTimeHome.GetRewardGold(seconds) > 0 ||
                 rewardTimeHome.GetRewardGem(seconds) > 0 ||
                 rewardTimeHome.GetRewardPowerStone(seconds) > 0;

        ActibeBtnClaimed(!hasReward);
        btnUnClaimx2.SetActive(GameDatas.GetClaimedX2OfflineReward() >= 2 || !hasReward);
    }

    private void ActibeBtnClaimed(bool isClaim)
    {
        btnUnClaim.SetActive(isClaim);
        btnUnClaimx2.SetActive(isClaim);
    }

     public void BtnClaimNow()
    {
        rewardTimeHome.OnClaimReward();
        rewardHomeMain.ResetReward();
        CheckClaim();
    }

    public void BtnClaimX2()
    {
        purchaseCount = GameDatas.GetClaimedX2OfflineReward();
        string lastPurchaseDate = GameDatas.GetLastClaimdX2OfflineRewardDate();

        if (lastPurchaseDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            purchaseCount = 0;
            GameDatas.SetLastClaimdX2OfflineRewardDate();
        }

        if (purchaseCount >= 2)
        {
            CheckClaim();
            DebugCustom.LogColor("Bạn chỉ có thể mua tối đa 2 lần mỗi ngày.");
            return;
        }

        if (purchaseCount >= 2) return;
        WatchAds.WatchRewardedVideo(() => {
            purchaseCount++;
            GameDatas.SetClaimedX2OfflineReward(purchaseCount);

            rewardTimeHome.OnClaimReward(true);
            rewardHomeMain.ResetReward();
            CheckClaim();
        }, "Claim Reward X2");
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
