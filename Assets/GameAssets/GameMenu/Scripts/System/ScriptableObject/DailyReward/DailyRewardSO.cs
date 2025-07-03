using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardSO", menuName = "Data/DailyReward/DailyRewardSO", order = 0)]
public class DailyRewardSO : SerializedScriptableObject
{

    public List<DailyData> dailyDatas = new();
    public Sprite imgIcon;
    public Dictionary<int, AccumulateData> accumulateDatas = new();

    public Dictionary<WorldType, float> goldBonusWorld = new();

    public float GetGoldBonusWorld(WorldType type)
    {
        return goldBonusWorld[type];
    }

    public AccumulateData GetAccumulateDataDataByKey(int key)
    {
        return accumulateDatas[key];
    }
}

public class AccumulateData
{
    public int gold;
    public int gem;
    public int powerstone;
}

public class DailyData
{
    public CurrencyType currencyType;
    public int goldRewrad;
}
