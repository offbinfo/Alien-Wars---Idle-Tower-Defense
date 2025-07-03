using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHomeCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject unlockBtnDailyGift;
    [SerializeField]
    private GameObject lockBtnDailyGift;
    [SerializeField]
    private GameObject unlockBtnLuckyDraw;
    [SerializeField]
    private GameObject lockBtnLuckyDraw;
    [SerializeField]
    private GameObject unlockBtnRanking;
    [SerializeField]
    private GameObject lockBtnRanking;
    [SerializeField]
    private GameObject unlockBtnArena;
    [SerializeField]
    private GameObject lockBtnArena;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
        OnRefeshUIHome(null);
    }

    private void OnRefeshUIHome(object obj)
    {
        unlockBtnDailyGift.SetActive(GameDatas.isUnlockDailyGift);
        lockBtnDailyGift.SetActive(!GameDatas.isUnlockDailyGift);

        unlockBtnLuckyDraw.SetActive(GameDatas.isUnlockLuckyDraw);
        lockBtnLuckyDraw.SetActive(!GameDatas.isUnlockLuckyDraw);

        unlockBtnRanking.SetActive(GameDatas.isUnlockRanking);
        lockBtnRanking.SetActive(!GameDatas.isUnlockRanking);

        unlockBtnArena.SetActive(GameDatas.IsUnlockFeatureArena);
        lockBtnArena.SetActive(!GameDatas.IsUnlockFeatureArena);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
    }
}
