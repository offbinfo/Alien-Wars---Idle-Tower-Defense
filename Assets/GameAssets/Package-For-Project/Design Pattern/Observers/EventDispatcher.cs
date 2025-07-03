using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum EventID
{
    None,

    OnChangedSkinTower,
    OnEnemyKilled,
    OnRefreshUIWave,
    OnBombExplode,
    OnResetData,
    OnBuyUpgrader,
    OnCallSatellite,
    UpgraderTowerInGame,
    UpgraderTowerBaseGame,
    OnBuyClusterUpgrader,
    OnRefreshUnlockSlotCard,
    OnUnlockCard,

    OnGoldChanged,
    OnGemChanged,
    OnPowerStoneChanged,
    OnTouramentChanged,
    OnBadgesChanged,
    OnArmorSphereChanged,
    OnLoopSong,
    OnPowerSphereChanged,
    OnEngineSphereChanged,
    OnCryogenicSphereChanged,

    OnSilverChanged_ingame,
    OnGoldChanged_ingame,
    OnGemChanged_ingame,

    OnNewCardUnlock,
    OnAmountSlotChanged,
    OnLevelCardChanged,
    OnAmountCardChanged,
    OnUnequipcard,
    OnEquipCard,
    OnSetChanged,

    OnRefreshUnlockSlotLab,
    OnRefeshUILabs,

    OnStartUpgradeSubject,
    OnUpgradeSubjectSuccessLab,
    OnUpgradeSubjectSuccess_SubjectId,
    OnChangeAmountRevive,

    OnUnlockUltimateWeapon,
    OnUnlockSlotUW,

    OnMissileDestroy,
    EnemyDestroyedByShockwave,
    ShockwaveBonusHealth,
    OnAddTimeBonusGold,

    OnRefeshUIHome,
    OnClaimAllItemReward,
    OnRefeshAvatarUnlock,
    OnRefeshUIUpgrade,

    OnTutorialPlayDemoEnd,
    OnRefreshDailyGift,
    OnRefreshClaimDailyGift,
    AdsChanged,
    OnChangedName,
    OnChangedAvatar,
    OnRefreshClaimEventChallenge,

    OnLevelUnlockCardChanged,
    OnChangedLanguage,
    OnUsingPremiumMileStones,
    FinishWave,
    OnChangedTabCard,
    OnRefreshDailyReward,
    OnRefreshTabInforGamePlay,
    XBonusGoldDailyQuest,
    OnRefreshTextWaveUI,
    OnUpgradeInGame,
    OnOnRefreshRankArena,
    OnOnRefreshUIRankArena,
    OnChangeCountBuyX,
    OnLifeBoxAfterBoss,
    OnClaimItemLifeBoxDrop,
    OnRefreshUICHangeLanguage,
    OnReviveTower,
    OnBeginnerQuestProgressChanged,
    OnBeginnerQuestClaim,
    OnClickBtnClaimBeginnerQuest,
    OnScaleMapByRangedTower,
    OnRefreshRelicEquip,
    OnChangeSongTheme,
    OnBotChallengeUnlock,
    OnX2BadgesEvent,
}

public class EventDispatcher
{
    [RuntimeInitializeOnLoadMethod]
    public static void Init ()
    {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private static void SceneManager_sceneUnloaded (Scene arg0)
    {
        if (arg0.buildIndex != 0) // Chỉ clear nếu scene bị unload KHÔNG phải scene 0
        {
            DebugCustom.LogColor("Clear event");
            listeners.Clear();
        }
        //listeners.Clear ();
    }

    static Dictionary<EventID, Action<object>> listeners = new Dictionary<EventID, Action<object>> ();

    public static void PostEvent (EventID eventID, object parameter)
    {
        if (listeners.ContainsKey (eventID))
        {
            listeners [eventID].Invoke (parameter);
        }
    }

    public static void AddEvent (EventID eventID, Action<object> action)
    {
        if (!listeners.ContainsKey (eventID))
            listeners [eventID] = action;
        else
            listeners [eventID] += action;
    }

    public static void RemoveEvent (EventID eventID, Action<object> action)
    {
        /*if (listeners.ContainsKey (eventID))
            listeners [eventID] -= action;*/
        if (listeners.ContainsKey(eventID))
        {
            listeners[eventID] -= action;
            if (listeners[eventID] == null) 
                listeners.Remove(eventID);
        }
    }
}
