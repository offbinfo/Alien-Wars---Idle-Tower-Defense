using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChallengeManager : SingletonGame<EventChallengeManager>
{

    public Dictionary<MissionChallengeType, int> missionProgress = new Dictionary<MissionChallengeType, int>();

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        foreach (MissionChallengeType missionType in Enum.GetValues(typeof(MissionChallengeType)))
        {
            SubscribeEvent(missionType, value => UpdateEventChallengeProgress(missionType, value));
        }
    }

    private void UnregisterEvents()
    {
        foreach (MissionChallengeType missionType in Enum.GetValues(typeof(MissionChallengeType)))
        {
            UnsubscribeEvent(missionType, value => UpdateEventChallengeProgress(missionType, value));
        }
    }

    private void SubscribeEvent(MissionChallengeType type, Action<int> callback)
    {
        switch (type)
        {
            case MissionChallengeType.LoginDays: EventChallengeListenerManager.OnLoginDays += callback; break;
            case MissionChallengeType.UpgradeLabTotalDays: EventChallengeListenerManager.OnUpgradeLabTotalDays += callback; break;
            case MissionChallengeType.ClaimFreeGemTimes: EventChallengeListenerManager.OnFreeGemClaimed += callback; break;
            case MissionChallengeType.NoDamageAfterWave: EventChallengeListenerManager.OnNoDamageAfterWave += callback; break;
            case MissionChallengeType.BuyCards: EventChallengeListenerManager.OnCardPurchased += callback; break;
            case MissionChallengeType.PlayWaveWithoutCard: EventChallengeListenerManager.OnPlayWaveWithoutCard += callback; break;
            case MissionChallengeType.CompleteDailyMissions: EventChallengeListenerManager.OnCompleteDailyMissions += callback; break;
            case MissionChallengeType.UpgradeWorkshop: EventChallengeListenerManager.OnUpgrade += callback; break;
            case MissionChallengeType.DealCriticalDamageEnemies: EventChallengeListenerManager.OnCriticalHits += callback; break;
            case MissionChallengeType.ReachWaveAnyWorld: EventChallengeListenerManager.OnTotalWavesCleared += callback; break;
            case MissionChallengeType.KillFastEnemies: EventChallengeListenerManager.OnFastEnemiesKilled += callback; break;
            case MissionChallengeType.KillBosses: EventChallengeListenerManager.OnBossKilled += callback; break;
            case MissionChallengeType.KillBossesOutOfRange: EventChallengeListenerManager.OnKillBossesOutOfRange += callback; break;
            case MissionChallengeType.KillNormalEnemies: EventChallengeListenerManager.OnNormalEnemiesKilled += callback; break;
            case MissionChallengeType.KillTankEnemies: EventChallengeListenerManager.OnTankEnemiesKilled += callback; break;
            case MissionChallengeType.KillRangeEnemies: EventChallengeListenerManager.OnKillRangeEnemies += callback; break;
            case MissionChallengeType.KillWithTomahawk: EventChallengeListenerManager.OnKillWithTomahawk += callback; break;
            case MissionChallengeType.KillWithShockwave: EventChallengeListenerManager.OnKillWithShockwave += callback; break;
            case MissionChallengeType.KillInHighlight: EventChallengeListenerManager.OnKillInHighlight += callback; break;
            case MissionChallengeType.EnemiesInVoidNexus: EventChallengeListenerManager.OnEnemiesInVoidNexus += callback; break;
            case MissionChallengeType.KillDuringGoldenSanctuary: EventChallengeListenerManager.OnKillDuringGoldenSanctuary += callback; break;
            case MissionChallengeType.KillWithMine: EventChallengeListenerManager.OnKillWithMine += callback; break;
            case MissionChallengeType.KillWithSatellite: EventChallengeListenerManager.OnKillWithSatellite += callback; break;
            case MissionChallengeType.SkipWaves: EventChallengeListenerManager.OnSkipWaves += callback; break;
            case MissionChallengeType.PlayForHours: EventChallengeListenerManager.OnPlayForHours += callback; break;
            case MissionChallengeType.PlayArenaTimes:
                EventChallengeListenerManager.OnPlayArenaTimes += callback;
                break;
            case MissionChallengeType.WatchVideoTimes:
                EventChallengeListenerManager.OnAdsWatched += callback;
                break;
        }
    }

    private void UnsubscribeEvent(MissionChallengeType type, Action<int> callback)
    {
        switch (type)
        {
            case MissionChallengeType.LoginDays: EventChallengeListenerManager.OnLoginDays -= callback; break;
            case MissionChallengeType.UpgradeLabTotalDays: EventChallengeListenerManager.OnUpgradeLabTotalDays -= callback; break;
            case MissionChallengeType.ClaimFreeGemTimes: EventChallengeListenerManager.OnFreeGemClaimed -= callback; break;
            case MissionChallengeType.NoDamageAfterWave: EventChallengeListenerManager.OnNoDamageAfterWave -= callback; break;
            case MissionChallengeType.BuyCards: EventChallengeListenerManager.OnCardPurchased -= callback; break;
            case MissionChallengeType.PlayWaveWithoutCard: EventChallengeListenerManager.OnPlayWaveWithoutCard -= callback; break;
            case MissionChallengeType.CompleteDailyMissions: EventChallengeListenerManager.OnCompleteDailyMissions -= callback; break;
            case MissionChallengeType.UpgradeWorkshop: EventChallengeListenerManager.OnUpgrade -= callback; break;
            case MissionChallengeType.DealCriticalDamageEnemies: EventChallengeListenerManager.OnCriticalHits -= callback; break;
            case MissionChallengeType.ReachWaveAnyWorld: EventChallengeListenerManager.OnTotalWavesCleared -= callback; break;
            case MissionChallengeType.KillFastEnemies: EventChallengeListenerManager.OnFastEnemiesKilled -= callback; break;
            case MissionChallengeType.KillBosses: EventChallengeListenerManager.OnBossKilled -= callback; break;
            case MissionChallengeType.KillBossesOutOfRange: EventChallengeListenerManager.OnKillBossesOutOfRange -= callback; break;
            case MissionChallengeType.KillNormalEnemies: EventChallengeListenerManager.OnNormalEnemiesKilled -= callback; break;
            case MissionChallengeType.KillTankEnemies: EventChallengeListenerManager.OnTankEnemiesKilled -= callback; break;
            case MissionChallengeType.KillRangeEnemies: EventChallengeListenerManager.OnKillRangeEnemies -= callback; break;
            case MissionChallengeType.KillWithTomahawk: EventChallengeListenerManager.OnKillWithTomahawk -= callback; break;
            case MissionChallengeType.KillWithShockwave: EventChallengeListenerManager.OnKillWithShockwave -= callback; break;
            case MissionChallengeType.KillInHighlight: EventChallengeListenerManager.OnKillInHighlight -= callback; break;
            case MissionChallengeType.EnemiesInVoidNexus: EventChallengeListenerManager.OnEnemiesInVoidNexus -= callback; break;
            case MissionChallengeType.KillDuringGoldenSanctuary: EventChallengeListenerManager.OnKillDuringGoldenSanctuary -= callback; break;
            case MissionChallengeType.KillWithMine: EventChallengeListenerManager.OnKillWithMine -= callback; break;
            case MissionChallengeType.KillWithSatellite: EventChallengeListenerManager.OnKillWithSatellite -= callback; break;
            case MissionChallengeType.SkipWaves: EventChallengeListenerManager.OnSkipWaves -= callback; break;
            case MissionChallengeType.PlayForHours: EventChallengeListenerManager.OnPlayForHours -= callback; break;
            case MissionChallengeType.PlayArenaTimes:
                EventChallengeListenerManager.OnPlayArenaTimes -= callback;
                break;
            case MissionChallengeType.WatchVideoTimes:
                EventChallengeListenerManager.OnAdsWatched -= callback;
                break;
        }
    }


    private void UpdateEventChallengeProgress(MissionChallengeType missionType, int value)
    {
        if (GameDatas.IsFirstTimeGoHome) return;
        if (!missionProgress.ContainsKey(missionType))
        {
            missionProgress[missionType] = GameDatas.GetProgressMissionChallenge(missionType);
        }

        if(GameDatas.GetProgressMissionChallenge(missionType) == 0)
        {
            missionProgress[missionType] = 0;
        }

        missionProgress[missionType] += value;

        GameDatas.SetProgressMissionChallenge(missionType, missionProgress[missionType]);

        EventDispatcher.PostEvent(EventID.OnRefreshClaimEventChallenge, missionType);
    }

}

