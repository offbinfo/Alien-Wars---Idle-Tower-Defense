using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class UnlockUWElement : GameMonoBehaviour
{
    [SerializeField]
    private TMP_Text txtPrice;
    [SerializeField]
    private GameObject btnBuy;
    private bool isUnlock;
    public Action OnOpenUltimate;
    private int priceUnlock;
    [SerializeField]
    private GameObject btnUnBuy;

    [SerializeField]
    private TabUltimateChoose tabUltimateChoose;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnUnlockUltimateWeapon, (obj) => {
            SetData();
            tabUltimateChoose.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        SetData();
    }

    public void SetData()
    {
        priceUnlock = Cfg.UWCtrl.GetPriceUnlockUW();
        txtPrice.text = string.Format("{0} <sprite name=powerstone> " + LanguageManager.GetText("unlock"), priceUnlock);
        CheckHide();
        btnUnBuy.SetActive(priceUnlock > GameDatas.PowerStone);
    }

    private void CheckHide()
    {
        var countUnlock = Cfg.UWCtrl.UWeaponManager.CountUnlock;
        var maxUW = Cfg.UWCtrl.UWeaponManager.ultimateWeapons.Count;
        if (countUnlock >= maxUW)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnBuy()
    {
        tabUltimateChoose.gameObject.SetActive(true);
    }
}
