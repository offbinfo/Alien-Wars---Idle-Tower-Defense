using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInforCtrl : GameMonoBehaviour
{
    [SerializeField]
    private SO_CardManager so_CardManager;
    public SO_CardManager SO_CardManager => so_CardManager;
    public int currentIndexSet;
    private static readonly Dictionary<UpgraderID, CardID> upgraderToCardMap = new()
    {
        { UpgraderID.attack_damage, CardID.COMMON_DAMAGE },
        { UpgraderID.health, CardID.COMMON_HEALTH },
        { UpgraderID.generation, CardID.COMMON_HP_REGEN },
        { UpgraderID.bomb_area, CardID.RARE_BOMB_RANGE },
        { UpgraderID.attack_speed, CardID.EPIC_ATK_SPD },
        { UpgraderID.critical_shot_damage, CardID.EPIC_CRITICAL_DAMAGE },
        { UpgraderID.discount_of_coin_price, CardID.EPIC_REDUCE_PRICE },
        { UpgraderID.critical_shot_chance, CardID.DIVINE_CRITICAL_CHANCE }
    };

    public float GetCardStatBonus(UpgraderID upgraderID)
    {
        if (upgraderToCardMap.TryGetValue(upgraderID, out CardID cardID))
        {
            return Cfg.cardCtrl.GetCurrentStat(cardID);
        }
        return 0f;
    }

    #region Card Tower

    public bool IsCardUnlock(CardID cardID)
    {
        return false;
    }

    public float GetCurrentStat(CardID id)
    {
        try
        {
            if (!GameDatas.IsCardEquiped(id)) return 0;
        }
        catch
        {
            return 0;
        }

        return SO_CardManager.cardInforDataDict[id].GetCurrentStat();
    }
    public Card_SO GetCard(CardID id)
    {
        return SO_CardManager.cardInforDataDict[id];
    }
    #endregion
}
