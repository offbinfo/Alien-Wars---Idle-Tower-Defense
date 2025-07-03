using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateWeapon", menuName = "Data/UltimateWeapon/UW_Passive", order = 0)]
public class SO_UW_Passive : SO_UW_Base
{
    public Vector3 dmgScale;
    public Vector3 costDmgScale;
    public int levelMaxDmgScale;
    public Vector3 angle;
    public Vector3 costAngle;
    public int levelMaxAngle;

    public int levelDmgScale => GameDatas.GetLevelUltimateDmgScale(id);
    public int levelAngle => GameDatas.GetLevelUltimateAngle(id);

    public override List<string> statName
    {
        get
        {
            var names = base.statName;
            names.Add(LanguageManager.GetText("dmg_scale"));
            names.Add(LanguageManager.GetText("angle"));
            return names;
        }
    }
    public override List<int> Level
    {
        get
        {
            var list = base.Level;
            list.Add(levelDmgScale);
            list.Add(levelAngle);
            return list;
        }
    }
    public override List<int> LevelMax
    {
        get
        {
            var levelsMax = base.LevelMax;
            levelsMax.Add(levelMaxDmgScale);
            levelsMax.Add(levelMaxAngle);
            return levelsMax;
        }
    }

    public override List<object> GetStatProperty(int levelPlus = 0)
    {
        var properties = base.GetStatProperty(levelPlus);
        properties.Add(GetDmgScale(levelDmgScale + levelPlus));
        properties.Add(GetAngle(levelAngle + levelPlus));
        return properties;
    }
    public override List<int> GetPrice(int levelPlus = 0)
    {
        var prices = base.GetPrice(levelPlus);
        prices.Add(GetCostDmgScale(levelDmgScale + levelPlus));
        prices.Add(GetCostAngle(levelAngle + levelPlus));
        return prices;
    }
    public override List<Action> GetActionUpgrade()
    {
        var actions = base.GetActionUpgrade();
        actions.Add(UpgradeDmgScale);
        actions.Add(UpgradeAngle);
        return actions;
    }
    public override List<string> GetFormat
    {
        get
        {
            var list = base.GetFormat;
            list.Add("x");
            list.Add("");
            return list;
        }
    }

    public float GetDmgScale(int level)
    {
        return dmgScale.QuadraticEquation(level);
    }
    public int GetCostDmgScale(int level)
    {
        return (int)costDmgScale.QuadraticEquation(level);
    }
    public int GetAngle(int level)
    {
        return (int)angle.QuadraticEquation(level);
    }
    public int GetCostAngle(int level)
    {
        return (int)costAngle.QuadraticEquation(level);
    }


    //Get current
    public float GetCurrentDmgScale()
    {
        return GetDmgScale(levelDmgScale);
    }
    public int GetCurrentCostDmgScale()
    {
        return GetCostDmgScale(levelDmgScale);
    }
    public int GetCurrentAngle()
    {
        return GetAngle(levelAngle);
    }
    public int GetCurrentCostAngle()
    {
        return GetCostAngle(levelAngle);
    }

    public void UpgradeDmgScale()
    {
        GameDatas.SetLevelUltimateDmgScale(id, levelDmgScale + 1);
    }
    public void UpgradeAngle()
    {
        GameDatas.SetLevelUltimateAngle(id, levelAngle + 1);
    }
}
