using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventBoostPack", menuName = "Data/EventPack/EventBoostPack", order = 0)]
public class EventBoostPack : BasePack
{
    //[ShowIf(nameof(IsBadges))]
    public int amountBadges;
    //[ShowIf(nameof(IsGem))]
    public int amountGem;
/*    public CurrencyType currencyType;

    private bool IsGem() => currencyType == CurrencyType.GEM;
    private bool IsBadges() => currencyType == CurrencyType.BADGES;*/
}
