using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BannerPackSO", menuName = "Data/Banner/BannerPackSO", order = 0)]
public class BannerPackSO : ScriptableObject
{
    public TypeBannerPack typeBannerPack;
    public float amountGem;
    public float amountPowerStone;
    public float amountGold;
}
