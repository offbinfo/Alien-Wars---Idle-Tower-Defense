using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubStatsTechSystemSO", menuName = "Data/TechSystem/SubStatsTechSystemSO", order = 0)]
public class SubStatsTechSystemSO : SerializedScriptableObject
{
    public Dictionary<SubStatsTechSystem, StatModifier> statDict = new();

    public float GetStatValue(SubStatsTechSystem subStatsTech, TypeRarityTech rarity)
    {
        if (statDict.TryGetValue(subStatsTech, out var modifier))
        {
            return modifier.GetValue(rarity);
        }
        return 0f;
    }

    public StatModifier GetStatModifier(SubStatsTechSystem subStatsTech, TypeRarityTech rarity)
    {
        if (statDict.TryGetValue(subStatsTech, out var modifier))
        {
            return modifier;
        }
        return null;
    }

    /*    [Button("Add Data")]
        public void AddData()
        {
            statDict.Clear();
            statDict = new()
            {
                [SubStatsTechSystem.HpRegen] = new StatModifier(
            SubStatsTechSystem.HpRegen,
            TypeFormat.Percent,
            new List<float> { 20f, 40f, 60f, 100f, 200f, 400f }
        ),

                [SubStatsTechSystem.DamageResistancePercent] = new StatModifier(
            SubStatsTechSystem.DamageResistancePercent,
            TypeFormat.Percent,
            new List<float> { 1f, 2f, 3f, 5f, 6f, 8f }
        ),

                [SubStatsTechSystem.DamageReduce] = new StatModifier(
            SubStatsTechSystem.DamageReduce,
            TypeFormat.Percent,
            new List<float> { 15f, 25f, 40f, 100f, 500f, 1000f }
        ),

                [SubStatsTechSystem.DamageReturnPower] = new StatModifier(
            SubStatsTechSystem.DamageReturnPower,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 2f, 4f, 7f, 10f }
        ),

                [SubStatsTechSystem.Lifesteal] = new StatModifier(
            SubStatsTechSystem.Lifesteal,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 0.3f, 0.5f, 1.5f, 2f }
        ),

                [SubStatsTechSystem.ImpulseWaveSize] = new StatModifier(
            SubStatsTechSystem.ImpulseWaveSize,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 0.1f, 0.3f, 0.7f, 1f }
        ),

                [SubStatsTechSystem.ImpulseWaveFrequency] = new StatModifier(
            SubStatsTechSystem.ImpulseWaveFrequency,
            TypeFormat.Seconds,
            new List<float> { 0f, 0f, -1f, -2f, -3f, -4f }
        ),

                [SubStatsTechSystem.DodgeChance] = new StatModifier(
            SubStatsTechSystem.DodgeChance,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 0f, 1.5f, 3.5f, 5f }
        ),

                [SubStatsTechSystem.ShieldHp] = new StatModifier(
            SubStatsTechSystem.ShieldHp,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 20f, 40f, 90f, 120f }
        ),

                [SubStatsTechSystem.ShieldTime] = new StatModifier(
            SubStatsTechSystem.ShieldTime,
            TypeFormat.Seconds,
            new List<float> { 0f, 0f, -20f, -40f, -80f, -100f }
        ),

                [SubStatsTechSystem.SatelliteDamage] = new StatModifier(
            SubStatsTechSystem.SatelliteDamage,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 40f, 100f, 300f, 800f }
        ),

                [SubStatsTechSystem.BombDamage] = new StatModifier(
            SubStatsTechSystem.BombDamage,
            TypeFormat.Percent,
            new List<float> { 0f, 30f, 50f, 150f, 500f, 800f }
        ),

                [SubStatsTechSystem.BombArea] = new StatModifier(
            SubStatsTechSystem.BombArea,
            TypeFormat.Flat,
            new List<float> { 0f, 0.1f, 0.15f, 0.3f, 0.75f, 1f }
        ),
                [SubStatsTechSystem.AttackSpeed] = new StatModifier(
            SubStatsTechSystem.AttackSpeed,
            TypeFormat.Flat,
            new List<float> { 0.3f, 0.5f, 0.7f, 1f, 3f, 5f }
        ),

                [SubStatsTechSystem.CriticalChance] = new StatModifier(
            SubStatsTechSystem.CriticalChance,
            TypeFormat.Percent,
            new List<float> { 2f, 3f, 4f, 6f, 8f, 10f }
        ),

                [SubStatsTechSystem.CriticalFactor] = new StatModifier(
            SubStatsTechSystem.CriticalFactor,
            TypeFormat.Flat,
            new List<float> { 2f, 4f, 6f, 8f, 12f, 15f }
        ),

                [SubStatsTechSystem.AttackRange] = new StatModifier(
            SubStatsTechSystem.AttackRange,
            TypeFormat.Flat,
            new List<float> { 2f, 4f, 8f, 12f, 20f, 30f }
        ),

                [SubStatsTechSystem.DamagePerMeter] = new StatModifier(
            SubStatsTechSystem.DamagePerMeter,
            TypeFormat.Percent,
            new List<float> { 3f, 5f, 7f, 14f, 20f, 30f }
        ),

                [SubStatsTechSystem.MultiShotChance] = new StatModifier(
            SubStatsTechSystem.MultiShotChance,
            TypeFormat.Percent,
            new List<float> { 0f, 3f, 5f, 7f, 10f, 13f }
        ),

                [SubStatsTechSystem.MultiShotTargets] = new StatModifier(
            SubStatsTechSystem.MultiShotTargets,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 3f, 4f }
        ),

                [SubStatsTechSystem.RapidFireChance] = new StatModifier(
            SubStatsTechSystem.RapidFireChance,
            TypeFormat.Percent,
            new List<float> { 0f, 2f, 4f, 6f, 9f, 12f }
        ),

                [SubStatsTechSystem.BounceShotChance] = new StatModifier(
            SubStatsTechSystem.BounceShotChance,
            TypeFormat.Percent,
            new List<float> { 0f, 2f, 3f, 5f, 9f, 12f }
        ),

                [SubStatsTechSystem.BounceShotTargets] = new StatModifier(
            SubStatsTechSystem.BounceShotTargets,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 3f, 4f }
        ),

                [SubStatsTechSystem.BounceShotRange] = new StatModifier(
            SubStatsTechSystem.BounceShotRange,
            TypeFormat.Flat,
            new List<float> { 0f, 0.5f, 0.8f, 1.2f, 1.8f, 2f }
        ),

                [SubStatsTechSystem.KnockbackChance] = new StatModifier(
            SubStatsTechSystem.KnockbackChance,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 2f, 4f, 6f, 9f }
        ),

                [SubStatsTechSystem.KnockbackForce] = new StatModifier(
            SubStatsTechSystem.KnockbackForce,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 0.1f, 0.4f, 0.9f, 1.5f }
        ),

                [SubStatsTechSystem.StunChance] = new StatModifier(
            SubStatsTechSystem.StunChance,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 0f, 2f, 6f, 10f }
        ),

                [SubStatsTechSystem.CorpseExplosionDamage] = new StatModifier(
            SubStatsTechSystem.CorpseExplosionDamage,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 50f, 120f, 210f, 600f }
        ),
                [SubStatsTechSystem.CoinPerKill] = new StatModifier(
            SubStatsTechSystem.CoinPerKill,
            TypeFormat.Flat,
            new List<float> { 0.1f, 0.2f, 0.3f, 0.5f, 1.2f, 2.5f }
        ),

                [SubStatsTechSystem.CoinPerWave] = new StatModifier(
            SubStatsTechSystem.CoinPerWave,
            TypeFormat.Flat,
            new List<float> { 30f, 50f, 100f, 200f, 500f, 1000f }
        ),

                [SubStatsTechSystem.GoldPerKill] = new StatModifier(
            SubStatsTechSystem.GoldPerKill,
            TypeFormat.Flat,
            new List<float> { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f }
        ),

                [SubStatsTechSystem.GoldPerWave] = new StatModifier(
            SubStatsTechSystem.GoldPerWave,
            TypeFormat.Flat,
            new List<float> { 20f, 35f, 60f, 120f, 200f, 350f }
        ),

                [SubStatsTechSystem.FreeAttackUpgrade] = new StatModifier(
            SubStatsTechSystem.FreeAttackUpgrade,
            TypeFormat.Percent,
            new List<float> { 2f, 4f, 6f, 8f, 10f, 12f }
        ),

                [SubStatsTechSystem.FreeDefenseUpgrade] = new StatModifier(
            SubStatsTechSystem.FreeDefenseUpgrade,
            TypeFormat.Percent,
            new List<float> { 2f, 4f, 6f, 8f, 10f, 12f }
        ),

                [SubStatsTechSystem.FreeResourceUpgrade] = new StatModifier(
            SubStatsTechSystem.FreeResourceUpgrade,
            TypeFormat.Percent,
            new List<float> { 2f, 4f, 6f, 8f, 10f, 12f }
        ),

                [SubStatsTechSystem.CoinInterest] = new StatModifier(
            SubStatsTechSystem.CoinInterest,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 2f, 4f, 6f, 8f }
        ),

                [SubStatsTechSystem.KillBeamDamage] = new StatModifier(
            SubStatsTechSystem.KillBeamDamage,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 0.7f, 1f, 1.5f, 0f }
        ),

                [SubStatsTechSystem.LifeboxHpAmount] = new StatModifier(
            SubStatsTechSystem.LifeboxHpAmount,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 3f, 5f, 7f, 10f }
        ),

                [SubStatsTechSystem.LifeboxMaxHp] = new StatModifier(
            SubStatsTechSystem.LifeboxMaxHp,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 0.4f, 0.7f, 1f, 1.5f }
        ),

                [SubStatsTechSystem.LifeboxChance] = new StatModifier(
            SubStatsTechSystem.LifeboxChance,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 5f, 8f, 11f, 15f }
        ),
                [SubStatsTechSystem.GoldenSanctuaryBonus] = new StatModifier(
            SubStatsTechSystem.GoldenSanctuaryBonus,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 3f, 4f }
        ),

                [SubStatsTechSystem.GoldenSanctuaryDuration] = new StatModifier(
            SubStatsTechSystem.GoldenSanctuaryDuration,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 2f, 4f, 7f, 0f }
        ),

                [SubStatsTechSystem.GoldenSanctuaryCooldown] = new StatModifier(
            SubStatsTechSystem.GoldenSanctuaryCooldown,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, -5f, -8f, -12f, 0f }
        ),

                [SubStatsTechSystem.VoidNexusSize] = new StatModifier(
            SubStatsTechSystem.VoidNexusSize,
            TypeFormat.Flat,
            new List<float> { 2f, 4f, 6f, 8f, 10f, 12f }
        ),

                [SubStatsTechSystem.VoidNexusDuration] = new StatModifier(
            SubStatsTechSystem.VoidNexusDuration,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 2f, 3f, 4f, 0f }
        ),

                [SubStatsTechSystem.VoidNexusCooldown] = new StatModifier(
            SubStatsTechSystem.VoidNexusCooldown,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, -2f, -3f, -4f, 0f }
        ),

                [SubStatsTechSystem.HighlightBonus] = new StatModifier(
            SubStatsTechSystem.HighlightBonus,
            TypeFormat.Flat,
            new List<float> { 1.2f, 2.5f, 3.5f, 10f, 15f, 20f }
        ),

                [SubStatsTechSystem.HighlightAngle] = new StatModifier(
            SubStatsTechSystem.HighlightAngle,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 3f, 6f, 11f, 15f }
        ),

                [SubStatsTechSystem.AntiForceDuration] = new StatModifier(
            SubStatsTechSystem.AntiForceDuration,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 4f, 7f, 10f, 0f }
        ),

                [SubStatsTechSystem.AntiForceSpeedReduction] = new StatModifier(
            SubStatsTechSystem.AntiForceSpeedReduction,
            TypeFormat.Percent,
            new List<float> { 0f, 0f, 3f, 8f, 11f, 15f }
        ),

                [SubStatsTechSystem.AntiForceCooldown] = new StatModifier(
            SubStatsTechSystem.AntiForceCooldown,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, -4f, -7f, -10f, 0f }
        ),

                [SubStatsTechSystem.ShockwaveDamage] = new StatModifier(
            SubStatsTechSystem.ShockwaveDamage,
            TypeFormat.Percent,
            new List<float> { 50f, 100f, 150f, 300f, 500f, 760f }
        ),

                [SubStatsTechSystem.ShockwaveQuantity] = new StatModifier(
            SubStatsTechSystem.ShockwaveQuantity,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 3f, 0f }
        ),

                [SubStatsTechSystem.ShockwaveCooldown] = new StatModifier(
            SubStatsTechSystem.ShockwaveCooldown,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, -6f, -10f, -13f, 0f }
        ),

                [SubStatsTechSystem.TomahawkDamage] = new StatModifier(
            SubStatsTechSystem.TomahawkDamage,
            TypeFormat.Flat,
            new List<float> { 8f, 15f, 25f, 50f, 100f, 250f }
        ),

                [SubStatsTechSystem.TomahawkQuantity] = new StatModifier(
            SubStatsTechSystem.TomahawkQuantity,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 4f, 5f }
        ),

                [SubStatsTechSystem.TomahawkCooldown] = new StatModifier(
            SubStatsTechSystem.TomahawkCooldown,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, -2f, -4f, -6f, 0f }
        ),

                [SubStatsTechSystem.ThunderboltDamage] = new StatModifier(
            SubStatsTechSystem.ThunderboltDamage,
            TypeFormat.Percent,
            new List<float> { 150f, 300f, 700f, 1500f, 3000f, 4000f }
        ),

                [SubStatsTechSystem.ThunderboltQuantity] = new StatModifier(
            SubStatsTechSystem.ThunderboltQuantity,
            TypeFormat.Flat,
            new List<float> { 0f, 0f, 1f, 2f, 3f, 4f }
        ),

                [SubStatsTechSystem.ThunderboltChance] = new StatModifier(
            SubStatsTechSystem.ThunderboltChance,
            TypeFormat.Percent,
            new List<float> { 2f, 4f, 6f, 9f, 12f, 15f }
        )
            };
        }*/
}

[Serializable]
public class StatModifier
{
    public SubStatsTechSystem subStatsTech;
    public TypeFormat typeFormat;
    public List<float> values = new List<float>(6);

    public StatModifier(SubStatsTechSystem subStatsTech, TypeFormat typeFormat, List<float> values)
    {
        this.subStatsTech = subStatsTech;
        this.typeFormat = typeFormat;
        this.values = values;
    }

    public float GetValue(TypeRarityTech rarity)
    {
        int index = (int)rarity;
        if (index < values.Count)
            return values[index];
        return 0f;
    }
}
