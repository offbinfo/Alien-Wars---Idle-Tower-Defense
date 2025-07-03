using language;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Bot_Base : ScriptableObject
{
    
    public TypeBot typeBot;
    public bool isUnlock => GameDatas.IsBotChallengeUnlock(typeBot);
    public Vector3 coolDown;
    public int levelMaxCoolDown;
    public Vector3 range;
    public int levelMaxRange;
    public Vector3 duration;
    public int levelMaxDuration;
    public Vector3 costBadges;

    public int levelCoolDown => GameDatas.GetLevelCoolDownBot(typeBot);
    public int levelRange => GameDatas.GetLevelRangeBot(typeBot);
    public int levelDuration => GameDatas.GetLevelDurationBot(typeBot);
    public string Name => LanguageManager.GetText(typeBot + "_name");
    public string Description
    {
        get
        {
            var list_stat = GetStatProperty();
            if (list_stat.Count > 3)
            {
                return string.Format(LanguageManager.GetText(typeBot + "_des"), list_stat[3], list_stat[1]);
            }
            return string.Format(LanguageManager.GetText(typeBot + "_des"), list_stat[0], list_stat[1]);
        }
    }
    public Sprite iconUW;

    public virtual List<string> statName
    {
        get
        {
            List<string> name = new List<string>();
            name.Add(LanguageManager.GetText("duration_bonus"));
            name.Add(LanguageManager.GetText("cooldown"));
            name.Add(LanguageManager.GetText("range_bonus"));
            return name;
        }
    }
    public virtual List<int> Level
    {
        get
        {
            var list = new List<int>();
            list.Add(levelDuration);
            list.Add(levelCoolDown);
            list.Add(levelRange);
            return list;
        }
    }
    public virtual List<int> LevelMax
    {
        get
        {
            var levelsMax = new List<int>();
            levelsMax.Add(levelMaxDuration);
            levelsMax.Add(levelMaxCoolDown);
            levelsMax.Add(levelMaxRange);
            return levelsMax;
        }
    }
    public virtual List<object> GetStatProperty(int levelPlus = 0)
    {
        List<object> stats = new List<object>();
        stats.Add(GetDuration(levelDuration + levelPlus));
        stats.Add(GetCoolDown(levelCoolDown + levelPlus));
        stats.Add(GetRange(levelRange + levelPlus));
        return stats;
    }
    public virtual List<int> GetPrice(int levelPlus = 0)
    {
        List<int> prices = new List<int>();
        prices.Add(GetDuration(levelDuration + levelPlus));
        prices.Add(GetCoolDown(levelCoolDown + levelPlus));
        prices.Add(GetRange(levelRange + levelPlus));
        return prices;
    }
    public virtual List<Action> GetActionUpgrade()
    {
        var actions = new List<Action>();
        actions.Add(UpgradeDuration);
        actions.Add(UpgradeCoolDown);
        actions.Add(UpgradeRange);
        return actions;
    }
    public virtual List<string> GetFormat
    {
        get
        {
            var formats = new List<string>();
            formats.Add("s");
            formats.Add("s");
            formats.Add("");
            return formats;
        }
    }

    public int GetDuration(int level)
    {
        return (int)duration.QuadraticEquation(level);
    }
    public int GetCurrentDuration()
    {
        return (int)duration.QuadraticEquation(levelDuration);
    }

    public int GetCoolDown(int level)
    {
        return (int)coolDown.QuadraticEquation(level);
    }
    public int GetCurrentCoolDown()
    {
        return (int)coolDown.QuadraticEquation(levelCoolDown);
    }

    public int GetRange(int level)
    {
        return (int)range.QuadraticEquation(level);
    }
    public int GetCurrentRange()
    {
        return (int)range.QuadraticEquation(levelRange);
    }
    public void UpgradeCoolDown()
    {
        GameDatas.SetLevelCoolDownBot(typeBot, levelCoolDown + 1);
    }

    public void UpgradeRange()
    {
        GameDatas.SetLevelRangeBot(typeBot, levelRange + 1);
    }
    public void UpgradeDuration()
    {
        GameDatas.SetLevelDurationBot(typeBot, levelDuration + 1);
    }
}
