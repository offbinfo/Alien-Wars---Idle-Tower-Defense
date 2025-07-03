using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateWeapon", menuName = "Data/UltimateWeapon/UW_PassiveSlow", order = 0)]
public class SO_UW_PassiveSlow : SO_UW_Base
{

    public Vector3 durationBonus;
    public Vector3 costDurationBonus;
    public int levelMaxDurationBonus;
    public Vector3 slow;
    public Vector3 costSlow;
    public int levelMaxSlow;
    public Vector3 cooldown;
    public Vector3 costCoolDown;
    public int levelMaxCooldown;

    public int levelDurationBonus => GameDatas.GetLevelUltimateDurationBonus(id);
    public int levelSlowBonus => GameDatas.GetLevelUltimateSlowBonus(id);
    public int levelCoolDown => GameDatas.GetLevelUltimateCooldown(id);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("duration_bonus"));
            names.Add(LanguageManager.GetText("slow_bonus"));
            names.Add(LanguageManager.GetText("cooldown"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelDurationBonus);
            list.Add(levelSlowBonus);
            list.Add(levelCoolDown);
            return list;
        }
    }

    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxDurationBonus);
            levelsMax.Add(levelMaxSlow);
            levelsMax.Add(levelMaxCooldown);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetDurationBonus(levelDurationBonus + levelPlus));
        properties.Add(GetSlowBonus(levelSlowBonus + levelPlus));
        properties.Add(GetCooldown(levelCoolDown + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetCostDurationBonus(levelDurationBonus + levelPlus));
        prices.Add(GetCostSlowBonus(levelSlowBonus + levelPlus));
        prices.Add(GetCostCooldown(levelCoolDown + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeDurationBonus);
        actions.Add(UpgradeSlowBonus);
        actions.Add(UpgradeCooldown);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("s");
            list.Add("s");
            list.Add("s");
            return list;
        }
    }

    public float GetDurationBonus(int level)
    {
        return durationBonus.QuadraticEquation(level);
    }
    public int GetCostDurationBonus(int level)
    {
        return (int)costDurationBonus.QuadraticEquation(level);
    }

    public float GetSlowBonus(int level)
    {
        return slow.QuadraticEquation(level);
    }
    public int GetCostSlowBonus(int level)
    {
        return (int)costSlow.QuadraticEquation(level);
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
    public float GetCurrentDuration()
    {
        return GetDurationBonus(levelDurationBonus);
    }
    public int GetCurrentCostDurationBonus()
    {
        return GetCostDurationBonus(levelDurationBonus);
    }

    public float GetCurrentSlowChange()
    {
        return GetSlowBonus(levelDurationBonus);
    }
    public int GetCurrentCostSlowChange()
    {
        return GetCostSlowBonus(levelDurationBonus);
    }

    public int GetCurrentCooldown()
    {
        return GetCooldown(levelCoolDown);
    }
    public int GetCurrentCostCoolDown()
    {
        return GetCostCooldown(levelCoolDown);
    }

    public void UpgradeDurationBonus()
    {
        GameDatas.SetLevelUltimateDurationBonus(id, levelDurationBonus + 1);
    }

    public void UpgradeSlowBonus()
    {
        GameDatas.SetLevelUltimateSlowBonus(id, levelSlowBonus + 1);
    }

    public void UpgradeCooldown()
    {
        GameDatas.SetLevelUltimateCooldown(id, levelCoolDown + 1);
    }
}
