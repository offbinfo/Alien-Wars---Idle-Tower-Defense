using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUltimateWeapon : UIPanel, IBoard
{

    [SerializeField]
    private TabUltimateChoose tabUltimateChoose;
    [SerializeField]
    private TabUltimateInfor tabUltimateInfor;

    [SerializeField]
    private Transform content;
    [SerializeField]
    private UW_UpgradeElement uW_UpgradeElement;
    private List<UW_UpgradeElement> upgradeUWElements = new();

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupUltimateWeapon;
    }
    private void OnEnable()
    {
        OnAppear();
        OnRefresh();
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

    public void BuildData()
    {
        foreach(SO_UW_Base upgradeUW in Cfg.UWCtrl.UWeaponManager.ultimateWeapons)
        {
            UW_UpgradeElement element = Instantiate(uW_UpgradeElement, content);
            UW_UpgradeCellData cellData = new(upgradeUW);
            element.SetData(cellData);
            upgradeUWElements.Add(element);
        }
        OnRefresh();
    }

    private void OnRefresh()
    {
        if (upgradeUWElements.Count == 0) return;
        foreach (var upgradeUW in upgradeUWElements)
        {
            upgradeUW.CheckUnlock();
        }
    }

    private void OnDestroy()
    {
    }

    private void OnOpenUltimate()
    {
        tabUltimateChoose.gameObject.SetActive(true);   
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
