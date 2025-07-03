using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdspack : BasePackUI
{
    protected override bool claimed { get => GameDatas.RemoveAdsForever; }

    private void Start()
    {
        BoosterPack boosterPack = basePack as BoosterPack;
        price.Setup(boosterPack.key);
        key = boosterPack.key;
    }

    public override void BuyItem()
    {
        if (GameDatas.RemoveAdsForever)
            return;
        base.BuyItem();
    }

    public override void OnBought()
    {
        GameDatas.RemoveAdsForever = true;
    }
}
