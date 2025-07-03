using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bots", menuName = "Data/Bots/SO_Bot_Shock", order = 0)]
public class SO_Bot_Shock : SO_Bot_Base
{
    public Vector3 shockChange;
    public int levelMaxShockChange;

    public int levelShockChange=> GameDatas.GetLevelShockChangeBot(typeBot);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("shock_change"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelShockChange);
            return list;
        }
    }
    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxShockChange);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetShockChange(levelShockChange + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetShockChange(levelShockChange + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeSHockChange);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("%");
            return list;
        }
    }

    public int GetShockChange(int level)
    {
        return (int)shockChange.QuadraticEquation(level);
    }

    //Get current
    public int GetCurrentShockChange()
    {
        return GetShockChange(levelShockChange);
    }

    public void UpgradeSHockChange()
    {
        GameDatas.SetLevelShockChangeBot(typeBot, levelShockChange + 1);
    }
}
