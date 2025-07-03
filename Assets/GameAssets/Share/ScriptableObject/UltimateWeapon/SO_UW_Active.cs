using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateWeapon", menuName = "Data/UltimateWeapon/UW_Active", order = 0)]
public class SO_UW_Active : SO_UW_Base
{

    public Vector3 dmgBonus;
    public Vector3 costDmgBonus;
    public int levelMaxDmgBonus;
    public Vector3 cooldown;
    public Vector3 costCoolDown;
    public int levelMaxCooldown;

    public int levelDmgBonus => GameDatas.GetLevelUltimateDmgBonus(id);
    public int levelCoolDown => GameDatas.GetLevelUltimateCooldown(id);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("dmg_bonus"));
            names.Add(LanguageManager.GetText("cooldown"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelDmgBonus);
            list.Add(levelCoolDown);
            return list;
        }
    }

    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxDmgBonus);
            levelsMax.Add(levelMaxCooldown);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetDmgBonus(levelDmgBonus + levelPlus));
        properties.Add(GetCooldown(levelCoolDown + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetCostDmgBonus(levelDmgBonus + levelPlus));
        prices.Add(GetCostCooldown(levelCoolDown + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeDmgBonus);
        actions.Add(UpgradeCooldown);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("x");
            list.Add("s");
            return list;
        }
    }

    public float GetDmgBonus(int level)
    {
        return dmgBonus.QuadraticEquation(level);
    }
    public int GetCostDmgBonus(int level)
    {
        return (int)costDmgBonus.QuadraticEquation(level);
    }
    public int GetCooldown(int level)
    {
        return (int)cooldown.QuadraticEquation(level);
    }
    public int GetCostCooldown(int level)
    {
        return (int)costCoolDown.QuadraticEquation(level);
    }

    //Get current
    public float GetCurrentDmg()
    {
        return GetDmgBonus(levelDmgBonus);
    }
    public int GetCurrentCostDmgBonus()
    {
        return GetCostDmgBonus(levelDmgBonus);
    }
    public int GetCurrentCooldown()
    {
        return GetCooldown(levelCoolDown);
    }
    public int GetCurrentCostCoolDown()
    {
        return GetCostCooldown(levelCoolDown);
    }
    public void UpgradeDmgBonus()
    {
        GameDatas.SetLevelUltimateDmgBonus(id, levelDmgBonus + 1);
    }
    public void UpgradeCooldown()
    {
        GameDatas.SetLevelUltimateCooldown(id, levelCoolDown + 1);
    }
}
