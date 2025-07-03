using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TabUpgradeView : GameMonoBehaviour
{
    public UpgraderGroupID upgradeGroup;
    [SerializeField]
    private UpgraderElementUI upgradeElementUI;
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private GameObject infoText;
    public List<UpgraderElementUI> listUpgraderUI_ref => upgraderList;
    private List<UpgraderElementUI> upgraderList = new();

    float free_attack_upgrade => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.free_attack_upgrade);
    float free_defense_upgrade => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.free_deffense_upgrade);
    float free_resource_upgrade => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.free_resource_upgrade);

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnChangeCountBuyX, OnChangeCountBuyX);
        EventDispatcher.AddEvent(EventID.FinishWave, OnCheckUpgradeFree);
    }

    private void OnCheckUpgradeFree(object obj)
    {
        //upgrade free when finish wave

        int indexRand = Random.Range(0, upgraderList.Count);
        switch (upgradeGroup) 
        {
            case UpgraderGroupID.ATTACK:
                if(free_attack_upgrade.Chance())
                {
                    upgraderList[indexRand].UpgradeItem();
                }
                break; 
            case UpgraderGroupID.DEFENSE:
                if (free_defense_upgrade.Chance())
                {
                    upgraderList[indexRand].UpgradeItem();
                }
                break; 
            case UpgraderGroupID.RESOURCE:
                if (free_resource_upgrade.Chance())
                {
                    upgraderList[indexRand].UpgradeItem();
                }
                break;
            default:
                break;
        }
    }

    private void OnChangeCountBuyX(object obj)
    {
        foreach (var upgrader in upgraderList)
        {
            upgrader.OnChangePriceByCountBuyX();
        }
    }

    public void SetData(SO_UpgradeData upgradeData)
    {
        List<SO_UpgradeInforData> upgradeInforDatas = upgradeData.GetAllUpgradeInforTower();
        foreach (SO_UpgradeInforData upgradeInforData in upgradeInforDatas)
        {
            UpgraderElementUI cellView = Instantiate(upgradeElementUI, parent);
            UpgraderElementData cellData = new(upgradeInforData);
            cellView.SetData(cellData);
            upgraderList.Add(cellView);
        }
        CheckTabUpgradeEmpty();
    }

    private void CheckTabUpgradeEmpty()
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return;
            }
        }
        infoText.gameObject.SetActive(true);
    }

}
