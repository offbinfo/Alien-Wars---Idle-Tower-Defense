using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : SingletonGame<QuestManager>
{
    [SerializeField]
    private SO_DailyQuestManager SO_DailyQuestManager;
    private Dictionary<DailyQuestType, int> questProgress = new Dictionary<DailyQuestType, int>();

    protected override void Awake()
    {
        base.Awake();
        GameDatas.CheckAndDeleteJsonData();
    }

    private void Start()
    {
        RegisterEvents();
        OnResetDataQuest();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        foreach (DailyQuestType questType in Enum.GetValues(typeof(DailyQuestType)))
        {
            SubscribeEvent(questType, value => UpdateQuestProgress(questType, value));
        }
    }

    private void UnregisterEvents()
    {
        foreach (DailyQuestType questType in Enum.GetValues(typeof(DailyQuestType)))
        {
            UnsubscribeEvent(questType, value => UpdateQuestProgress(questType, value));
        }
    }

    private void SubscribeEvent(DailyQuestType type, Action<int> callback)
    {
        switch (type)
        {
            //case DailyQuestType.ArenaMatches: QuestEventManager.OnArenaJoined += callback; break;
            case DailyQuestType.StartBattle: QuestEventManager.OnBattleStarted += callback; break;
            case DailyQuestType.Upgrade: QuestEventManager.OnUpgrade += callback; break;
            case DailyQuestType.BuyCard: QuestEventManager.OnCardPurchased += callback; break;
            case DailyQuestType.DestroyFastEnemies: QuestEventManager.OnFastEnemiesKilled += callback; break;
            case DailyQuestType.SurviveWaves: QuestEventManager.OnWavesCleared += callback; break;
            case DailyQuestType.DestroyBoss: QuestEventManager.OnBossKilled += callback; break;
            case DailyQuestType.WatchAds: QuestEventManager.OnAdsWatched += callback; break;
            case DailyQuestType.ClaimFreeGems: QuestEventManager.OnFreeGemClaimed += callback; break;
            case DailyQuestType.DestroyShotEnemies: QuestEventManager.OnShotEnemiesKilled += callback; break;
            case DailyQuestType.DestroyTankEnemies: QuestEventManager.OnTankEnemiesKilled += callback; break;
            case DailyQuestType.DestroyNormalEnemies: QuestEventManager.OnNormalEnemiesKilled += callback; break;
            case DailyQuestType.CriticalHits: QuestEventManager.OnCriticalHits += callback; break;
            case DailyQuestType.UpgradeInBattle: QuestEventManager.OnUpgradedInBattle += callback; break;
            case DailyQuestType.ChangeSkin: QuestEventManager.OnSkinChanged += callback; break;
            case DailyQuestType.TotalWaves: QuestEventManager.OnTotalWavesCleared += callback; break;
        }
    }

    private void UnsubscribeEvent(DailyQuestType type, Action<int> callback)
    {
        switch (type)
        {
            //case DailyQuestType.ArenaMatches: QuestEventManager.OnArenaJoined -= callback; break;
            case DailyQuestType.StartBattle: QuestEventManager.OnBattleStarted -= callback; break;
            case DailyQuestType.Upgrade: QuestEventManager.OnUpgrade -= callback; break;
            case DailyQuestType.BuyCard: QuestEventManager.OnCardPurchased -= callback; break;
            case DailyQuestType.DestroyFastEnemies: QuestEventManager.OnFastEnemiesKilled -= callback; break;
            case DailyQuestType.SurviveWaves: QuestEventManager.OnWavesCleared -= callback; break;
            case DailyQuestType.DestroyBoss: QuestEventManager.OnBossKilled -= callback; break;
            case DailyQuestType.WatchAds: QuestEventManager.OnAdsWatched -= callback; break;
            case DailyQuestType.ClaimFreeGems: QuestEventManager.OnFreeGemClaimed -= callback; break;
            case DailyQuestType.DestroyShotEnemies: QuestEventManager.OnShotEnemiesKilled -= callback; break;
            case DailyQuestType.DestroyTankEnemies: QuestEventManager.OnTankEnemiesKilled -= callback; break;
            case DailyQuestType.DestroyNormalEnemies: QuestEventManager.OnNormalEnemiesKilled -= callback; break;
            case DailyQuestType.CriticalHits: QuestEventManager.OnCriticalHits -= callback; break;
            case DailyQuestType.UpgradeInBattle: QuestEventManager.OnUpgradedInBattle -= callback; break;
            case DailyQuestType.ChangeSkin: QuestEventManager.OnSkinChanged -= callback; break;
            case DailyQuestType.TotalWaves: QuestEventManager.OnTotalWavesCleared -= callback; break;
        }
    }

    private void UpdateQuestProgress(DailyQuestType questType, int value)
    {
        if(GameDatas.IsFirstTimeGoHome) return;
        if (!questProgress.ContainsKey(questType))
        {
            questProgress[questType] = GameDatas.GetTotalQuestDone(questType);
        }

        questProgress[questType] += value;

        GameDatas.SetTotalQuestDone(questType, questProgress[questType]);

        GameDatas.countDoneQuest++;
        EventDispatcher.PostEvent(EventID.OnRefreshClaimDailyGift, questType);
    }

    private void OnResetDataQuest()
    {
        string lastLogin = GameDatas.IsLastLoginDateDailyQuest();

        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastLogin != today)
        {
            DebugCustom.LogColor("Reset Quest");
            GameDatas.DeleteAllQuests();
            foreach (DailyQuestType questType in Enum.GetValues(typeof(DailyQuestType)))
            {
                GameDatas.SetTotalQuestDone(questType, 0);
                GameDatas.ClaimedQuestReward(questType, false);
            }
            foreach (DailyQuestSpecialType questType in Enum.GetValues(typeof(DailyQuestSpecialType)))
            {
                GameDatas.SetTotalQuestDone(questType, 0);
                GameDatas.ClaimedQuestReward(questType, false);
            }
            SetQuestByDay();

            GameDatas.LastLoginDateDailyQuest(today);
        }
    }