public static class EventChallengeListenerManager
{
    public static event Action<int> OnPlayArenaTimes;
    public static event Action<int> OnUpgrade;
    public static event Action<int> OnCardPurchased;
    public static event Action<int> OnFastEnemiesKilled;
    public static event Action<int> OnBossKilled;
    public static event Action<int> OnAdsWatched;
    public static event Action<int> OnFreeGemClaimed;
    public static event Action<int> OnShotEnemiesKilled;
    public static event Action<int> OnTankEnemiesKilled;
    public static event Action<int> OnNormalEnemiesKilled;
    public static event Action<int> OnCriticalHits;
    public static event Action<int> OnTotalWavesCleared;

    public static event Action<int> OnLoginDays;
    public static event Action<int> OnUpgradeLabTotalDays;
    public static event Action<int> OnNoDamageAfterWave;
    public static event Action<int> OnPlayWaveWithoutCard;
    public static event Action<int> OnCompleteDailyMissions;
    public static event Action<int> OnKillBossesOutOfRange;
    public static event Action<int> OnKillRangeEnemies;
    public static event Action<int> OnKillWithTomahawk;
    public static event Action<int> OnKillWithShockwave;
    public static event Action<int> OnKillInHighlight;
    public static event Action<int> OnEnemiesInVoidNexus;
    public static event Action<int> OnKillDuringGoldenSanctuary;
    public static event Action<int> OnKillWithMine;
    public static event Action<int> OnKillWithSatellite;
    public static event Action<int> OnSkipWaves;
    public static event Action<int> OnPlayForHours;

