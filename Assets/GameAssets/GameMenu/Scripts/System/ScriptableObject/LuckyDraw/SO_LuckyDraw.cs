using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_LuckyDraw", menuName = "Data/LuckyDraw/SO_LuckyDraw", order = 0)]
public class SO_LuckyDraw : SerializedScriptableObject
{
    public List<LuckyDraw> luckyDraws = new();
    public Sprite iconGem;
    public Sprite iconGold;
    public Sprite iconPowerStone;

    public Dictionary<WorldType, float> goldBonusWorld = new();

    public float GetGoldBonusWorld(WorldType type)
    {
        return goldBonusWorld[type];
    }
}

public class LuckyDraw
{
    public CurrencyType currencyType;
    [ShowIf(nameof(IsGem))]
    public int gem;
    [ShowIf(nameof(IsGold))]
    public int gold;
    [ShowIf(nameof(IsPowerStone))]
    public int powerStone;

    public float rate;

    private bool IsGem() => currencyType == CurrencyType.GEM;
    private bool IsGold() => currencyType == CurrencyType.GOLD;
    private bool IsPowerStone() => currencyType == CurrencyType.POWER_STONE;
}
