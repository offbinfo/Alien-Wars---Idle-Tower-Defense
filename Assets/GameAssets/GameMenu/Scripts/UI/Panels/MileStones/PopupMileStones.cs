using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMileStones : UIPanel, IBoard
{

    [SerializeField] SO_MileStoneManager dataMilestone;
    private int tierIndex = -1;
    [SerializeField]
    private TabTierCellView tabTierCellView;
    [SerializeField]
    private Transform content;
    private List<TabTierCellView> tabTierCells = new();
    [SerializeField]
    private TMP_Text txtTier;

    [SerializeField]
    private GameObject lockNextTier;
    [SerializeField]
    private GameObject lockPreTier;
    [SerializeField]
    private GameObject obj_BuypremiumPass;

    [SerializeField]
    private GameObject objClaimAll;
    [SerializeField]
    private GameObject btnUnBuyPremium;

    public int TierIndex
    {
        get { return tierIndex; }
        set
        {
            if (tierIndex != value)
            {
                tierIndex = value;
                txtTier.text = LanguageManager.GetText("world") + " " + (tierIndex + 1).ToString();
                ActiveTabTier();
            }
        }
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupMileStones;
    }
    private void OnEnable()
    {
        OnAppear();
        CheckNextTierByCurrentWorld();
        CheckClaimAll();

        //TierIndex = GameDatas.GetHighestWorld();
    }

    private void CheckClaimAll()
    {

    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefeshUIHome, OnRefeshUI);
        BuildData();
        EventDispatcher.AddEvent(EventID.OnUsingPremiumMileStones, OnBuyPremiumPass);
        OnBuyPremiumPass(null);
    }

    private void OnBuyPremiumPass(object obj)
    {
        btnUnBuyPremium.SetActive(GameDatas.IsUsingPremiumMileStones);
    }

    private void OnRefeshUI(object o)
    {
        CheckNextTierByCurrentWorld();
        CheckNextOrPreTier();
    }

    private void BuildData()
    {
        if (dataMilestone == null) return;

        List<SO_Milestone> milestones = dataMilestone.GetMilestonesValuesAsList();
        for (int i = 0; i < milestones.Count; i++)
        {
            TabTierCellView tabTier = Instantiate(tabTierCellView, content);
            tabTier.SetData(milestones[i].milestonesReward, milestones[i].typeWorld);
            tabTierCells.Add(tabTier);
        }
        TierIndex = 0;
        CheckNextTierByCurrentWorld();
    }

    public void OnClickBtnMilestone()
    {
        obj_BuypremiumPass.SetActive(true);
    }

    public void OnClaimAllReward()
    {
        EventDispatcher.PostEvent(EventID.OnClaimAllItemReward, 0);
    }

    public void NextTabTier()
    {
        if (lockNextTier.activeSelf) return;
        if (TierIndex < GameDatas.GetHighestWorld())
        {
            TierIndex++;
        }
    }

    public void PreviosTabTier()
    {
        if (lockPreTier.activeSelf) return;
        if (TierIndex > 0)
        {
            TierIndex--;
        }
    }

    private void CheckNextTierByCurrentWorld()
    {
        lockNextTier.SetActive(GameDatas.GetHighestWorld() <= TierIndex);
    }

    public void ActiveTabTier()
    {
        if(tierIndex > tabTierCells.Count - 1) return;
        CheckNextOrPreTier();

        foreach (var tabTier in tabTierCells) tabTier.gameObject.SetActive(false);
        tabTierCells[tierIndex].gameObject.SetActive(true);
    }

    private void CheckNextOrPreTier()
    {
        lockPreTier.SetActive(false);
        lockNextTier.SetActive(false);

        if (TierIndex == 0)
        {
            lockPreTier.SetActive(true);
        }
        else if (TierIndex == GameDatas.GetHighestWorld())
        {
            lockNextTier.SetActive(true);
        }
        else
        {
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshUIHome, OnRefeshUI);
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

    public void BtnClaimNow()
    {

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
