using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegendaryPack", menuName = "Data/Shop/LegendaryPack", order = 0)]
public class LegendaryPack : BasePack
{
    [ShowIf(nameof(IsGold))]
    public int amountGold;
    [ShowIf(nameof(IsGem))]
    public int amountGem;
    public CurrencyType currencyType;

    private bool IsGem() => currencyType == CurrencyType.GEM;
    private bool IsGold() => currencyType == CurrencyType.GOLD;
}
