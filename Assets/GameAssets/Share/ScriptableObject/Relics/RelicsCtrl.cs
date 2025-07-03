using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelicsCtrl : GameMonoBehaviour
{
    public RelicManagerSO manager;

    private static readonly Dictionary<BonusTypeRelic, TypeRelic[]> RelicMap = new()
    {
        { BonusTypeRelic.Coins, new[] {
            TypeRelic.DemonsPact, TypeRelic.DragonsBreath,
            TypeRelic.KingsTribute, TypeRelic.DimensionalRift
        }},
        { BonusTypeRelic.TowerDamage, new[] {
            TypeRelic.PhoenixFeather, TypeRelic.HoneyDrop,
            TypeRelic.SanctuaryOrb, TypeRelic.TwilightCrystal
        }},
        { BonusTypeRelic.TowerHealth, new[] {
            TypeRelic.BloodfangAmulet, TypeRelic.AlchemistsStone,
            TypeRelic.StormcallerTotem, TypeRelic.EclipseEye,
            TypeRelic.LunarCharm, TypeRelic.EldritchSigil
        }},
        { BonusTypeRelic.DefenseResistance, new[] {
            TypeRelic.ChronoCore, TypeRelic.StoneGuardian,
            TypeRelic.OrbOfOmniscience, TypeRelic.HourglassOfOblivion
        }},
        { BonusTypeRelic.CritFactor, new[] {
            TypeRelic.VoidFang, TypeRelic.GoldenTouch,
            TypeRelic.SoulHarvester, TypeRelic.FrostboundSigil,
            TypeRelic.VeilOfTheUnknown
        }},
        { BonusTypeRelic.LabSpeed, new[] {
            TypeRelic.CelestialBlade, TypeRelic.MerchantsFortune,
            TypeRelic.ChaosRune, TypeRelic.WhisperingRelic
        }},
        { BonusTypeRelic.DamagePerMeter, new[] {
            TypeRelic.ShadowCloak, TypeRelic.TreasureCompass,
            TypeRelic.NecromancersGrasp, TypeRelic.HourglassOfOblivion
        }},
    };

    /*    public float PercentRelic(BonusTypeRelic bonusTypeRelic, float parameter)
        {
            if (!RelicMap.TryGetValue(bonusTypeRelic, out var relics))
                return parameter;

            return relics.Sum(r => Mathf.RoundToInt(parameter * manager.GetPercentRelicByType(r)));
        }*/

    private void Start()
    {
        InitRelicReward();
    }

    private void InitRelicReward()
    {
        DebugCustom.LogColor("GameDatas.IsRandomRelicReward() " + GameDatas.IsRandomRelicReward());
        if (GameDatas.IsRandomRelicReward()) return;
        GameDatas.RandomRelicReward(true);

        var lockedRelics = manager.relicDicts
            .Where(kv => !GameDatas.IsRelicUnlock(kv.Key))
            .OrderBy(_ => Random.value) // Shuffle
            .Take(4)
            .Select(kv => kv.Value)
            .ToList();

        GameDatas.SaveAllListRelicItems(lockedRelics);
    }

    public float PercentRelic(BonusTypeRelic bonusTypeRelic, float parameter)
    {
        if (!RelicMap.TryGetValue(bonusTypeRelic, out var relics))
            return parameter;

        float totalBonus = 0f;

        foreach (var relic in relics)
        {
            if (GameDatas.IsRelicEquiped(relic))
            {
                float percent = manager.GetPercentRelicByType(relic);
                totalBonus += (parameter * percent);
            }
        }

        return parameter + totalBonus;
    }

    public float PercentDeductRelic(BonusTypeRelic bonusTypeRelic, float parameter)
    {
        if (!RelicMap.TryGetValue(bonusTypeRelic, out var relics))
            return parameter;

        float totalBonus = 0f;

        foreach (var relic in relics)
        {
            if (GameDatas.IsRelicEquiped(relic))
            {
                float percent = manager.GetPercentRelicByType(relic);
                totalBonus += (parameter * percent);
            }
        }

        return parameter - totalBonus;
    }

}
