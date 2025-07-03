using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Singleton<ThunderBolt>
{
    [SerializeField] UW_ID ID;
    [SerializeField]
    private Object_DataInformation dataInformation;
    public float changeThunder;
    public float thunderStunMulti;
    public bool thunderStockStun = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        var data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Active_Change>(ID);

        changeThunder = data.GetCurrentChange() + Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.THUNDER_STUN_CHANCE).GetCurrentProperty();
        thunderStunMulti = Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.THUNDER_STUN_MULTIPLIER).GetCurrentProperty();

        if(Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.THUNDER_STUN).currentLevel == Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.THUNDER_STUN).levelMax)
        {
            thunderStockStun = true;
        }

/*        DebugCustom.LogColor("changeThunder "+ changeThunder);
        DebugCustom.LogColor("GetCurrentQuantity " + data.GetCurrentQuantity());
        DebugCustom.LogColor("data.GetCurrentChange() " + data.GetCurrentChange());
        DebugCustom.LogColor("GetCurrentDmg " + data.GetCurrentDmg());*/
    }

    public Collider2D target;

}
