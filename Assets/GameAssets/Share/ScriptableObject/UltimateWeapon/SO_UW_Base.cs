using language;
using ProjectTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//CreateAssetMenu(fileName = "UWData")]
public class SO_UW_Base : ScriptableObject
{
    public UW_ID id;
    public bool isUnlock => GameDatas.isUnlockUltimateWeapon(id);
    public Vector3 quantity;
    public Vector3 costQuantity;
    public int levelMaxQuantity; 
    public int levelQuantity => GameDatas.GetLevelUltimateQuantity(id);
    public string Name => LanguageManager.GetText(id + "_name");
    public string Description
    {
        get
        {
            var list_stat = GetStatProperty();
            if(list_stat.Count > 3)
            {
                return string.Format(LanguageManager.GetText(id + "_des"), list_stat[3], list_stat[1]);
            }
            return string.Format(LanguageManager.GetText(id + "_des"), list_stat[0], list_stat[1]);
        }
    }
    public Sprite iconUW;

    public virtual List<string> statName
    {
        get
        {
            List<string> name = new List<string>();
            name.Add(LanguageManager.GetText("quantity"));
            return name;
        }
    }
    public virtual List<int> Level
    {
        get
        {
            var list = new List<int>();
            list.Add(levelQuantity);
            return list;
        }
    }
    public virtual List<int> LevelMax
    {
        get
        {
            var levelsMax = new List<int>();
            levelsMax.Add(levelMaxQuantity);
            return levelsMax;
        }
    }
    public virtual List<object> GetStatProperty(int levelPlus = 0)
    {
        List<object> stats = new List<object>();
        stats.Add(GetQuantity(levelQuantity + levelPlus));
        return stats;
    }
    public virtual List<int> GetPrice(int levelPlus = 0)
    {
        List<int> prices = new List<int>();
        prices.Add(GetCostQuantity(levelQuantity + levelPlus));
        return prices;
    }
    public virtual List<Action> GetActionUpgrade()
    {
        var actions = new List<Action>();
        actions.Add(UpgradeQuantity);
        return actions;
    }
    public virtual List<string> GetFormat
    {
        get
        {
            var formats = new List<string>();
            formats.Add("");
            return formats;
        }
    }

    public int GetQuantity(int level)
    {
        return (int)quantity.QuadraticEquation(level);
    }
    public int GetCostQuantity(int level)
    {
        return (int)costQuantity.QuadraticEquation(level);
    }
    public int GetCurrentQuantity()
    {
        return (int)quantity.QuadraticEquation(levelQuantity);
    }
    public int GetCurrentCostQuantity()
    {
        return (int)costQuantity.QuadraticEquation(levelQuantity);
    }
    public void UnlockThisUW()
    {
        GameDatas.SetUnlockUltimateWeapon(id, true);
        EventDispatcher.PostEvent(EventID.OnUnlockUltimateWeapon, id);
    }
    public void UpgradeQuantity()
    {
        GameDatas.SetLevelUltimateQuantity(id, levelQuantity + 1);
    }
}
