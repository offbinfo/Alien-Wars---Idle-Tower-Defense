using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicManagerSO", menuName = "Data/Relics/RelicManagerSO", order = 0)]
public class RelicManagerSO : SerializedScriptableObject
{
    public Dictionary<TypeRelic, RelicData> relicDicts = new();
    public List<RelicData> relicRares = new();
    public List<RelicData> relicEpics = new();

    public RelicData GetRelicData(TypeRelic typeRelic)
    {
        return relicDicts[typeRelic];
    }

    public Sprite GetIconRelicData(TypeRelic typeRelic)
    {
        return relicDicts[typeRelic].icon;
    }

    public float GetPercentRelicByType(TypeRelic typeRelic)
    {
        if (GameDatas.IsRelicEquiped(typeRelic))
        {
            return relicDicts[typeRelic].numberPercent / 100;
        }
        return 1f;
    }

/*    [Button("Add Data List")]
    public void AddDataList()
    {
        relicRares.Clear();
        relicEpics.Clear();

        foreach (var relic in relicDicts.Values)
        {
            switch (relic.typeRarityRelic)
            {
                case TypeRarityRelic.Rare:
                    relicRares.Add(relic);
                    break;
                case TypeRarityRelic.Epic:
                    relicEpics.Add(relic);
                    break;
            }
        }
    }
    [Button("Add Desc")]
    public void Awake()
    {
        relicDicts[TypeRelic.DemonsPact].desc = "Increase coins earned by 2%";
        relicDicts[TypeRelic.PhoenixFeather].desc = "Increase tower damage by 2%";
        relicDicts[TypeRelic.ChronoCore].desc = "Increase defense resistance by 2%";
        relicDicts[TypeRelic.HoneyDrop].desc = "Increase tower damage by 2%";
        relicDicts[TypeRelic.BloodfangAmulet].desc = "Increase tower health by 2%";
        relicDicts[TypeRelic.DragonsBreath].desc = "Increase coins earned by 2%";
        relicDicts[TypeRelic.CelestialBlade].desc = "Increase lab speed by 1.5%";
        relicDicts[TypeRelic.VoidFang].desc = "Increase crit factor by 2%";
        relicDicts[TypeRelic.StoneGuardian].desc = "Increase defense resistance by 2%";
        relicDicts[TypeRelic.ShadowCloak].desc = "Increase Damage/meter by 2%";
        relicDicts[TypeRelic.SanctuaryOrb].desc = "Increase tower damage by 2%";
        relicDicts[TypeRelic.AlchemistsStone].desc = "Increase tower health by 2%";
        relicDicts[TypeRelic.MerchantsFortune].desc = "Increase lab speed by 2%";
        relicDicts[TypeRelic.TreasureCompass].desc = "Increase damage/meter by 2%";
        relicDicts[TypeRelic.GoldenTouch].desc = "Increase crit factor by 2%";
        relicDicts[TypeRelic.KingsTribute].desc = "Increase Coins by 2%";
        relicDicts[TypeRelic.StormcallerTotem].desc = "Increase Health by 2%";

        relicDicts[TypeRelic.SoulHarvester].desc = "Increase critical factor by 5%";
        relicDicts[TypeRelic.EclipseEye].desc = "Increase tower health by 5%";
        relicDicts[TypeRelic.ChaosRune].desc = "Increase lab speed by 4%";
        relicDicts[TypeRelic.FrostboundSigil].desc = "Increase critical factor by 5%";
        relicDicts[TypeRelic.OrbOfOmniscience].desc = "Increase defense resistance by 5%";
        relicDicts[TypeRelic.NecromancersGrasp].desc = "Increase damage/meter by 5%";
        relicDicts[TypeRelic.TwilightCrystal].desc = "Increase damage by 5%";
        relicDicts[TypeRelic.LunarCharm].desc = "Increase health by 5%";
        relicDicts[TypeRelic.DimensionalRift].desc = "Increase coins by 5%";
        relicDicts[TypeRelic.WhisperingRelic].desc = "Increase lab speed by 4%";
        relicDicts[TypeRelic.VeilOfTheUnknown].desc = "Increase critical factor by 5%";
        relicDicts[TypeRelic.HourglassOfOblivion].desc = "Increase defense resistance by 5%";
        relicDicts[TypeRelic.EldritchSigil].desc = "Increase tower health by 5%";
    }*/

}

[Serializable]
public class RelicData
{
    public TypeRelic typeRelic;
    public TypeRarityRelic typeRarityRelic;
    public float numberPercent;
    public Sprite icon;
    public string desc;

    public RelicData(TypeRelic typeRelic, TypeRarityRelic typeRarityRelic, float numberPercent, Sprite icon)
    {
        this.typeRelic = typeRelic;
        this.typeRarityRelic = typeRarityRelic;
        this.numberPercent = numberPercent;
        this.icon = icon;
    }
}
