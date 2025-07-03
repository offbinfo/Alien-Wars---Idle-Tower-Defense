using EasyUI.PickerWheelUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class PopupLuckyDraw : UIPanel, IBoard
{
    [SerializeField]
    private PickerWheel pickerWheel;
    [SerializeField]
    private Transform center;
    [SerializeField]
    private GameObject lockSpin;
    [SerializeField]
    private GameObject btnUnBuy;
    public bool isSpin = false;
    [SerializeField]
    private TMP_Text txtSpin;
    [SerializeField]
    private TMP_Text txtPrice;

    [SerializeField]
    private int priceGemMoreTurn1 = 5;
    private int priceGemMoreTurn2 = 10;

    private int priceGem;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupLuckyDraw;
    }

    private void OnEnable()
    {
        OnAppear();
        isSpin = false;

        string lastPurchaseDate = GameDatas.GetLastPurchaseDate();

        if (lastPurchaseDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            purchaseCount = 0;
            GameDatas.SetMoreTurnPurchaseCount(purchaseCount);
            GameDatas.SetLastPurchaseDate();
            RefreshUI();
        }

        RefreshUI();

        pickerWheel.OpenPickerWheel();
    }

    private void RefreshUI()
    {
        purchaseCount = GameDatas.GetMoreTurnPurchaseCount();
        priceGem = (purchaseCount == 0) ? priceGemMoreTurn1 : priceGemMoreTurn2;
        txtPrice.text = priceGem.ToString();

        if(purchaseCount >= 2)
        {
            btnUnBuy.SetActive(true);
        }
        else
        {
            btnUnBuy.SetActive(priceGemMoreTurn1 > GameDatas.Gem);
        }
    }

    public override void Close()
    {
        base.Close();
        pickerWheel.ClosePickerWheel();
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
        CheckSpin();
    }

    private int purchaseCount;
    public void BtnMoreTurn()
    {
        purchaseCount = GameDatas.GetMoreTurnPurchaseCount();
        string lastPurchaseDate = GameDatas.GetLastPurchaseDate();

        if (lastPurchaseDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            purchaseCount = 0;
            GameDatas.SetLastPurchaseDate();
        }

        if (purchaseCount >= 2)
        {
            RefreshUI();
            DebugCustom.LogColor("Bạn chỉ có thể mua tối đa 2 lần mỗi ngày.");
            return;
        }

        priceGem = (purchaseCount == 0) ? priceGemMoreTurn1 : priceGemMoreTurn2;

        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceGem, OnBuySuccess);
    }

    private void OnBuySuccess(bool obj)
    {
        if(obj)
        {
            purchaseCount++;
            GameDatas.SetMoreTurnPurchaseCount(purchaseCount);
            OnClaimMoreTurn();
            RefreshUI();
        }
    }

    private void OnClaimMoreTurn()
    {
        GameDatas.CountSpinFree = 2;
        CheckSpin();
    }

    public void BtnSpinUsingAds()
    {
        WatchAds.WatchRewardedVideo(() => {
            GameDatas.CountSpinFree = 1;
            CheckSpin();
        }, "MoreTurnSpinAds");
    }

    public int BonusGoldByWorld(WheelPiece wheel)
    {
        return (int)(wheel.Amount * pickerWheel.SO_LuckyDraw.GetGoldBonusWorld((WorldType)GameDatas.GetHighestWorld()));
    }

    private void CheckSpin()
    {
        lockSpin.SetActive(!(GameDatas.IsSpinFree || GameDatas.CountSpinFree > 0));
    }

    public void BtnSpin()
    {
        if (isSpin) return;
        if(!GameDatas.IsSpinFree)
        {
            if(GameDatas.CountSpinFree > 0)
            {
                GameDatas.CountSpinFree -= 1;
                OnSpin();
            }
        } else
        {
            GameDatas.IsSpinFree = false;
            OnSpin();
        }
    }

    private void OnSpin()
    {
        txtSpin.text = "Is Spinning";
        isSpin = true;
        pickerWheel.Spin();
        pickerWheel.OnSpinEnd(wheel =>
        {
            txtSpin.text = "Spin";
            isSpin = false;
            ActiveAnimFlyCurrency(wheel.Currency, wheel.Amount);
            CheckSpin();
        });
    }

    private void ActiveAnimFlyCurrency(CurrencyType currency, int amount)
    {
        Transform posEnd = null;
        TopUI_Currency topUI_Currency = TopUI_Currency.instance;
        switch (currency)
        {
            case CurrencyType.GOLD:
                GameDatas.Gold += amount;
                posEnd = topUI_Currency.goldIcon.transform;
                break;
            case CurrencyType.GEM:
                GameDatas.Gem += amount;
                posEnd = topUI_Currency.gemIcon.transform;
                break;
            case CurrencyType.POWER_STONE:
                GameDatas.PowerStone += amount;
                posEnd = topUI_Currency.powerStoneIcon.transform;
                break;
            default:
                break;
        }
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, posEnd.position, currency);
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
