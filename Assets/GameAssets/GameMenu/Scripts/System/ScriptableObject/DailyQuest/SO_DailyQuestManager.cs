using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyQuestManager", menuName = "Data/DailyQuest/DailyQuestManager", order = 0)]
public class SO_DailyQuestManager : SerializedScriptableObject
{
    public Dictionary<DailyQuestType, List<int>> dailyQuestNormals = new();
    public Dictionary<DailyQuestSpecialType, List<int>> dailyQuestSpecials = new();
    public Dictionary<WorldType, RewardDailyQuest> rewardDailyQuests = new();
    public Dictionary<int, RewardDailyQuestWeekend> rewardDailyQuestsWeekend = new();
    private int[] probabilities = { 20, 30, 30, 15, 5 };

    private System.Random random = new System.Random();
    /*    public List<T> GetRandomEnumValues<T>(int count) where T : Enum
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            int totalValues = enumValues.Length;
            count = Mathf.Clamp(count, 1, totalValues);

            List<int> indexes = new List<int>(totalValues);
            for (int i = 0; i < totalValues; i++)
                indexes.Add(i);

            // Áp dụng Fisher-Yates Shuffle chỉ cho phần cần lấy
            for (int i = 0; i < count; i++)
            {
                int randomIndex = random.Next(i, totalValues);
                (indexes[i], indexes[randomIndex]) = (indexes[randomIndex], indexes[i]); 
            }

            List<T> result = new List<T>(count);
            for (int i = 0; i < count; i++)
                result.Add(enumValues[indexes[i]]);

            return result;
        }*/
    public List<T> GetRandomEnumValues<T>(int count = 5) where T : Enum
    {
        T[] enumValues = (T[])Enum.GetValues(typeof(T));
        int totalValues = enumValues.Length;

        count = Mathf.Min(count, totalValues);

        HashSet<T> selectedValues = new HashSet<T>();
        System.Random random = new System.Random();

        while (selectedValues.Count < count)
        {
            T randomValue = enumValues[random.Next(totalValues)];
            selectedValues.Add(randomValue);
        }

        return selectedValues.ToList();
    }


    public int GetRandomDailyQuestType(DailyQuestType dailyQuestType)
    {
        List<int> valueQuest = dailyQuestNormals[dailyQuestType];
        if(valueQuest.Count == 0) return 0;
        return GetRandomQuest(valueQuest);
    }

    public int GetRandomDailyQuestSpecial(DailyQuestSpecialType dailyQuestSpecialType)
    {
        List<int> valueQuest = dailyQuestSpecials[dailyQuestSpecialType];
        if (valueQuest.Count == 0) return 0;
        return GetRandomQuest(valueQuest);
    }

    private int GetRandomQuest(List<int> valueQuest)
    {
        int rand = UnityEngine.Random.Range(0, probabilities.Sum());
        int cumulative = 0;

        return valueQuest[probabilities
            .Select((prob, index) => cumulative += prob)
            .ToList()
            .FindIndex(cum => rand < cum)];
    }

    public RewardDailyQuest GetRewardDailyQuestByWorld(WorldType worldType)
    {
        if(rewardDailyQuests.ContainsKey(worldType)) return rewardDailyQuests[worldType];
        return null;
    }

    public RewardDailyQuestWeekend GetRewardDailyQuestsWeekendByKey(int pointCount)
    {
        if (rewardDailyQuestsWeekend.ContainsKey(pointCount)) return rewardDailyQuestsWeekend[pointCount];
        return null;
    }

    //tool
/*    [Button("Test")]
    public void Test()
    {
        foreach (var quest in GetRandomEnumValues<DailyQuestType>(4))
        {
            DebugCustom.LogColor(quest);
        }
        foreach (var quest in GetRandomEnumValues<DailyQuestSpecialType>(1))
        {
            DebugCustom.LogColor(quest);
        }
    }*/
}

public class RewardDailyQuest
{
    public int amountGold;
    public int amountGem;
}

public class RewardDailyQuestWeekend
{
    public TypeXBonusGold amountGold;
    public int amountGem;
    public int amountBadges;
    public int amountPowerstone;
}
