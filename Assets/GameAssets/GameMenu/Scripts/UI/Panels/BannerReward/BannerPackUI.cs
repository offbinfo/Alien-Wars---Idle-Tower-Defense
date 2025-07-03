using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BannerPackUI : BaseBannerPack
{
    [SerializeField]
    private GameObject btnUnBuy;

    protected override void Start()
    {
        base.Start();
        CheckActiveBuyPack();
    }

    public override void OnBought()
    {
        base.OnBought();
        CheckActiveBuyPack();
    }

    private void CheckActiveBuyPack()
    {
        if (typeBannerPack == TypeBannerPack.StonePack) return;
        bool isUnBuy = GameDatas.IsBuyBeginnerPack(indexPack);
        btnUnBuy.SetActive(isUnBuy);
        isBuy = !isUnBuy;
    }
}
