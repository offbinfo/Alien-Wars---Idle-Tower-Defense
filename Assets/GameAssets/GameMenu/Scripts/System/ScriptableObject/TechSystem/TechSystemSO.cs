using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TechSystemSO", menuName = "Data/TechSystem/TechSystemSO", order = 0)]
public class TechSystemSO : SerializedScriptableObject
{
    public Dictionary<TypeTech, TechData> techDict = new();
    public readonly Dictionary<TypeRarityTech, double> coefficientTechDict = new()
    {
        { TypeRarityTech.Common,        1.012 },
        { TypeRarityTech.UnCommon,      1.032 },
        { TypeRarityTech.UnCommonPlus,  1.052 },
        { TypeRarityTech.Rare,          1.072 },
        { TypeRarityTech.RarePlus,      1.102 },
        { TypeRarityTech.Legendary,     1.132 },
        { TypeRarityTech.LegendaryPlus, 1.162 },
        { TypeRarityTech.Artifact,      1.202 },
        { TypeRarityTech.ArtifactPlus,  1.252 },
        { TypeRarityTech.Heirloom,      1.312 }
    };
    public readonly Dictionary<TypeRarityTech, double> maxLevelTech = new()
    {
        { TypeRarityTech.Common,        20 },
        { TypeRarityTech.UnCommon,      40 },
        { TypeRarityTech.UnCommonPlus,  40 },
        { TypeRarityTech.Rare,          60 },
        { TypeRarityTech.RarePlus,      80 },
        { TypeRarityTech.Legendary,     100 },
        { TypeRarityTech.LegendaryPlus, 120 },
        { TypeRarityTech.Artifact,      140 },
        { TypeRarityTech.ArtifactPlus,  160 },
        { TypeRarityTech.Heirloom,      200 }
    };
    private const double PerLevelIncrement = 0.002;

    public float GetStatValue(TypeTech typeTech, TypeRarityTech rarity)
    {
        if (techDict.TryGetValue(typeTech, out var modifier))
        {
            return modifier.GetValue(rarity);
        }
        return 0f;
    }

    public TechData GetTechData(TypeTech typeTech, TypeRarityTech rarity)
    {
        if (techDict.TryGetValue(typeTech, out var modifier))
        {
            return modifier;
        }
        return null;
    }

    public double CalculateValue(TypeRarityTech rarity, int level)
    {
        if (!coefficientTechDict.ContainsKey(rarity))
        {
            DebugCustom.LogError("Rarity not found.");
            return 0;
        }

        double baseValue = coefficientTechDict[rarity];
        return baseValue + (level - 1) * PerLevelIncrement;
    }

    public bool IsMaxLevel(TypeTech typeTech ,TypeRarityTech rarity, int uniqueTech)
    {
        return GameDatas.GetLevelTech(typeTech, uniqueTech) >= maxLevelTech[rarity];
    }
}

[Serializable]
public class CoefficientTech
{
    public float coefficient;
    public int maxLevel;
}

[Serializable]
public class TechData
{
    public TypeTech typeTech;
    public string desc;
    public TypeFormat typeFormat;
    public List<float> values = new List<float>(6);

    public float GetValue(TypeRarityTech rarity)
    {
        int index = (int)rarity;
        if (index < values.Count)
            return values[index];
        return 0f;
    }
}
