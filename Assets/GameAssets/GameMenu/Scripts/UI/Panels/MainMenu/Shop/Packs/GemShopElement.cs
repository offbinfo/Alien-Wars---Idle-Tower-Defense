using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class GemShopElement : BasePackUI
{
    [SerializeField] TextMeshProUGUI gemText;
    private GemPack gemPack;

    private void Start()
    {
        gemPack = basePack as GemPack;
        gemText.text = gemPack.amount.ToString();
        price.Setup(gemPack.key);
        key = gemPack.key;
    }

    public override void OnBought()
    {
        OnConllect(gemPack.amount);
    }
}
