using UnityEngine;

[CreateAssetMenu(fileName = "GoldPack", menuName = "Data/Shop/GoldPack", order = 0)]
public class GoldPack : BasePack
{
    public CurrencyType currencyType = CurrencyType.GOLD;
    public int amount;
}