/*    [Button("Test")]
    public void Test()
    {
        List<DailyQuestType> dailyQuestTypes = SO_DailyQuestManager.GetRandomEnumValues<DailyQuestType>(5);
        DebugCustom.LogColor(dailyQuestTypes.Count);
        for (int i = 0; i < dailyQuestTypes.Count; i++)
        {
            DebugCustom.LogColor(dailyQuestTypes[i]);
        }
    }*/

    private void SetQuestByDay()
    {
        List<DailyQuestType> dailyQuestTypes = SO_DailyQuestManager.GetRandomEnumValues<DailyQuestType>(5);
        for (int i = 0; i < dailyQuestTypes.Count; i++)
        {
            int countQuest = SO_DailyQuestManager.GetRandomDailyQuestType(dailyQuestTypes[i]);
            ItemDailyQuestData itemDailyQuestData = new(countQuest, dailyQuestType: dailyQuestTypes[i]);
            GameDatas.SaveAllQuests(itemDailyQuestData);
        }
    }
}

public static class QuestEventManager
{
    //public static event Action<int> OnArenaJoined;
    public static event Action<int> OnBattleStarted;
    public static event Action<int> OnUpgrade;
    public static event Action<int> OnCardPurchased;
    public static event Action<int> OnFastEnemiesKilled;
    public static event Action<int> OnWavesCleared;
    public static event Action<int> OnBossKilled;
    public static event Action<int> OnAdsWatched;
    public static event Action<int> OnFreeGemClaimed;
    public static event Action<int> OnShotEnemiesKilled;
    public static event Action<int> OnTankEnemiesKilled;
    public static event Action<int> OnNormalEnemiesKilled;
    public static event Action<int> OnCriticalHits;
    public static event Action<int> OnUpgradedInBattle;
    public static event Action<int> OnSkinChanged;
    public static event Action<int> OnTotalWavesCleared;

    // Gọi sự kiện để cập nhật tiến trình nhiệm vụ
    //public static void ArenaJoined(int count) => OnArenaJoined?.Invoke(count);
    public static void BattleStarted(int count) => OnBattleStarted?.Invoke(count);
    public static void Upgrade(int count) => OnUpgrade?.Invoke(count);
    public static void CardPurchased(int count) => OnCardPurchased?.Invoke(count);
    public static void FastEnemiesKilled(int count) => OnFastEnemiesKilled?.Invoke(count);
    public static void WavesCleared(int count) => OnWavesCleared?.Invoke(count);
    public static void BossKilled(int count) => OnBossKilled?.Invoke(count);
    public static void AdsWatched(int count) => OnAdsWatched?.Invoke(count);
    public static void FreeGemClaimed(int count) => OnFreeGemClaimed?.Invoke(count);
    public static void ShotEnemiesKilled(int count) => OnShotEnemiesKilled?.Invoke(count);
    public static void TankEnemiesKilled(int count) => OnTankEnemiesKilled?.Invoke(count);
    public static void NormalEnemiesKilled(int count) => OnNormalEnemiesKilled?.Invoke(count);
    public static void CriticalHits(int count) => OnCriticalHits?.Invoke(count);
    public static void UpgradedInBattle(int count) => OnUpgradedInBattle?.Invoke(count);
    public static void SkinChanged(int count) => OnSkinChanged?.Invoke(count);
    public static void TotalWavesCleared(int count) => OnTotalWavesCleared?.Invoke(count);
}

