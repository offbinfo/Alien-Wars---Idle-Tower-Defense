using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRevival : MonoBehaviour
{

    public int revival => revivalCount;

    private TowerData towerData;
    private TowerCtrl towerCtrl;

    int revivalCount = 0;
    private void Awake()
    {
        towerCtrl = GetComponent<TowerCtrl>();
        towerData = GetComponent<TowerData>();
        revivalCount = 0;
        if (GameDatas.ReviveTowerCount <= 0) StartCoroutine(I_CountRevivalReset());
    }
    public void Revival()
    {
        if (towerData.hpCurrent > 0) return;

        gameObject.SetActive(true);
        towerData.hpCurrent = towerData.maxHP;
        towerCtrl.HPBarTower.InActiveHpBar(true);
        revivalCount += 1;

        if (GameDatas.ReviveTowerCount - 1 <= 0)
        {
            GameDatas.timeTargetFullRevive = DateTime.Now.AddMinutes(15);
        }

        GameDatas.ReviveTowerCount -= 1;
        EventDispatcher.PostEvent(EventID.OnReviveTower, 0);

    }
    IEnumerator I_CountRevivalReset()
    {
        yield return new WaitUntil(() => DateTime.Now >= GameDatas.timeTargetFullRevive);
        GameDatas.ReviveTowerCount = 5;
    }
}
