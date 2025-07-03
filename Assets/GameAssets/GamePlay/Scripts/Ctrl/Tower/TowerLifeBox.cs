using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLifeBox : GameMonoBehaviour
{

    float lifebox_hp_amount => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.lifebox_hp_amount)
        + (ConfigManager.instance.labCtrl.LapManager.
                    GetSingleSubjectById(IdSubjectType.LIFEBOX_HP_AMOUNT).GetCurrentProperty() / 100);
    float lifebox_max_hp => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.lifebox_max_hp) +
        (ConfigManager.instance.labCtrl.LapManager.
                    GetSingleSubjectById(IdSubjectType.LIFEBOX_MAX_HP).GetCurrentProperty() / 100);

    public float CurHpLifeBox { 
        get => curHpLifeBox; 
        set {
            curHpLifeBox = value;
            UpdateUI(curHpLifeBox);
        } 
    }

    private TowerData towerData;
    [SerializeField]
    private Transform main;
    private Object_DataInformation _objData;

    private float curHpLifeBox;

    private void Awake()
    {
        towerData = GetComponent<TowerData>();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnClaimItemLifeBoxDrop, OnClaimItemLifeBoxDrop);
    }

    private void OnClaimItemLifeBoxDrop(object obj)
    {
        float hpLifeBox = towerData.maxHP * (lifebox_hp_amount / 100);
        float maxHpLifeBox = towerData.maxHP * (lifebox_hp_amount / 100);

        curHpLifeBox = Mathf.Min(hpLifeBox, maxHpLifeBox);

        UpdateUI(curHpLifeBox);
    }

    private void UpdateUI(float value)
    {
        float hp = Mathf.Min(value / towerData.maxHP, 1);
        if(hp < 0)
        {
            hp = 0;
        }
        main.localScale = new Vector3(hp, 1, 1);
    }
}
