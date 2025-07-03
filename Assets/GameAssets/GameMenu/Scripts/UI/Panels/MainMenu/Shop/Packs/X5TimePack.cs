using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X5TimePack : BasePackUI
{
    protected override bool claimed => GameDatas.timeGameMax >= 5;

    private void Start()
    {
        BoosterPack boosterPack = basePack as BoosterPack;
        price.Setup(boosterPack.key);
        key = boosterPack.key;
    }

    public override void BuyItem()
    {
        if (GameDatas.timeGameMax >= 5)
            return;
        base.BuyItem();
    }

    public override void OnBought()
    {
        GameDatas.timeGameMax = 5;
    }
}