    public static void PlayArenaTimes(int count) => OnPlayArenaTimes?.Invoke(count);
    public static void Upgrade(int count) => OnUpgrade?.Invoke(count);
    public static void CardPurchased(int count) => OnCardPurchased?.Invoke(count);
    public static void FastEnemiesKilled(int count) => OnFastEnemiesKilled?.Invoke(count);
    public static void BossKilled(int count) => OnBossKilled?.Invoke(count);
    public static void AdsWatched(int count) => OnAdsWatched?.Invoke(count);
    public static void FreeGemClaimed(int count) => OnFreeGemClaimed?.Invoke(count);
    public static void TankEnemiesKilled(int count) => OnTankEnemiesKilled?.Invoke(count);
    public static void NormalEnemiesKilled(int count) => OnNormalEnemiesKilled?.Invoke(count);
    public static void CriticalHits(int count) => OnCriticalHits?.Invoke(count);
    public static void TotalWavesCleared(int count) => OnTotalWavesCleared?.Invoke(count);
    public static void LoginDays(int count) => OnLoginDays?.Invoke(count);
    public static void UpgradeLabTotalDays(int count) => OnUpgradeLabTotalDays?.Invoke(count);
    public static void NoDamageAfterWave(int count) => OnNoDamageAfterWave?.Invoke(count);
    public static void PlayWaveWithoutCard(int count) => OnPlayWaveWithoutCard?.Invoke(count);
    public static void CompleteDailyMissions(int count) => OnCompleteDailyMissions?.Invoke(count);
    public static void KillBossesOutOfRange(int count) => OnKillBossesOutOfRange?.Invoke(count);
    public static void KillRangeEnemies(int count) => OnKillRangeEnemies?.Invoke(count);
    public static void KillWithTomahawk(int count) => OnKillWithTomahawk?.Invoke(count);
    public static void KillWithShockwave(int count) => OnKillWithShockwave?.Invoke(count);
    public static void KillInHighlight(int count) => OnKillInHighlight?.Invoke(count);
    public static void EnemiesInVoidNexus(int count) => OnEnemiesInVoidNexus?.Invoke(count);
    public static void KillDuringGoldenSanctuary(int count) => OnKillDuringGoldenSanctuary?.Invoke(count);
    public static void KillWithMine(int count) => OnKillWithMine?.Invoke(count);
    public static void KillWithSatellite(int count) => OnKillWithSatellite?.Invoke(count);
    public static void SkipWaves(int count) => OnSkipWaves?.Invoke(count);
    public static void PlayForHours(int count) => OnPlayForHours?.Invoke(count);

}


