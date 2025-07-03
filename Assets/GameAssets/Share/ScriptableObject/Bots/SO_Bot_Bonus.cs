using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bots", menuName = "Data/Bots/SO_Bot_Bonus", order = 0)]
public class SO_Bot_Bonus : SO_Bot_Base
{
    public Vector3 bonus;
    public int levelMaxBonus;

    public int levelbonus => GameDatas.GetLevelBonusBot(typeBot);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("bonus"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelbonus);
            return list;
        }
    }
    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxBonus);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetBonusBot(levelbonus + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetBonusBot(levelbonus + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeBonusBot);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("x");
            return list;
        }
    }

    public int GetBonusBot(int level)
    {
        return (int)bonus.QuadraticEquation(level);
    }

    //Get current
    public int GetCurrentBonus()
    {
        return GetBonusBot(levelbonus);
    }

    public void UpgradeBonusBot()
    {
        GameDatas.SetLevelBonusBot(typeBot, levelbonus + 1);
    }
}
