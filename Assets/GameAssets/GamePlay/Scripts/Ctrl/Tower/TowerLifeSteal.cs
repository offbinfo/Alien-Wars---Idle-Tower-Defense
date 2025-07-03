using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLifeSteal : GameMonoBehaviour
{
    private Object_DataInformation data;
    private float lifeSteal => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.life_steal);

    private void Awake()
    {
        data = GetComponent<Object_DataInformation>();
    }

    public void StealLife(float totalDamage)
    {
        data.hpCurrent += totalDamage * lifeSteal / 100f;
    }
}
