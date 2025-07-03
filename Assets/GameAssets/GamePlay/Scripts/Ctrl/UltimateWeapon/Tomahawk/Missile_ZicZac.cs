using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_ZicZac : Projectile_ZicZac
{
    private void Awake()
    {
        AddEventOnReachTarget(OnReachTarget);
    }
    private void OnDestroy()
    {
        RemoveEventOnReachTarget(OnReachTarget);
    }
    private void OnReachTarget()
    {
        //set target random;
        var towerPos = TowerCtrl.instance.transform.position;
        var range = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_range) / 10;
        SetTarget((range + 1).GetRandomPosition() + towerPos);
    }
}
