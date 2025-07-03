using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BannerEventBoost : MonoBehaviour
{
    [SerializeField] Transform center;

    [SerializeField]
    private BasePack basePack;
    private EventBoostPack boostPack;
    [SerializeField] private PurchaserPrice price;
    [SerializeField] private GameObject btnUnBuy;

    private void Start()
    {
        boostPack = basePack as EventBoostPack;
        price.Setup(boostPack.key);
        CheckBuy();
    }

    public void Buy()
    {
        if(GameDatas.IsX2BadgesEvent()) return;
        GamePurchaser.BuyProduct(boostPack.key, () =>
        {
            GameDatas.X2BadgesEvent(true);
            GameDatas.Badges += boostPack.amountBadges;
            var TargetUIBadges = TopUI_Currency_Horizontal.instance != null ? TopUI_Currency_Horizontal.instance.badges.transform.position : CurrencyContainer.instance._trans_badges.position;
            ObjectUI_Fly_Manager.instance.Get(10, center.position, TargetUIBadges, CurrencyType.BADGES);

            GameDatas.Gem += boostPack.amountGem;
            GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + boostPack.amountGem);
            var TargetUI = TopUI_Currency_Horizontal.instance != null ? TopUI_Currency_Horizontal.instance.gemIcon.transform.position : CurrencyContainer.instance._trans_gem.position;
            ObjectUI_Fly_Manager.instance.Get(10, center.position, TargetUI, CurrencyType.GEM);
            CheckBuy();
        });

        // display bonus

    }

    private void CheckBuy()
    {
        btnUnBuy.gameObject.SetActive(GameDatas.IsX2BadgesEvent());
    }
}
