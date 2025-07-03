using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemPack", menuName = "Data/Shop/GemPack", order = 0)]
public class GemPack : BasePack
{
    public CurrencyType currencyType = CurrencyType.GEM;
    public int amount;
}
