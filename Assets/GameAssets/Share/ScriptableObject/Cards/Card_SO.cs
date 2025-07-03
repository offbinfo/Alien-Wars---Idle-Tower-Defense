using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Data/Cards/Card", order = 0)]
public class Card_SO : ScriptableObject
{
    public TypeCard type;
    public CardID id;
    public Sprite icon;
    public List<float> stats;
    public string titleName;
    public string describe;
    public Format format;
    public int currentlevel
    {
        get
        {
            return GameDatas.GetLevelCard(id);
        }
    }
    public bool isFullCard => isMaxLevel || amountCard >= (stats.Count - currentlevel - 1) * 5;

    public int amountCard
    {
        get
        {
            return GameDatas.GetAmountCard(id);
        }
        set
        {


            GameDatas.SetAmountCard(id, value);

            if (isFullCard)
            {
                var amountMax = (stats.Count - currentlevel - 1) * 5;
                GameDatas.SetAmountCard(id, amountMax);
                // return;
            }

            EventDispatcher.PostEvent(EventID.OnAmountCardChanged, id);
        }
    }
    public bool isMaxLevel => currentlevel == stats.Count - 1;

    public void Upgrade()
    {
        if (isMaxLevel) return;
        if (amountCard != 0)
        {
            amountCard -= 5;
        }
        GameDatas.UpgradeCard(id, currentlevel + 1);

        EventDispatcher.PostEvent(EventID.OnLevelCardChanged, id);
        QuestEventManager.Upgrade(1);
    }

    public float GetCurrentStat()
    {
        return isUnlock ? stats[currentlevel] : 0;
    }
    public float GetNextStat()
    {
        if (currentlevel + 1 > stats.Count - 1) return 0;
        return stats[currentlevel + 1];
    }

    public bool isUnlock
    {
        get
        {
            return GameDatas.IsUnlockCard(id);
        }
        set
        {
            
        }
    }

}
