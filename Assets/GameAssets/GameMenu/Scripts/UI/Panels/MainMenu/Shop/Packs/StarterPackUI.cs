using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarterPackUI : BasePackUI
{
    protected override bool claimed => GameDatas.isX2;
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
        GameDatas.isX2 = true;
        OnConllect(goldenPack.amountGem);
    }
}
