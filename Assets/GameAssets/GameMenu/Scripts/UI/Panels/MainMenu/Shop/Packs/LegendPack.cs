using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LegendPack : BasePackUI
{
    protected override bool claimed => GameDatas.isX4;
    private GoldenPack goldenPack;

    private void Start()
    {
        goldenPack = basePack as GoldenPack;
        price.Setup(goldenPack.key);
        key = goldenPack.key;

        SetUpTextAmountPackGolden(goldenPack.currencyType, 
            goldenPack.typeXBonusCurrency, goldenPack.amountGold, goldenPack.amountGem);
    }

    public override void OnBought()
    {
        GameDatas.isX4 = true;
        OnConllect(goldenPack.amountGem);
    }
}
