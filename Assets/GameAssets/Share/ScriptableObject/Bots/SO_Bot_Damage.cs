using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "Bots", menuName = "Data/Bots/SO_Bot_Damage", order = 0)]
public class SO_Bot_Damage : SO_Bot_Base
{
    public Vector3 damageBonus;
    public int levelMaxDamageBonus;
    public Vector3 damageReduction;
    public int levelMaxDamageReduction;

    public int levelDamageBonus => GameDatas.GetLevelDamageBot(typeBot);
    public int levelDamageReduction => GameDatas.GetLevelDamageReductionBot(typeBot);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("dmg_bonus"));
            names.Add(LanguageManager.GetText("damage_reduction"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelDamageBonus);
            list.Add(levelDamageReduction);
            return list;
        }
    }
    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxDamageBonus);
            levelsMax.Add(levelMaxDamageReduction);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetDmgBonusBot(levelDamageBonus + levelPlus));
        properties.Add(GetDmgReductionBot(levelDamageReduction + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetDmgBonusBot(levelDamageBonus + levelPlus));
        prices.Add(GetDmgReductionBot(levelDamageReduction + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeDmgBonusBot);
        actions.Add(UpgradeDmgReductionBot);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("x");
            list.Add("%");
            return list;
        }
    }

    public int GetDmgBonusBot(int level)
    {
        return (int)damageBonus.QuadraticEquation(level);
    }

    public int GetDmgReductionBot(int level)
    {
        return (int)damageReduction.QuadraticEquation(level);
    }

    //Get current
    public int GetCurrentDmgBonus()
    {
        return GetDmgBonusBot(levelDamageBonus);
    }

    public int GetCurrentDmgReduction()
    {
        return GetDmgReductionBot(levelDamageReduction);
    }

    public void UpgradeDmgBonusBot()
    {
        GameDatas.SetLevelDamageBot(typeBot, levelDamageBonus + 1);
    }
    public void UpgradeDmgReductionBot()
    {
        GameDatas.SetLevelDamageReductionBot(typeBot, levelDamageReduction + 1);
    }
}
