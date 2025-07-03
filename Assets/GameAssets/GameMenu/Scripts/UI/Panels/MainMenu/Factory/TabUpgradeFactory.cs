using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUpgradeFactory : GameMonoBehaviour
{
    public UpgraderGroupID upgradeGroup;
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private GameObject infoText;
    [SerializeField]
    private UpgraderFactoryElement upgraderFactoryCellView;
    [SerializeField]
    private UpgraderClusterUpgradeTab upgraderClusterUpgrade;
    [SerializeField]
    private List<UpgraderFactoryElement> elementUpgraders;
    [SerializeField] 
    private GridLayoutGroup gridLayout;

    public List<UpgraderFactoryElement> ElementUpgraders { get => elementUpgraders; }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnBuyClusterUpgrader, OnRefreshCLusterUpgrader);
        EventDispatcher.AddEvent(EventID.OnGoldChanged, OnRefreshUI);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, OnUpgradeSubjectSuccessLab);
        EventDispatcher.AddEvent(EventID.OnEquipCard, OnUpgradeSubjectSuccessLab);
        EventDispatcher.AddEvent(EventID.OnUnequipcard, OnUpgradeSubjectSuccessLab);
        EventDispatcher.AddEvent(EventID.OnChangeCountBuyX, OnChangeCountBuyX);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, OnUpgradeSubjectSuccessLab);
    }

    private void OnChangeCountBuyX(object obj)
    {
        foreach (var upgrader in elementUpgraders)
        {
            upgrader.OnChangePriceByCountBuyX();
        }
    }

    private void OnUpgradeSubjectSuccessLab(object obj)
    {
        foreach (var upgrader in elementUpgraders)
        {
            upgrader.OnResferhData();
        }
    }

    private void OnRefreshUI(object obj)
    {
        foreach (var upgrader in elementUpgraders)
        {
            upgrader.OnSilverChanged();
        }
    }

    private void OnRefreshCLusterUpgrader(object o)
    {
        foreach(var upgrader in elementUpgraders)
        {
            upgrader.CheckUnlockItemUpgrade();
        }
    }

    public void SetData(SO_UpgradeData upgradeData)
    {
        List<SO_UpgradeInforData> upgradeInforDatas = upgradeData.GetAllUpgradeInforTower();
        foreach (SO_UpgradeInforData upgradeInforData in upgradeInforDatas)
        {
            UpgraderFactoryElement cellView = Instantiate(upgraderFactoryCellView, parent);
            UpgraderFactoryCellData cellData = new(upgradeInforData);
            cellView.SetData(cellData);
            elementUpgraders.Add(cellView);
            TabUpgraderManager.instance.upgraderFactoryElements.Add(cellView);
        }
        upgraderClusterUpgrade.SetData(upgradeData, upgradeGroup);
    }

    public void ExtentGrid(bool isExtentGrid)
    {
        gridLayout.padding.bottom = isExtentGrid ? 300 : 0;
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnBuyClusterUpgrader, OnRefreshCLusterUpgrader);
        EventDispatcher.RemoveEvent(EventID.OnGoldChanged, OnRefreshUI);
    }
}
