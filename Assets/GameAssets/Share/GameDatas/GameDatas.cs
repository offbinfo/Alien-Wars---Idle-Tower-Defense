using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameDatas
{

    #region Resource data
    public static float Gold
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_Gold, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_Gold, value);
            EventDispatcher.PostEvent(EventID.OnGoldChanged, 0);
        }
    }

    public static float Gem
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_Gem, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_Gem, value);
            EventDispatcher.PostEvent(EventID.OnGemChanged, 0);
        }
    }

    public static float PowerStone
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_power_stone, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_power_stone, value);
            EventDispatcher.PostEvent(EventID.OnPowerStoneChanged, 0);
        }
    }

    public static float Tourament
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_tourament, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_tourament, value);
            EventDispatcher.PostEvent(EventID.OnTouramentChanged, 0);
        }
    }

    public static float Badges
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_Badges, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_Badges, value);
            EventDispatcher.PostEvent(EventID.OnBadgesChanged, 0);
        }
    }

    public static float ArmorSphere
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_ArmorSphere, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_ArmorSphere, value);
            EventDispatcher.PostEvent(EventID.OnArmorSphereChanged, 0);
        }
    }

    public static float IsLoopSong
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_IsLoopSong, 1); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_IsLoopSong, value);
            EventDispatcher.PostEvent(EventID.OnLoopSong, 0);
        }
    }

    public static float PowerSphere
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_PowerSphere, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_PowerSphere, value);
            EventDispatcher.PostEvent(EventID.OnPowerSphereChanged, 0);
        }
    }

    public static float EngineSphere
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_EngineSphere, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_EngineSphere, value);
            EventDispatcher.PostEvent(EventID.OnEngineSphereChanged, 0);
        }
    }

    public static float CryogenicSphere
    {
        get { return SavePrefs.GetFloat(GameKeys.KEY_CryogenicSphere, 0); }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_CryogenicSphere, value);
            EventDispatcher.PostEvent(EventID.OnCryogenicSphereChanged, 0);
        }
    }

    public static void BuyUsingCurrency(CurrencyType currency, float amount, Action<bool> OnBuySuccess)
    {
        float currentAmount = GetCurrency(currency);

        if (currentAmount >= amount)
        {
            SetCurrency(currency, currentAmount - amount);
            OnBuySuccess?.Invoke(true);
        }
        else
        {
            OnBuySuccess?.Invoke(false);
        }
    }

    private static float GetCurrency(CurrencyType currency)
    {
        return currency switch
        {
            CurrencyType.GOLD => Gold,
            CurrencyType.GEM => Gem,
            CurrencyType.POWER_STONE => PowerStone,
            CurrencyType.BADGES => Badges,
            _ => throw new ArgumentException("Invalid currency type"),
        };
    }

    private static void SetCurrency(CurrencyType currency, float value)
    {
        switch (currency)
        {
            case CurrencyType.GOLD:
                Gold = value;
                break;
            case CurrencyType.GEM:
                Gem = value;
                break;
            case CurrencyType.POWER_STONE:
                PowerStone = value;
                break;
            case CurrencyType.BADGES:
                Badges = value;
                break;
            default:
                throw new ArgumentException("Invalid currency type");
        }
    }


    #endregion

    #region X2 time
    public static bool activeX2
    {
        get
        {
            var now = DateTime.Now;
            return (now.Hour >= 12 && now.Hour < 14) || (now.Hour >= 19 && now.Hour < 22);
        }
    }
    #endregion

    #region Upgraders Tower Data
    public static int CountBuyXUpgrader
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_CountBuyXUpgrader, 1);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_CountBuyXUpgrader, value);
            EventDispatcher.PostEvent(EventID.OnChangeCountBuyX, 0);
        }
    }

    public static void SetLevelUpgraderInforTower(UpgraderID upgraderID, int indexLevelUgrader)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_LevelUpgraderInforTower}{upgraderID}", indexLevelUgrader);
    }

    public static int GetLevelUpgraderInforTower(UpgraderID upgraderID)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_LevelUpgraderInforTower}{upgraderID}", 1);
    }

    public static void SetLevelUpgraderGroupTower(UpgraderGroupID upgraderGroupID, int indexLevelUgrader)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_LevelUpgraderGroupTower}{upgraderGroupID}", indexLevelUgrader);
    }

    public static int GetLevelUpgraderGroupTower(UpgraderGroupID upgraderGroupID)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_LevelUpgraderGroupTower}{upgraderGroupID}", 1);
    }

    public static void UnlockClusterUpgrader(UpgraderCategory upgraderCategory)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_ClusterUpgrader}{upgraderCategory}", 1);
    }

    public static bool IsUnlockClusterUpgrader(UpgraderCategory upgraderCategory)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_ClusterUpgrader}{upgraderCategory}", 0) == 1 ? true : false;
    }

    public static void LockClusterUpgrader(UpgraderCategory upgraderCategory)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_ClusterUpgrader}{upgraderCategory}", 0);
    }
    #endregion

    #region Cards Tower Data
    public static DateTime LastQuestCheckDate
    {
        get
        {
            string savedDate = PlayerPrefs.GetString(GameKeys.Key_LastQuestCheckDate, "");
            if (string.IsNullOrEmpty(savedDate))
            {
                return DateTime.MinValue;
            }

            if (DateTime.TryParse(savedDate, out DateTime parsedDate))
            {
                return parsedDate.Date; 
            }

            return DateTime.MinValue;
        }
        set
        {
            PlayerPrefs.SetString(GameKeys.Key_LastQuestCheckDate, value.ToString("yyyy-MM-dd"));
            PlayerPrefs.Save();
        }
    }

    public static int countSpinCard
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_countSpinCard, 0);
        }
        set
        {
            if (levelCardUnlock >= 7)
            {
                SavePrefs.SetInt(GameKeys.Key_countSpinCard, 30);
                return;
            }
            SavePrefs.SetInt(GameKeys.Key_countSpinCard, value);
            if (value >= 30)
            {
                levelCardUnlock += 1;
                SavePrefs.SetInt(GameKeys.Key_countSpinCard, value - 30);
            }
        }
    }
    public static int levelCardUnlock
    { // level unlock ra các loại card để mở , unlock = số lượt quay
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_levelCardUnlock, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_levelCardUnlock, value);
            EventDispatcher.PostEvent(EventID.OnLevelUnlockCardChanged, null);
        }
    }

    public static int IndexSlotCardUnlockByGold
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.KEY_IndexSlotCardUnlockByGold, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_IndexSlotCardUnlockByGold, value);
            EventDispatcher.PostEvent(EventID.OnRefreshUnlockSlotCard, 0);
        }
    }

    public static int IndexSlotCard
    {
        get {
            return SavePrefs.GetInt(GameKeys.KEY_IndexSlotCard, 0); 
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_IndexSlotCard, value);
            EventDispatcher.PostEvent(EventID.OnRefreshUnlockSlotCard, 0);
        }
    }

    public static int TotalCardEquip
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.KEY_TotalCardEquip, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_TotalCardEquip, value);
        }
    }

    public static bool IsUnlockSlotCard(int id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_IsUnlockSlotCard}{id}", 0) == 1 ? true : false;
    }
    public static void UnlockSlotCard(int id)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_IsUnlockSlotCard}{id}", 1);
        EventDispatcher.PostEvent(EventID.OnNewCardUnlock, id);
    }

    public static bool IsUnlockCard(CardID id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_IsUnlockCard}{id}", 0) == 1 ? true : false;
    }
    public static void UnlockCard(CardID id)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_IsUnlockCard}{id}", 1);
        EventDispatcher.PostEvent(EventID.OnNewCardUnlock, id);
    }

    public static int GetLevelCard(CardID id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_CardLevel}{id}", 0);
    }
    public static void UpgradeCard(CardID id, int level)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_CardLevel}{id}", level);
    }

    public static int GetAmountCard(CardID id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_CardAmount}{id}", 0);
    }
    public static void SetAmountCard(CardID id, int amount)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_CardAmount}{id}", amount);
    }

    // Equip Card
    public static bool IsCardEquiped(CardID id)
    {
        if(GetCardEquiped(id) != 0)
        {
            return true;
        }
        return false;
    }

    public static int GetCardEquiped(CardID id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_CardEquip}{id}", 0);
    }

    public static void SetCardEquiped(CardID id)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_CardEquip}{id}", (int)id);
    }

    public static void UnEquipedCard(CardID id)
    {
        SavePrefs.DeleteKey($"{GameKeys.KEY_CardEquip}{id}");
    }

    #endregion

    #region Labs Tower Data
    public static int CountSlotLabUnlock
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.KEY_CountSlotUnlock, 1);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_CountSlotUnlock, value);
        }
    }

    public static int GetLevelSubjectLab(IdSubjectType id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_LevelSubjectLab}{id}", 1);
    }
    public static void SetLevelSubjectLab(IdSubjectType id, int level)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_LevelSubjectLab}{id}", level);
    }

    public static DateTime GetTimeTargetSubject(IdSubjectType id)
    {
        return LoadObject<DateTime>($"{GameKeys.KEY_TimeTargetSubjectLab}{id}");
    }
    public static void SetTimeTargetSubject(IdSubjectType id, DateTime timeTarget)
    {
        SaveObject($"{GameKeys.KEY_TimeTargetSubjectLab}{id}", timeTarget);
    }

    public static int SlotSave(int index)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_SlotSave}{index}", -1);
    }
    public static void SlotSave(int index, IdSubjectType id)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_SlotSave}{index}", (int)id);
    }
    public static void SetNullSlotSave(int index)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_SlotSave}{index}", -1);
    }

    public static void UnlockClusterLabInfor(LabCategory labCategory)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_UnlockLabInfor}{labCategory}", 1);
    }

    public static bool IsClusterUnlockLabInfor(LabCategory labCategory)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_UnlockLabInfor}{labCategory}", 0) == 1 ? true : false;
    }
    #endregion

    #region Init Default In Game Data
    public static bool IsFirstDoneWave40
    {
        get { return SavePrefs.GetInt(GameKeys.Key_IsFirstDoneWave40, 0) == 1 ? true : false; }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsFirstDoneWave40, 1);
        }
    }

    public static string StartTimeFirstInGame
    {
        get { return SavePrefs.GetString(GameKeys.Key_StartTimeKey, DateTime.Now.ToString()); }
        set
        {
            SavePrefs.SetString(GameKeys.Key_StartTimeKey, DateTime.UtcNow.ToString("o"));
        }
    }

    public static bool IsTut_Play
    {
        get
        {
            return (SavePrefs.GetInt(GameKeys.Key_IsTut_Play, 1) == 1 /*&& !IsTut_PlayDemo*/) ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsTut_Play, value ? 1 : 0);
        }
    }

    public static bool isTutLab
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isTutLab, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isTutLab, value ? 1 : 0);
        }
    }

    public static bool isTutSpeed
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isTutSpeed, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isTutSpeed, value ? 1 : 0);
        }
    }

    public static bool isTutBuildPhase2
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isTutBuildPhase2, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isTutBuildPhase2, value ? 1 : 0);
        }
    }

    public static bool IsFirstTimeGoHome
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsFirstTimeGoHome, 1) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsFirstTimeGoHome, value ? 1 : 0);
        }
    }

    public static bool IsFirstClaimOfflineReward
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsFirstClaimOfflineReward, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsFirstClaimOfflineReward, value ? 1 : 0);
        }
    }

    public static bool IsResetData
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsResetData, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsResetData, value ? 1 : 0);
        }
    }

    public static bool IsTut_0_Upgrade
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsTut_0_Upgrade, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsTut_0_Upgrade, value ? 1 : 0);
        }
    }

    public static bool IsTut_PlayDemo
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsTutPlayDemo, 1) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsTutPlayDemo, value ? 1 : 0);
        }
    }

    public static bool IsEndTutorial
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsEndTutorial, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsEndTutorial, value ? 1 : 0);
        }
    }

    public static bool IsFirstInGame
        {
            get { return SavePrefs.GetInt(GameKeys.KEY_IsFirstInGame, 0) == 1 ? true : false; }
            set
            {
                SavePrefs.SetInt(GameKeys.KEY_IsFirstInGame, value ? 1 : 0);
            }
        }

        public static bool IsActiveTutorial
        {
            get { return SavePrefs.GetInt(GameKeys.KEY_IsActiveTutorial, 0) == 1 ? true : false; }
            set
            {
                SavePrefs.SetInt(GameKeys.KEY_IsActiveTutorial, value ? 1 : 0);
            }
        }

        public static void DefaultFirstInGame()
        {
            UnlockSlotCard(0);
            if (!IsFirstInGame)
                {
                StartTimeFirstInGame = DateTime.UtcNow.ToString("o");
                //SetDataUpgraderTowerTutorial();
                ResetUpgraderTowerDefault();
            }
        }

        public static void ResetUpgraderTowerDefault()
        {
            // Danh sách giá trị mặc định cho Upgrader và GroupUpgrader
            int indexDefault = 1;
            var upgraderLevels = new (UpgraderID, int)[]
            {
                (UpgraderID.attack_damage, indexDefault),
                (UpgraderID.attack_range, indexDefault),
                (UpgraderID.attack_speed, indexDefault),
                (UpgraderID.health, indexDefault),
                (UpgraderID.shield_hp, indexDefault),
                (UpgraderID.shield_spawn_time, indexDefault),
                (UpgraderID.satellite_number, indexDefault),
                (UpgraderID.bomb_number, indexDefault)
            };

            var upgraderGroupLevels = new (UpgraderGroupID, int)[]
            {
                (UpgraderGroupID.ATTACK, indexDefault),
                (UpgraderGroupID.DEFENSE, indexDefault),
                (UpgraderGroupID.RESOURCE, indexDefault),
                (UpgraderGroupID.SATELLITE, indexDefault)
            };

            UpgraderCategory[] categories =
            {
                UpgraderCategory.WORKSHOP_REFLECT,
                UpgraderCategory.WORKSHOP_SHIELD,
                UpgraderCategory.WORKSHOP_SATELLITES,
                UpgraderCategory.WORKSHOP_GROUND_MINES
            };

            UnlockClusterUpgrader(UpgraderCategory.WORKSHOP_BASE);

            SetUnlockUltimateWeapon(UW_ID.TOMAHAWK, false);
            SetUnlockUltimateWeapon(UW_ID.HIGHLIGHT, false);
            SetUnlockUltimateWeapon(UW_ID.SHOCKWAVE, false);

            SetLevelUltimateQuantity(UW_ID.TOMAHAWK, 1);
            SetLevelUltimateQuantity(UW_ID.HIGHLIGHT, 1);
            SetLevelUltimateQuantity(UW_ID.SHOCKWAVE, 1);

            foreach (var (id, level) in upgraderLevels) SetLevelUpgraderInforTower(id, level);
            foreach (var (groupId, level) in upgraderGroupLevels) SetLevelUpgraderGroupTower(groupId, level);
            foreach (var category in categories) LockClusterUpgrader(category);
        }

        public static void SetDataUpgraderTowerTutorial()
        {
            // Danh sách giá trị tutorial cho Upgrader và GroupUpgrader
            var upgraderLevels = new (UpgraderID, int)[]
            {
                (UpgraderID.attack_damage, 18),
                (UpgraderID.attack_range, 28),
                (UpgraderID.attack_speed, 101),
                (UpgraderID.health, 44),
                (UpgraderID.shield_hp, 3),
                (UpgraderID.shield_spawn_time, 17),
                (UpgraderID.satellite_number, 4),
                (UpgraderID.bomb_number, 6)
            };

            var upgraderGroupLevels = new (UpgraderGroupID, int)[]
            {
                (UpgraderGroupID.ATTACK, 1),
                (UpgraderGroupID.RESOURCE, 1),
                (UpgraderGroupID.DEFENSE, 3),
                (UpgraderGroupID.SATELLITE, 3)
            };

            UpgraderCategory[] categories =
            {
                UpgraderCategory.WORKSHOP_REFLECT,
                UpgraderCategory.WORKSHOP_SHIELD,
                UpgraderCategory.WORKSHOP_SATELLITES,
                UpgraderCategory.WORKSHOP_GROUND_MINES
            };

            UnlockClusterUpgrader(UpgraderCategory.WORKSHOP_BASE);

            SetUnlockUltimateWeapon(UW_ID.TOMAHAWK, true);
            SetUnlockUltimateWeapon(UW_ID.HIGHLIGHT, true);

            SetLevelUltimateQuantity(UW_ID.TOMAHAWK, 5);
            SetLevelUltimateQuantity(UW_ID.HIGHLIGHT, 3);

            foreach (var (id, level) in upgraderLevels) SetLevelUpgraderInforTower(id, level);
            foreach (var (groupId, level) in upgraderGroupLevels) SetLevelUpgraderGroupTower(groupId, level);
            foreach (var category in categories) UnlockClusterUpgrader(category);
        }
    #endregion

    #region World and wave
    public static int GetHighestWaveInWorld(int indexWorld)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_HighestWaveInWorld}{indexWorld}", 0);
    }
    public static void SetHighestWaveInWorld(int indexWorld, int highestWave)
    {
        if (GetHighestWaveInWorld(indexWorld) >= highestWave) return;
        SavePrefs.SetInt($"{GameKeys.KEY_HighestWaveInWorld}{indexWorld}", highestWave);
    }

    static int currentWorld;
    public static int CurrentWorld
    {
        get { return currentWorld; }
        set
        {
            currentWorld = value;
        }
    }

    public static int GetHighestWorld()
    {
        return SavePrefs.GetInt(GameKeys.KEY_HighestWorld, 0);
    }
    public static void SetHighestWorld(int highestWorld)
    {
        SavePrefs.SetInt(GameKeys.KEY_HighestWorld, highestWorld);
    }
    #endregion

    #region technology
    public static float timeGameMax
    {
        get
        {
            return SavePrefs.GetFloat(GameKeys.KEY_timeGameMax, 2f);
        }
        set
        {
            if (value < timeGameMax) return;
            SavePrefs.SetFloat(GameKeys.KEY_timeGameMax, value);
        }
    }

    public static float TimeGamePlaySpeed
    {
        get
        {
            return SavePrefs.GetFloat(GameKeys.KEY_TimeGamePlaySpeed, 1f);
        }
        set
        {
            SavePrefs.SetFloat(GameKeys.KEY_TimeGamePlaySpeed, value);
        }
    }

    #endregion

    #region Function
    public static void SaveObject(string key, object obj)
    {
        var json = JsonParse.ToJson(obj);
        SavePrefs.SetString(key, json);
    }
    public static T LoadObject<T>(string key)
    {
        var json = SavePrefs.GetString(key, null);
        if (string.IsNullOrEmpty(json))
            return default;

        return JsonParse.FromJson<T>(json);
    }
    #endregion

    #region Revive Tower
    public static int ReviveTowerCount
    {
        get { return SavePrefs.GetInt(GameKeys.KEY_ReviveTowerCount, 5); }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_ReviveTowerCount, value);
            EventDispatcher.PostEvent(EventID.OnChangeAmountRevive, 0);
        }
    }

    public static DateTime timeTargetFullRevive
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.KEY_timeTargetFullRevive);
        }
        set
        {
            SaveObject(GameKeys.KEY_timeTargetFullRevive, value);
        }
    }
    #endregion

    #region SHOP
    public static bool isX2
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isX2, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isX2, value ? 1 : 0);
        }
    }
    public static bool isX3
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isX3, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isX3, value ? 1 : 0);
        }
    }
    public static bool isX4
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isX4, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isX4, value ? 1 : 0);
        }
    }
    public static float x_sum
    {
        get
        {
            var value = (isX2 ? 2 : 0) + (isX3 ? 3 : 0) + (isX4 ? 4 : 0) + (isX_2Gold ? 1.5f : 0);
            return value;
        }
    }

    public static DateTime timeClaimFreeGem_Target
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeClaimFreeGem);
        }
        set
        {
            SaveObject(GameKeys.Key_timeClaimFreeGem, value);
        }
    }
    public static DateTime timeClaimFreeGold_Target
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeClaimFreeGold);
        }
        set
        {
            SaveObject(GameKeys.Key_timeClaimFreeGold, value);
        }
    }
    #endregion

    #region Bonus gold in game
    public static DateTime timeTargetBonusGold
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeTargetBonusGold);
        }
        set
        {
            SaveObject(GameKeys.Key_timeTargetBonusGold, value);
        }
    }
    public static bool isX_2Gold => timeTargetBonusGold > DateTime.Now;

    public static int XBonusGoldDailyQuest
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_XBonusGoldDailyQuest, 1);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_XBonusGoldDailyQuest, value);
            EventDispatcher.PostEvent(EventID.XBonusGoldDailyQuest, 0);
        }
    }

    public static float TimeBonusGold
    {
        get
        {
            return SavePrefs.GetFloat(GameKeys.Key_TimeBonusGold, 15f);
        }
        set
        {
            SavePrefs.SetFloat(GameKeys.Key_TimeBonusGold, value);
        }
    }
    #endregion

    #region Ultimate Weapon


    public static bool isUnlockUltimateWeapon(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_isUnlockUltimateWeapon}{id}", 0) == 1 ? true : false;
    }
    public static void SetUnlockUltimateWeapon(UW_ID id, bool isUnlock)
    {
        SavePrefs.SetInt($"{GameKeys.Key_isUnlockUltimateWeapon}{id}", isUnlock ? 1 : 0);
    }
    public static int GetLevelUltimateQuantity(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelUltimateQuantity}{id}", 1);
    }
    public static void SetLevelUltimateQuantity(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelUltimateQuantity}{id}", value);
    }

    public static int GetLevelUltimateDmgBonus(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelUltimateDmgBonus}{id}", 1);
    }

    public static void SetLevelUltimateDmgBonus(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelUltimateDmgBonus}{id}", value);
    }

    public static int GetLevelUltimateDurationBonus(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelUltimateDurationBonus}{id}", 1);
    }

    public static void SetLevelUltimateSizeBonus(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelUltimateSizeBonus}{id}", value);
    }

    public static int GetLevelUltimateSizeBonus(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelUltimateSizeBonus}{id}", 1);
    }

    public static void SetLevelUltimateDurationBonus(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelUltimateDurationBonus}{id}", value);
    }

    public static int GetLevelUltimateSlowBonus(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelUltimateSlowBonus}{id}", 1);
    }

    public static void SetLevelUltimateSlowBonus(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelUltimateSlowBonus}{id}", value);
    }

    public static int GetLevelUltimateCooldown(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelUltimateCooldown}{id}", 1);
    }
    public static void SetLevelUltimateCooldown(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelUltimateCooldown}{id}", value);
    }

    public static int GetLevelUltimateChangeCost(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelUltimateChangeCost}{id}", 1);
    }
    public static void SetLevelUltimateChangeCost(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelUltimateChangeCost}{id}", value);
    }

    public static int GetLevelUltimateDmgScale(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelUltimateDmgScale}{id}", 1);
    }
    public static void SetLevelUltimateDmgScale(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelUltimateDmgScale}{id}", value);
    }

    public static int GetLevelUltimateAngle(UW_ID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelUltimateAngle}{id}", 1);
    }
    public static void SetLevelUltimateAngle(UW_ID id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelUltimateAngle}{id}", value);
    }

    public static int IsUnlockSLotUltimateWeapon()
    {
        return SavePrefs.GetInt(GameKeys.Key_UnlockSLotUltimateWeapon, 0);
    }
    public static void UnlockSLotUltimateWeapon(int value)
    {
        SavePrefs.SetInt(GameKeys.Key_UnlockSLotUltimateWeapon, value);
        EventDispatcher.PostEvent(EventID.OnUnlockSlotUW, 0);
    }
    #endregion

    #region RemoveAds 

    public static bool RemoveAdsForever
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_RemoveAdsForever, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_RemoveAdsForever, value ? 1 : 0);
        }
    }
    #endregion

    #region User Power

    public static float basePower => 6.8f;
    public static float userPower
    {
        get
        {
            var sum = 4f;
            /*if(ConfigManager.instance != null)
            {
                foreach (var data in ConfigManager.instance.
                    upgraderCtrl.UpgradeInforManager.upgradeInforDatas)
                {
                    sum += data.GetCoefficient(GetLevelUpgraderInforTower(data.upgraderID));
                }
            } else
            {
                sum = 4f;   
            }*/
            return sum;
        }
    }
    #endregion

    #region rewardTime
    public static int secondToGetReward
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_SecondToGetReward, 60 * 60 * 4); //4 hour
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_SecondToGetReward, value);
        }
    }
    public static int SecondsAccumulate
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_SecondsAccumulate, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_SecondsAccumulate, value);
        }
    }
    public static DateTime LastLogoutTime
    {
        get
        {
            long binary = Convert.ToInt64(SavePrefs.GetString(GameKeys.Key_LastLogoutTimeOfflineReward, "0"));
            return binary == 0 ? DateTime.UtcNow : DateTime.FromBinary(binary);
        }
        set
        {
            SavePrefs.SetString(GameKeys.Key_LastLogoutTimeOfflineReward, value.ToBinary().ToString());
        }
    }
    #endregion

    #region MileStone
    public static bool IsUsingPremiumMileStones
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsUsingPremiumMileStones, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsUsingPremiumMileStones, value ? 1 : 0);
            EventDispatcher.PostEvent(EventID.OnUsingPremiumMileStones, 0);
        }
    }

    public static void SaveAllItems(ItemData newItem)
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathMileStoneUnlock);

        AllItemMileStoneData allItemsData = LoadAllItems() ?? new AllItemMileStoneData();

        allItemsData.items.Add(newItem);

        string jsonData = JsonUtility.ToJson(allItemsData, true);
        File.WriteAllText(filePath, jsonData);
        //SaveObject(filePath, allItemsData);
        DebugCustom.Log("All items saved to file: " + filePath);
    }


    public static AllItemMileStoneData LoadAllItems()
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathMileStoneUnlock);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            AllItemMileStoneData allItemsData = JsonUtility.FromJson<AllItemMileStoneData>(jsonData);
            //AllItemMileStoneData allItemsData = LoadObject<AllItemMileStoneData>(filePath);
            return allItemsData;
        }
        else
        {
            DebugCustom.LogWarning("File not found!");
            return null;
        }
    }

    #endregion

    #region Unlock Features

    public static bool isUnlockRanking
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isUnlockRanking, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isUnlockRanking, value ? 1 : 0);
        }
    }

    public static bool isUnlockAchievement
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isUnlockAchievement, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isUnlockAchievement, value ? 1 : 0);
        }
    }

    public static bool isUnlockLuckyDraw
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isUnlockLuckyDraw, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isUnlockLuckyDraw, value ? 1 : 0);
        }
    }

    public static bool isUnlockDailyGift
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isUnlockDailyGift, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isUnlockDailyGift, value ? 1 : 0);
        }
    }

    public static bool isUnlockLab
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_isUnlockLab, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isUnlockLab, value ? 1 : 0);
        }
    }

    public static bool IsUnlockFeatureCard
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsUnlockFeatureCard, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsUnlockFeatureCard, value ? 1 : 0);
        }
    }

    public static bool IsUnlockFeatureArena
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_IsUnlockFeatureArena, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IsUnlockFeatureArena, value ? 1 : 0);
        }
    }

    #endregion

    #region Lucky Spin
    public static string GetLastPurchaseDate()
    {
        return PlayerPrefs.GetString(GameKeys.Key_LastPurchaseDate, "");
    }

    public static void SetLastPurchaseDate()
    {
        PlayerPrefs.SetString(GameKeys.Key_LastPurchaseDate, DateTime.Now.ToString("yyyy-MM-dd"));
    }
    
    public static int GetMoreTurnPurchaseCount()
    {
        return PlayerPrefs.GetInt(GameKeys.Key_MoreTurnPurchaseCount, 0);
    }

    public static void SetMoreTurnPurchaseCount(int purchaseCount)
    {
        PlayerPrefs.SetInt(GameKeys.Key_MoreTurnPurchaseCount, purchaseCount);
    }

    public static bool IsSpinFree
    {
        get
        {
            string lastSpinTimeStr = SavePrefs.GetString(GameKeys.Key_lastSpinTime, "0");
            long lastSpinTime = long.Parse(lastSpinTimeStr);

            if (lastSpinTime == 0 || (DateTime.UtcNow - DateTimeOffset.FromUnixTimeSeconds(lastSpinTime).UtcDateTime).TotalHours >= 24)
            {
                return true;
            }

            return SavePrefs.GetInt(GameKeys.Key_isSpinFree, 1) == 1;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_isSpinFree, value ? 1 : 0);

            if (!value)
            {
                SavePrefs.SetString(GameKeys.Key_lastSpinTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            }
        }
    }

    public static int CountSpinFree
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_CountSpinFree, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_CountSpinFree, value);
        }
    }
    #endregion

    #region DailyReward

    public static void DayDailyReceived(int day, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_IsDayReceived}{day}", value);
    }

    public static bool IsDayDailyReceived(int day)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsDayReceived}{day}", 0) == 1 ? true : false;
    }

    public static void LastLoginDateDailyReward(string day)
    {
        SavePrefs.SetString($"{GameKeys.Key_LastLoginDateDailyReward}", day);
    }

    public static string IsLastLoginDateDailyReward()
    {
        return SavePrefs.GetString($"{GameKeys.Key_LastLoginDateDailyReward}", "");
    }

    public static void CurrentDayDailyReward(int day)
    {
        SavePrefs.SetInt($"{GameKeys.Key_CurrentDayDailyReward}", day);
    }

    public static int IsCurrentDayDailyReward()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_CurrentDayDailyReward}", 0);
    }

    public static void SetAccumulateDailyReward(int day)
    {
        if(day > 30)
        {
            SavePrefs.SetInt($"{GameKeys.Key_IsAccumulateDailyReward}", 0);
        }
        else
        {
            SavePrefs.SetInt($"{GameKeys.Key_IsAccumulateDailyReward}", day);
        }
        EventDispatcher.PostEvent(EventID.OnRefreshDailyReward, 0);
    }

    public static int GetAccumulateDailyReward()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsAccumulateDailyReward}", 0);
    }

    public static void ClaimedAccumulateDailyReward(int day, bool claimed)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ClaimedAccumulateDailyReward}{day}", claimed == true ? 1 : 0);
    }

    public static bool IsClaimedAccumulateDailyReward(int day)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ClaimedAccumulateDailyReward}{day}", 0) == 1 ? true : false;
    }
    #endregion

    #region DailyGift
    public static void LastLoginDateDailyQuest(string day)
    {
        SavePrefs.SetString($"{GameKeys.Key_LastLoginDateDailyQuest}", day);
    }

    public static string IsLastLoginDateDailyQuest()
    {
        return SavePrefs.GetString($"{GameKeys.Key_LastLoginDateDailyQuest}", "");
    }

    public static void ClaimedQuestReward(Enum typeQuest, bool claimed)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ClaimedAccumulateDailyGift}{typeQuest}", claimed == true ? 1 : 0);
    }

    public static bool IsClaimedQuestReward(Enum typeQuest)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ClaimedAccumulateDailyGift}{typeQuest}", 0) == 1 ? true : false;
    }

    public static void ClaimedAccumulateDailyGift(int day, bool claimed)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ClaimedAccumulateDailyGift}{day}", claimed == true ? 1 : 0);
    }

    public static bool IsClaimedAccumulateDailyGift(int day)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ClaimedAccumulateDailyGift}{day}", 0) == 1 ? true : false;
    }

    public static void SetTotalQuestDone(Enum typeQuest, int countQuest)
    {
        //if (countQuest == 0) return;
        SavePrefs.SetInt($"{GameKeys.Key_TotalQuestDone}{typeQuest}", countQuest);
    }

    public static int GetTotalQuestDone(Enum typeQuest)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_TotalQuestDone}{typeQuest}", 0);
    }

    public static void SetAccumulateDailyGift(int accumulate)
    {
        SavePrefs.SetInt($"{GameKeys.Key_AccumulateDailyGift}", accumulate);
        EventDispatcher.PostEvent(EventID.OnRefreshDailyGift, 0);
    }

    public static int GetAccumulateDailyGift()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_AccumulateDailyGift}", 0);
    }

    public static bool IsChangeQuestReward
    {
        get
        {
            string lastSpinTimeStr = SavePrefs.GetString(GameKeys.Key_IsChangeQuestReward, "0");
            long lastSpinTime = long.Parse(lastSpinTimeStr);

            if (lastSpinTime == 0 || (DateTime.UtcNow - DateTimeOffset.FromUnixTimeSeconds(lastSpinTime).UtcDateTime).TotalHours >= 24)
            {
                return true;
            }

            return false;
        }
        set
        {
            if (!value)
            {
                SavePrefs.SetString(GameKeys.Key_IsChangeQuestReward, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            }
        }
    }

    public static void DeleteAllQuests()
    {
        if (PlayerPrefs.HasKey(GameKeys.QuestKey))
        {
            PlayerPrefs.DeleteKey(GameKeys.QuestKey);
        }
    }

    public static void SaveAllQuests(ItemDailyQuestData newItem)
    {
        AllDailyQuestData allItemsData = LoadAllQuests() ?? new AllDailyQuestData();
        allItemsData.items.Add(newItem);

        string jsonData = JsonUtility.ToJson(allItemsData);
        string encodedData = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonData));

        PlayerPrefs.SetString(GameKeys.QuestKey, encodedData);
        PlayerPrefs.Save();
        DebugCustom.Log("All items saved to PlayerPrefs");
    }

    public static AllDailyQuestData LoadAllQuests()
    {
        if (PlayerPrefs.HasKey(GameKeys.QuestKey))
        {
            string encodedData = PlayerPrefs.GetString(GameKeys.QuestKey);
            string jsonData = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedData));

            return JsonUtility.FromJson<AllDailyQuestData>(jsonData);
        }
        else
        {
            DebugCustom.LogWarning("No saved quests found!");
            return null;
        }
    }

    #endregion

    #region WelcomeBack Round
    public static int GetWaveInResume(int world)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetWaveInResume}{world}", 0);
    }
    public static void SetWaveInResume(int world ,int wave)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetWaveInResume}{world}", wave);
    }

    public static int GetWorldInResume()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetWorldInResume}", 0);
    }
    public static void SetWorldInResume(int world)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetWorldInResume}", world);
    }

    public static bool IsResumeWave(int world)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ResumeWave}{world}", 0) == 1 ? true : false;
    }
    public static void ResumeWave(int world, bool resume)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ResumeWave}{world}", resume == true ? 1 : 0);
    }

    public static bool IsActiveWelcomeBack()
    {
        return SavePrefs.GetInt(GameKeys.Key_IsActiveWelcomeBack, 0) == 1 ? true : false;
    }
    public static void ActiveWelcomeBack(bool resume)
    {
        SavePrefs.SetInt(GameKeys.Key_IsActiveWelcomeBack, resume == true ? 1 : 0);
    }
    #endregion

    #region data
    public static void CheckAndDeleteJsonData()
    {
        if (!PlayerPrefs.HasKey(GameKeys.Key_GameInstalled))
        {
            DebugCustom.Log("PlayerPrefs đã bị reset! Xóa dữ liệu JSON...");

            // Xóa tất cả file JSON
            string filePathQuest = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathQuestsUnlock);
            string filePathMilestone = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathMileStoneUnlock);
            if (File.Exists(filePathQuest))
            {
                File.Delete(filePathQuest);
                DebugCustom.Log("Đã xóa file JSON: " + filePathQuest);
            }
            if (File.Exists(filePathMilestone))
            {
                File.Delete(filePathMilestone);
                DebugCustom.Log("Đã xóa file JSON: " + filePathMilestone);
            }

            PlayerPrefs.SetInt(GameKeys.Key_GameInstalled, 1);
            PlayerPrefs.Save();
        }
    }
    #endregion

    #region Profile
    public static void UnlockAvatar(TypeAvatar typeAvatar)
    {
        SavePrefs.SetInt($"{GameKeys.Key_IsUnlockAvatar} {typeAvatar}", 1);
    }

    public static bool IsUnlockAvatar(TypeAvatar typeAvatar)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsUnlockAvatar} {typeAvatar}", 0) == 1 ? true : false;
    }

    public static string userID
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }
    public static string user_name
    {
        get
        {
            return SavePrefs.GetString(GameKeys.Key_user_name, "playerid_1");
        }
        set
        {
            SavePrefs.SetString(GameKeys.Key_user_name, value);
            EventDispatcher.PostEvent(EventID.OnChangedName, null);
        }
    }
    public static int id_avatar
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_id_avatar, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_id_avatar, value);
            EventDispatcher.PostEvent(EventID.OnChangedAvatar, null);
        }
    }
    //Player
    public static DateTime StartDate
    {
        get
        {
            var time = LoadObject<DateTime>(GameKeys.Key_StartDate);
            if (time == default)
            {
                time = DateTime.Now;
                SaveObject(GameKeys.Key_StartDate, time);
            }
            return time;
        }
    }
    [RuntimeInitializeOnLoadMethod]
    private static void SetStartDate()
    {
        Debug.Log(StartDate);
    }
    public static int GetDataProfile(IDInfo id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_DataProfile}{id}", 0);
    }
    public static void SetDataProfile(IDInfo id, float value)
    {
        if (value < 0) value = 0;
        SavePrefs.SetFloat($"{GameKeys.Key_DataProfile}{id}", value);

        if (id == IDInfo.NumberofBossesDefeated)
        {
            var stat = new User_BossKilled()
            {
                userID = userID,
                name = user_name,
                idAvatar = id_avatar,
                bossKilled = value,
            };
        }
    }

    public static int countUpgraderTime
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_countUpgraderTime, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_countUpgraderTime, value);
        }
    }

    public static int countDoneQuest
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_countDoneQuest, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_countDoneQuest, value);
        }
    }

    public static int countLoseGame
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_countLoseGame, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_countLoseGame, value);
        }
    }

    public static int countEnemyDestroy
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_countEnemyDestroy, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_countEnemyDestroy, value);
        }
    }

    #endregion

    #region Arena
    public static string GetLastLogTimeRank()
    {
        return PlayerPrefs.GetString(GameKeys.lastLogTimeKey);
    }

    public static void SetLastLogTimeRank()
    {
        PlayerPrefs.SetString(GameKeys.lastLogTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public static bool CheckNewArenaEvent()
    {
        if (!Extensions.CheckArenaTime()) return false; 

        DateTime today = DateTime.Today;
        string lastEventDateStr = PlayerPrefs.GetString(GameKeys.lastEventDateKey, "");
        DateTime lastEventDate = string.IsNullOrEmpty(lastEventDateStr) ? DateTime.MinValue : DateTime.Parse(lastEventDateStr);

        if (lastEventDate != today)
        {
            DebugCustom.Log("Sự kiện mới bắt đầu!");
            PlayerPrefs.SetString(GameKeys.lastEventDateKey, today.ToString("yyyy-MM-dd"));
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            DebugCustom.Log("Đã trong cùng một sự kiện.");
        }

        return false;
    }

    public static bool IsFirstPlayArena
    {
        get { return SavePrefs.GetInt(GameKeys.KEY_IsFirstPlayArena, 0) == 1 ? true : false; }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_IsFirstPlayArena, 1);
        }
    }

    public static int TotalArenaRankPlay
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.KEY_TotalArenaRankPlay, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_TotalArenaRankPlay, value);
        }
    }
    public static int TotalArenaRankActive
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.KEY_TotalArenaRankActive, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.KEY_TotalArenaRankActive, value);
        }
    }

    public static void StartBattleTimer()
    {
        // Nếu chưa có thời gian bắt đầu, thì lưu lại
        if (!PlayerPrefs.HasKey(GameKeys.KEY_PlayStartTicks))
        {
            PlayerPrefs.SetString(GameKeys.KEY_PlayStartTicks, DateTime.Now.Ticks.ToString());
        }
    }

    public static bool IsResetRank()
    {
        return PlayerPrefs.HasKey(GameKeys.lastLogTimeKey);
    }

    public static void StopBattleAndAddTime()
    {
        if (!PlayerPrefs.HasKey(GameKeys.KEY_PlayStartTicks))
            return;

        long startTicks = long.Parse(PlayerPrefs.GetString(GameKeys.KEY_PlayStartTicks, "0"));
        DateTime startTime = new DateTime(startTicks);
        TimeSpan sessionDuration = DateTime.Now - startTime;

        int totalMinutesPlayed = PlayerPrefs.GetInt(GameKeys.KEY_PlayTotalMinutes, 0);
        totalMinutesPlayed += (int)sessionDuration.TotalMinutes;

        PlayerPrefs.SetInt(GameKeys.KEY_PlayTotalMinutes, totalMinutesPlayed);

        // Reset start time
        PlayerPrefs.DeleteKey(GameKeys.KEY_PlayStartTicks);
    }

    public static int GetPlayTimeMinutesPerDay()
    {
        return PlayerPrefs.GetInt(GameKeys.KEY_PlayTotalMinutes, 0);
    }

    public static void ResetPlayTime()
    {
        PlayerPrefs.DeleteKey(GameKeys.KEY_PlayTotalMinutes);
        PlayerPrefs.DeleteKey(GameKeys.KEY_PlayStartTicks);
    }

    public static bool IsArenaChain()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ArenaChain}", 0) == 1 ? true : false;
    }

    public static void ArenaChain(bool isChain)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ArenaChain}", isChain == true ? 1 : 0);
    }

    public static bool IsBattleArena()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ArenaChain}", 0) == 1 ? true : false;
    }

    public static void StartBattleArena(bool isChain)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ArenaChain}", isChain == true ? 1 : 0);
    }

    public static bool IsActiveEventArena()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ResetRankArena}", 0) == 1 ? true : false;
    }

    public static void ActiveEventArena(bool isChain)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ResetRankArena}", isChain == true ? 1 : 0);
    }

    static int currentRank;
    public static int CurrentRank
    {
        get { return currentRank; }
        set
        {
            currentRank = value;
        }
    }

    public static void DecreaseRank()
    {
        int currentRank = GetHighestRank();

        if (currentRank > (int)TypeRank.Recruit)
        {
            currentRank--;
        }

        SetHighestRank(currentRank);
        EventDispatcher.PostEvent(EventID.OnOnRefreshUIRankArena, 0);
    }

    public static int TotalReplayArena
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_TotalReplayArena, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_TotalReplayArena, value);
        }
    }

    public static void IncreaseRank()
    {
        int currentRank = GetHighestRank();

        if (currentRank < (int)TypeRank.General)
        {
            GameAnalytics.LogTotalUpRankArena(TotalReplayArena);
            currentRank++;
        }

        SetHighestRank(currentRank);
        EventDispatcher.PostEvent(EventID.OnOnRefreshUIRankArena, 0);
    }

    public static int GetHighestRank()
    {
        return SavePrefs.GetInt(GameKeys.KEY_HighestRank, 0);
    }
    public static void SetHighestRank(int highestRank)
    {
        highestRank = Mathf.Clamp(highestRank, (int)TypeRank.Recruit, (int)TypeRank.General);
        SavePrefs.SetInt(GameKeys.KEY_HighestRank, highestRank);
    }

    public static int GetIndexRank(TypeRank typeRank)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_IndexRank}{typeRank}", 30);
    }
    public static void SetIndexRank(TypeRank typeRank,int indexRank)
    {
        indexRank = Mathf.Clamp(indexRank, 1, 30);
        SavePrefs.SetInt($"{GameKeys.KEY_IndexRank}{typeRank}", indexRank);
    }

    public static int GetHighestWaveInRank(TypeRank typeRank)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_HighestWaveInRank}{typeRank}", 0);
    }
    public static void SetHighestWaveInRank(TypeRank typeRank, int highestWave)
    {
        if (GetHighestWaveInRank(typeRank) >= highestWave) return;
        SavePrefs.SetInt($"{GameKeys.KEY_HighestWaveInRank}{typeRank}", highestWave);
    }

    public static bool IsResetTouramentArena()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsResetTouramentArena}", 0) == 1 ? true : false;
    }

    public static void ResetTouramentArena(bool isChain)
    {
        SavePrefs.SetInt($"{GameKeys.Key_IsResetTouramentArena}", isChain == true ? 1 : 0);
    }

    public static bool IsClaimRewardArenaRank()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsClaimRewardArenaRank}", 0) == 1 ? true : false;
    }

    public static void ClaimRewardArenaRank(bool isChain)
    {
        SavePrefs.SetInt($"{GameKeys.Key_IsClaimRewardArenaRank}", isChain == true ? 1 : 0);
    }

    public static void SaveAllHistoryArena(ItemHistoryArenaData newItem)
    {
        AllHistoryArenaData allItemsData = LoadAllHistoryArenas() ?? new AllHistoryArenaData();
        allItemsData.items.Add(newItem);

        string jsonData = JsonUtility.ToJson(allItemsData);
        string encodedData = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonData));

        PlayerPrefs.SetString(GameKeys.HistoryArenaKey, encodedData);
        PlayerPrefs.Save();
        DebugCustom.Log("All items saved to PlayerPrefs");
    }

    public static AllHistoryArenaData LoadAllHistoryArenas()
    {
        if (PlayerPrefs.HasKey(GameKeys.HistoryArenaKey))
        {
            string encodedData = PlayerPrefs.GetString(GameKeys.HistoryArenaKey);
            string jsonData = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedData));

            return JsonUtility.FromJson<AllHistoryArenaData>(jsonData);
        }
        else
        {
            DebugCustom.LogWarning("No saved quests found!");
            return null;
        }
    }
    #endregion

    #region Challenge
    public static int GetLevelShockChangeBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelShockChangeBot}{id}", 1);
    }
    public static void SetLevelShockChangeBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelShockChangeBot}{id}", value);
    }

    public static int GetLevelDamageBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelDamageBot}{id}", 1);
    }
    public static void SetLevelDamageBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelDamageBot}{id}", value);
    }

    public static int GetLevelBonusBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelBonusBot}{id}", 1);
    }
    public static void SetLevelBonusBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelBonusBot}{id}", value);
    }

    public static int GetLevelDamageReductionBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelDamageReductionBot}{id}", 1);
    }
    public static void SetLevelDamageReductionBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelDamageReductionBot}{id}", value);
    }

    public static int GetLevelCoolDownBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelCoolDownBotQuantity}{id}", 1);
    }
    public static void SetLevelCoolDownBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelCoolDownBotQuantity}{id}", value);
    }

    public static int GetLevelRangeBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelRangeBotQuantity}{id}", 1);
    }
    public static void SetLevelRangeBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelRangeBotQuantity}{id}", value);
    }

    public static int GetLevelDurationBot(TypeBot id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_GetLevelDurationBotQuantity}{id}", 1);
    }
    public static void SetLevelDurationBot(TypeBot id, int value)
    {
        SavePrefs.SetInt($"{GameKeys.Key_GetLevelDurationBotQuantity}{id}", value);
    }


    public static bool IsBotChallengeUnlock(TypeBot typeBot)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_BotChallengeUnlock}{typeBot}", 0) == 1 ? true : false;
    }

    public static void BotChallengeUnlock(TypeBot typeBot)
    {
        SavePrefs.SetInt($"{GameKeys.Key_BotChallengeUnlock}{typeBot}", 1);
        EventDispatcher.PostEvent(EventID.OnBotChallengeUnlock, 0);
    }

    public static bool IsThemeSongUnlock(TypeSong typeSong)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ThemeSongUnlock}{typeSong}", 0) == 1 ? true : false;
    }

    public static void ThemeSongUnlock(TypeSong typeSong)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ThemeSongUnlock}{typeSong}", 1);
    }

    public static bool IsRelicUnlock(TypeRelic typeRelic)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_RelicUnlock}{typeRelic}", 0) == 1 ? true : false;
    }

    public static void RelicUnlock(TypeRelic typeRelic)
    {
        SavePrefs.SetInt($"{GameKeys.Key_RelicUnlock}{typeRelic}", 1);
    }

    public static bool IsX2BadgesEvent()
    {
        return SavePrefs.GetInt(GameKeys.KEY_IsX2BadgesEvent, 0) == 1 ? true : false;
    }

    public static void X2BadgesEvent(bool isX2)
    {
        SavePrefs.SetInt(GameKeys.KEY_IsX2BadgesEvent, isX2 ? 1 : 0);
        EventDispatcher.PostEvent(EventID.OnX2BadgesEvent, 0);
    }

    public static bool IsRandomRelicReward()
    {
        return SavePrefs.GetInt(GameKeys.KEY_RandomRelicReward, 0) == 1 ? true : false;
    }

    public static void RandomRelicReward(bool isRand)
    {
        SavePrefs.SetInt(GameKeys.KEY_RandomRelicReward, isRand ? 1 : 0);
    }

    //check new day
    public static string LastCheckNewDayChallenge
    {
        get
        {
            string timeStr = PlayerPrefs.GetString(GameKeys.KEY_LastCheckNewDayChallenge, "");
            return timeStr;
        }
        set
        {
            SavePrefs.SetString(GameKeys.KEY_LastCheckNewDayChallenge, value.ToString());
        }
    }

    public static bool CheckNewDayEventChallenge()
    {
        if(LastCheckNewDayChallenge == "")
        {
            LastCheckNewDayChallenge = DateTime.Now.ToString();
        }
        DateTime lastLogin = DateTime.Parse(LastCheckNewDayChallenge);
        DateTime now = DateTime.Now;

        bool isNewDay = now.Date > lastLogin.Date;

        if (isNewDay)
        {
            LastCheckNewDayChallenge = DateTime.Now.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly TimeSpan EventDuration = TimeSpan.FromDays(14);
    private static readonly DateTime FirstEventStartDate = DateTime.Now;

    public static bool IsDuringEvent(DateTime now)
    {
        int weeksPassed = (int)((now - FirstEventStartDate).TotalDays / 14);
        DateTime currentEventStart = FirstEventStartDate.AddDays(weeksPassed * 14);
        DateTime currentEventEnd = currentEventStart + EventDuration;

        return now >= currentEventStart && now < currentEventEnd;
    }

    public static bool HasEventEnded(DateTime now)
    {
        return !IsDuringEvent(now);
    }

    public static bool IsNewEventCycle(DateTime lastCheckedTime, DateTime now)
    {
        int lastCycle = (int)((lastCheckedTime - FirstEventStartDate).TotalDays / 14);
        int currentCycle = (int)((now - FirstEventStartDate).TotalDays / 14);

        return currentCycle > lastCycle;
    }

    public static DateTime LastCheckTimeChallenge
    {
        get
        {
            string timeStr = PlayerPrefs.GetString(GameKeys.KEY_LastCheckKey, DateTime.Now.ToString());
            return DateTime.Parse(timeStr);
        }
        set
        {
            SavePrefs.SetString(GameKeys.KEY_LastCheckKey, value.ToString());
        }
    }

    public static int TotalMissionChallenge
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_TotalMissionChallenge, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_TotalMissionChallenge, value);
        }
    }

    public static float TotalTimeUpgradeLab
    {
        get
        {
            return SavePrefs.GetFloat(GameKeys.Key_TotalTimeUpgradeLab, 0);
        }
        set
        {
            SavePrefs.SetFloat(GameKeys.Key_TotalTimeUpgradeLab, value);
        }
    }

    public static bool IsFirstStartEventChallenge()
    {
        return SavePrefs.GetInt($"{GameKeys.Key_IsFirstStartEventChallenge}", 0) == 1 ? true : false;
    }

    public static void FirstStartEventChallenge(bool isFirst)
    {
        SavePrefs.SetInt($"{GameKeys.Key_IsFirstStartEventChallenge}", isFirst == true ? 1 : 0);
    }

    public static void ResetEventChallenge()
    {
        FirstStartEventChallenge(false);
        TotalMissionChallenge = 0;
        foreach (MissionChallengeType missionType in Enum.GetValues(typeof(MissionChallengeType)))
        {
            SetIndexRankMissionChallenge(missionType, 1);
            SetProgressMissionChallenge(missionType, 0);
        }
    }

    public static void SetIndexRankMissionChallenge(MissionChallengeType mission, int indexRank)
    {
        if(indexRank > 3) indexRank = 3;
        SavePrefs.SetInt($"{GameKeys.Key_IndexRankMissionChallenge}{mission}", indexRank);
    }

    public static int GetIndexRankMissionChallenge(MissionChallengeType mission)
    {
        int indexRank = SavePrefs.GetInt($"{GameKeys.Key_IndexRankMissionChallenge}{mission}", 1);
        if (indexRank > 3) indexRank = 3;
        return indexRank;
    }

    public static void SetProgressMissionChallenge(MissionChallengeType mission, int progress)
    {
        SavePrefs.SetInt($"{GameKeys.Key_ProgressMissionChallenge}{mission}", progress);
    }

    public static int GetProgressMissionChallenge(MissionChallengeType mission)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_ProgressMissionChallenge}{mission}", 0);
    }

    public static void SaveAllRelicItems(RelicData newItem)
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathChallenge);

        AllChallengeData allItemsData = new AllChallengeData();

        allItemsData.items.Add(newItem);
        string jsonData = JsonUtility.ToJson(allItemsData, true);

        File.WriteAllText(filePath, jsonData);
        DebugCustom.Log("All items saved to file (old data cleared): " + filePath);
    }

    public static void SaveAllListRelicItems(List<RelicData> newItems)
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathChallenge);

        AllChallengeData allItemsData = new AllChallengeData();

        allItemsData.items = newItems;

        string jsonData = JsonUtility.ToJson(allItemsData, true);
        File.WriteAllText(filePath, jsonData);

        DebugCustom.Log("List of items saved to file (old data cleared): " + filePath);
    }


    public static AllChallengeData LoadAllRelicItems()
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathChallenge);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            AllChallengeData allItemsData = JsonUtility.FromJson<AllChallengeData>(jsonData);
            return allItemsData;
        }
        else
        {
            DebugCustom.LogWarning("File not found!");
            return null;
        }
    }
    #endregion

    #region banner Pack
    public static bool IsActiveBannerPack()
    {
        return SavePrefs.GetInt(GameKeys.Key_ActiveBannerPack, 0) == 1 ? true : false;
    }
    public static void ActiveBannerPack(bool isClaim)
    {
        SavePrefs.SetInt(GameKeys.Key_ActiveBannerPack, isClaim ? 1 : 0);
    }

    public static DateTime FirstOpenDate
    {
        get
        {
            if (!PlayerPrefs.HasKey(GameKeys.FirstOpenKey))
            {
                var now = DateTime.UtcNow;
                PlayerPrefs.SetString(GameKeys.FirstOpenKey, now.ToString());
                return now;
            }
            return DateTime.Parse(PlayerPrefs.GetString(GameKeys.FirstOpenKey));
        }
        set => PlayerPrefs.SetString(GameKeys.FirstOpenKey, value.ToString());
    }

    public static int LastBannerCycle
    {
        get => PlayerPrefs.GetInt(GameKeys.LastCycleKey, 0);
        set => PlayerPrefs.SetInt(GameKeys.LastCycleKey, value);
    }

    public static void ResetBannerCycle(DateTime now)
    {
        BuyBeginnerPack(0, false);
        BuyBeginnerPack(1, false);
        FirstOpenDate = now;
    }

    public static bool IsBuyBeginnerPack(int indexPack)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_BuyBeginnerPack}{indexPack}", 0) == 1 ? true : false;
    }

    public static void BuyBeginnerPack(int indexPack, bool isBuy)
    {
        SavePrefs.SetInt($"{GameKeys.Key_BuyBeginnerPack}{indexPack}", isBuy == true ? 1 : 0);
    }
    #endregion

    #region beginner quests reward
    public static bool IsClaimBeginnerQuest(BeginnerQuestID id)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_Beginnerclaimed}{id}", 0) == 1 ? true : false;
    }
    public static void ClaimBeginnerQuest(BeginnerQuestID id, bool isClaim)
    {
        SavePrefs.SetInt($"{GameKeys.Key_Beginnerclaimed}{id}", isClaim ? 1 : 0);
    }

    public static bool CheckNotiBeginnerQuest
    {
        get
        {
            var datas = Resources.LoadAll<BeginnerQuest_SO>("BeginnerQuest/");

            foreach (var data in datas)
            {
                if (data.isDone && !data.isClaim) return true;
            }
            return false;
        }
    }
    public static DateTime timeTargetBeginnerQuest
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeTargetBeginnerQuest);
        }
        set
        {
            SaveObject(GameKeys.Key_timeTargetBeginnerQuest, value);
        }
    }

    public static DateTime timeStart
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeStartBeginnerQuests);
        }
        set
        {
            SaveObject(GameKeys.Key_timeStartBeginnerQuests, value);
        }
    }
    public static int CountDaysLogin
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_CountDaysLoginBeginner, 0);
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_CountDaysLoginBeginner, value);
        }
    }
    public static DateTime timeStart_30minRemain
    {
        get
        {
            return LoadObject<DateTime>(GameKeys.Key_timeStart_30minRemainBeginner);
        }
        set
        {
            SaveObject(GameKeys.Key_timeStart_30minRemainBeginner, value);
        }
    }

    public static bool ActiveBeginnerQuests
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_ActiveBeginnerQuests, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_ActiveBeginnerQuests, 1);
        }
    }

    public static bool FirstInGameActiveBeginnerQuest
    {
        get
        {
            return SavePrefs.GetInt(GameKeys.Key_FirstInGameActiveBeginnerQuest, 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_FirstInGameActiveBeginnerQuest, 1);
        }
    }

    public static float GetBeginnerQuestProgress(BeginnerQuestID id)
    {
        return SavePrefs.GetFloat($"{GameKeys.Key_BeginnerQuestProgress}{id}", 0);
    }
    public static void SetBeginnerQuestProgress(BeginnerQuestID id, float progress)
    {
        if (timeTargetBeginnerQuest == default || DateTime.Now > timeTargetBeginnerQuest)
            return;
        EventDispatcher.PostEvent(EventID.OnBeginnerQuestProgressChanged, id);
        SavePrefs.SetFloat($"{GameKeys.Key_BeginnerQuestProgress}{id}", progress);
    }
    #endregion

    #region Double event Gold and Gem

/*    public static bool IsDoubleEventGold()
    {
        return SavePrefs.GetInt("DoubleEventGold", 0) == 1 ? true : false;
    }
    public static void DoubleEventGold(bool isActive)
    {
        SavePrefs.SetInt("DoubleEventGold", isActive ? 1 : 0);
    }

    public static bool IsDoubleEventGem()
    {
        return SavePrefs.GetInt("DoubleEventGem", 0) == 1 ? true : false;
    }
    public static void DoubleEventGem(bool isActive)
    {
        SavePrefs.SetInt("DoubleEventGem", isActive ? 1 : 0);
    }

    public static bool ActiveDoubleEventGold
    {
        get
        {
            return SavePrefs.GetInt("ActiveDoubleEventGold", 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt("ActiveDoubleEventGold", 1);
        }
    }

    public static bool ActiveDoubleEventGem
    {
        get
        {
            return SavePrefs.GetInt("ActiveDoubleEventGem", 0) == 1 ? true : false;
        }
        set
        {
            SavePrefs.SetInt("ActiveDoubleEventGem", 1);
        }
    }*/
    #endregion

    #region Relics
    public static bool IsRelicEquiped(TypeRelic id)
    {
        if (GetRelicEquiped(id) != 0)
        {
            return true;
        }
        return false;
    }

    public static int GetRelicEquiped(TypeRelic id)
    {
        return SavePrefs.GetInt($"{GameKeys.KEY_RelicEquip}{id}", 0);
    }

    public static void SetRelicEquiped(TypeRelic id)
    {
        SavePrefs.SetInt($"{GameKeys.KEY_RelicEquip}{id}", 1/*(int)id*/);
        //IndexSlotRelic++;
        EventDispatcher.PostEvent(EventID.OnLevelCardChanged, 0);
    }

    public static void UnEquipedRelic(TypeRelic id)
    {
        //SavePrefs.DeleteKey($"{GameKeys.KEY_RelicEquip}{id}");
        SavePrefs.SetInt($"{GameKeys.KEY_RelicEquip}{id}", 0);
        //IndexSlotRelic--;
        EventDispatcher.PostEvent(EventID.OnLevelCardChanged, 0);
    }

    public static bool IsMaxSlotRelic()
    {
        float totalRelicEquip = 0;
        foreach (TypeRelic type in Enum.GetValues(typeof(TypeRelic)))
        {
            if(IsRelicEquiped(type))
            {
                totalRelicEquip++;
            }
        }
        if (totalRelicEquip == maxSlotRelic)
        {
            return true;
        }
        return false;
    }

    public static int IndexSlotRelic()
    {
        int totalRelicEquip = 0;
        foreach (TypeRelic type in Enum.GetValues(typeof(TypeRelic)))
        {
            if (IsRelicEquiped(type))
            {
                totalRelicEquip++;
            }
        }
        return totalRelicEquip;
    }

    public static int maxSlotRelic = 10;
/*    public static int IndexSlotRelic
    {
        get { return SavePrefs.GetInt(GameKeys.Key_IndexSlotRelic, 0); }
        set
        {
            SavePrefs.SetInt(GameKeys.Key_IndexSlotRelic, value);
            EventDispatcher.PostEvent(EventID.OnRefreshRelicEquip, 0);
        }
    }*/

    #endregion

    #region Music Challenge
    public static int IsThemeSongUsing()
    {
        return SavePrefs.GetInt(GameKeys.Key_ThemeSongUsing, 1);
    }

    public static void ThemeSongUsing(TypeSong typeSong)
    {
        SavePrefs.SetInt(GameKeys.Key_ThemeSongUsing, (int)typeSong);
        EventDispatcher.PostEvent(EventID.OnChangeSongTheme, 0);
    }
    #endregion

    #region Claimed Reward
    public static string GetLastClaimdX2OfflineRewardDate()
    {
        return SavePrefs.GetString(GameKeys.Key_LastClaimdX2OfflineRewardDate, "");
    }

    public static void SetLastClaimdX2OfflineRewardDate()
    {
        SavePrefs.SetString(GameKeys.Key_LastClaimdX2OfflineRewardDate, DateTime.Now.ToString("yyyy-MM-dd"));
    }
    
    public static int GetClaimedX2OfflineReward()
    {
        return SavePrefs.GetInt(GameKeys.Key_ClaimedX2OfflineReward, 0);
    }

    public static void SetClaimedX2OfflineReward(int purchaseCount)
    {
        SavePrefs.SetInt(GameKeys.Key_ClaimedX2OfflineReward, purchaseCount);
    }
    #endregion

    #region TechSystem

    public static int GetPitySystem()
    {
        return SavePrefs.GetInt(GameKeys.Key_PitySystem, 0);
    }

    public static void SetPitySystem(int pitySystem)
    {
        SavePrefs.SetInt(GameKeys.Key_PitySystem, pitySystem);
    }

    public static int GetUniqueTechSystem()
    {
        return SavePrefs.GetInt(GameKeys.Key_UniqueTechSystem, 0);
    }

    public static void SetUniqueTechSystem()
    {
        int uniqueTech = GetUniqueTechSystem() + 1;
        SavePrefs.SetInt(GameKeys.Key_UniqueTechSystem, uniqueTech);
    }

    public static int GetLevelTech(TypeTech typeTech, int uniqueTech)
    {
        return SavePrefs.GetInt($"{GameKeys.Key_LevelTech}{typeTech}{uniqueTech}", 0);
    }
    
    public static void SetLevelTech(TypeTech typeTech, int uniqueTech, int pitySystem)
    {
        SavePrefs.SetInt($"{GameKeys.Key_LevelTech}{typeTech}{uniqueTech}", pitySystem);
    }

    #endregion
}
[System.Serializable]
public class UserID
{
    public string userID;
}

[System.Serializable]
public class User_BossKilled : UserID
{
    public string name;
    public int idAvatar;
    public float bossKilled;
}

#region MileStone Data

[System.Serializable]
public class ItemData
{
    public int IndexWave;
    public TypeMileStoneCateGory Category;
    public WorldType World;

    public ItemData(int indexWave, TypeMileStoneCateGory category, WorldType world)
    {
        IndexWave = indexWave;
        Category = category;
        World = world;
    }
}

[System.Serializable]
public class AllItemMileStoneData
{
    public List<ItemData> items = new List<ItemData>();
}

#endregion

#region Challenge Data

[System.Serializable]
public class AllChallengeData
{
    public List<RelicData> items = new List<RelicData>();
}

#endregion

#region Daily Quest

[System.Serializable]
public class ItemDailyQuestData
{
    public int countQuest;
    public DailyQuestType dailyQuestType;
    public DailyQuestSpecialType dailyQuestSpecialType;

    public ItemDailyQuestData(int countQuest, DailyQuestType dailyQuestType = DailyQuestType.StartBattle, 
        DailyQuestSpecialType dailyQuestSpecialType = DailyQuestSpecialType.DestroyEnemiesHighlight)
    {
        this.countQuest = countQuest;
        this.dailyQuestType = dailyQuestType;
        this.dailyQuestSpecialType = dailyQuestSpecialType;
    }
}

[System.Serializable]
public class AllDailyQuestData
{
    public List<ItemDailyQuestData> items = new List<ItemDailyQuestData>();
}

#endregion

#region data HistoryArena

[System.Serializable]
public class AllHistoryArenaData
{
    public List<ItemHistoryArenaData> items = new List<ItemHistoryArenaData>();
}

[System.Serializable]
public class ItemHistoryArenaData
{
    public TypeRank typeRank;
    public float maxWave;
    public string time;
    public int indexRank;

    public ItemHistoryArenaData(TypeRank typeRank, float maxWave, string time, int indexRank)
    {
        this.typeRank = typeRank;
        this.maxWave = maxWave;
        this.time = time;
        this.indexRank = indexRank;
    }
}

#endregion

#region Data TechSystem

[System.Serializable]
public class AllTechSystem
{
    public List<ItemTechSystemData> items = new List<ItemTechSystemData>();
}

[System.Serializable]
public class ItemTechSystemData
{
    public TypeRarityTech typeRarityTech;
    public TypeTech typeTech;
    public TypeClassTechSystem typeClassTechSystem;
    public int level;
    public bool isUniqueEffect;
    public int uniqueTech;

    public ItemTechSystemData(TypeTech typeTech, TypeRarityTech typeRarityTech, 
        TypeClassTechSystem typeClassTechSystem, int level, bool isUniqueEffect, int uniqueTech)
    {
        this.typeTech = typeTech;
        this.typeRarityTech = typeRarityTech;
        this.typeClassTechSystem = typeClassTechSystem;
        this.level = level;
        this.isUniqueEffect = isUniqueEffect;
        this.uniqueTech = uniqueTech;
    }
}

public static class TechSystemStorage
{
    private const string _prefsKey = GameKeys.AllTechSystem;
    private static AllTechSystem _cache;

    public static AllTechSystem LoadData()
    {
        if (_cache != null) return _cache;

        if (PlayerPrefs.HasKey(_prefsKey))
        {
            try
            {
                string encoded = PlayerPrefs.GetString(_prefsKey);
                string json = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                _cache = JsonUtility.FromJson<AllTechSystem>(json) ?? new AllTechSystem();
            }
            catch
            {
                DebugCustom.LogWarning("Corrupted techsystem data, resetting.");
                _cache = new AllTechSystem();
            }
        }
        else
        {
            _cache = new AllTechSystem();
        }

        return _cache;
    }

    private static void SaveData()
    {
        string json = JsonUtility.ToJson(LoadData());
        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        PlayerPrefs.SetString(_prefsKey, encoded);
        PlayerPrefs.Save();
        DebugCustom.Log("TechSystem data saved.");
    }

    /// <summary>
    /// Thêm mới một item vào hệ thống.
    /// </summary>
    public static void AddItem(ItemTechSystemData newItem)
    {
        var data = LoadData();
        data.items.Add(newItem);
        SaveData();
    }

    /// <summary>
    /// Cập nhật level của một item, tìm theo class + rarity + techType.
    /// Trả về false nếu không tìm thấy.
    /// </summary>
    public static bool UpdateLevel(
        TypeClassTechSystem classType,
        TypeTech typeTech,
        int newLevel)
    {
        var data = LoadData();
        // Dùng loop để tránh allocations
        for (int i = 0; i < data.items.Count; i++)
        {
            var item = data.items[i];
            if (item.typeClassTechSystem == classType &&
                item.typeTech == typeTech)
            {
                if (item.level == newLevel)
                    return true; // không thay đổi

                item.level = newLevel;
                SaveData();
                DebugCustom.Log(
                    $"Updated level [{classType}/{typeTech}] -> {newLevel}");
                return true;
            }
        }

        DebugCustom.LogWarning(
            $"No item found for {classType}/{typeTech} to update.");
        return false;
    }

    public static bool UpgrdadeRarityTech(
        TypeClassTechSystem classType,
        TypeTech typeTech,
        TypeRarityTech typeRarityTech)
    {
        var data = LoadData();
        // Dùng loop để tránh allocations
        for (int i = 0; i < data.items.Count; i++)
        {
            var item = data.items[i];
            if (item.typeClassTechSystem == classType &&
                item.typeTech == typeTech)
            {
                if (item.typeRarityTech == typeRarityTech)
                    return true; // không thay đổi

                item.typeRarityTech = typeRarityTech;
                item.level = 1;
                SaveData();
                DebugCustom.Log(
                    $"Updated level [{classType}/{typeTech}] -> {typeRarityTech}");
                return true;
            }
        }

        DebugCustom.LogWarning(
            $"No item found for {classType}/{typeTech} to update.");
        return false;
    }

    public static void MergeTech(
    TypeTech typeTech,
    int uniqueTech)
    {
        
    }

    /// <summary>
    /// Xóa tất cả item khớp classType và rarityType.
    /// Trả về số lượng item đã xóa.
    /// </summary>
    public static int DeleteByClassAndRarity(
        TypeClassTechSystem classType,
        TypeTech typeTech)
    {
        var data = LoadData();
        int removedCount = 0;

        // Duyệt ngược để xoá an toàn
        for (int i = data.items.Count - 1; i >= 0; i--)
        {
            var item = data.items[i];
            if (item.typeClassTechSystem == classType &&
                item.typeTech == typeTech)
            {
                data.items.RemoveAt(i);
                removedCount++;
            }
        }

        if (removedCount > 0)
        {
            SaveData();
            DebugCustom.Log(
                $"Deleted {removedCount} item(s) for Class={classType}, Rarity={typeTech}");
        }
        else
        {
            DebugCustom.LogWarning(
                $"No items found to delete for Class={classType}, Rarity={typeTech}");
        }

        return removedCount;
    }
}

#endregion

/*#region Wrapper GameDatas Json

[System.Serializable]
public class GameDatasWrapper
{
    public float gold;
    public float gem;
    public float powerStone;
    public float tourament;
    public float badges;
    public float armorSphere;
    public float powerSphere;
    public float engineSphere;
    public float cryogenicSphere;

    public bool activeX2;

    public int countBuyXUpgrader;
    public Dictionary<string, int> levelUpgraderInforTower = new Dictionary<string, int>();
    public Dictionary<string, int> levelUpgraderGroupTower = new Dictionary<string, int>();
    public Dictionary<string, bool> clusterUpgrader = new Dictionary<string, bool>();

    public DateTime lastQuestCheckDate;
    public int countSpinCard;
    public int levelCardUnlock;
    public int indexSlotCardUnlockByGold;
    public int indexSlotCard;
    public Dictionary<string, bool> unlockSlotCard = new Dictionary<string, bool>();
    public Dictionary<string, bool> unlockCard = new Dictionary<string, bool>();
    public Dictionary<string, int> levelCard = new Dictionary<string, int>();
    public Dictionary<string, int> amountCard = new Dictionary<string, int>();
    public Dictionary<string, int> cardEquipped = new Dictionary<string, int>();

    public int countSlotLabUnlock;
    public Dictionary<string, int> levelSubjectLab = new Dictionary<string, int>();
    public Dictionary<string, DateTime> timeTargetSubject = new Dictionary<string, DateTime>();
    public Dictionary<string, int> slotSave = new Dictionary<string, int>();
    public Dictionary<string, bool> clusterUnlockLabInfor = new Dictionary<string, bool>();

    public bool isFirstDoneWave40;
    public string startTimeFirstInGame;
    public bool isTutPlay;
    public bool isTutLab;
    public bool isTutSpeed;
    public bool isTutBuildPhase2;
    public bool isFirstTimeGoHome;
    public bool isFirstClaimOfflineReward;
    public bool isResetData;
    public bool isTut0Upgrade;
    public bool isTutPlayDemo;
    public bool isEndTutorial;
    public bool isFirstInGame;
    public bool isActiveTutorial;

    public int currentWorld;
    public Dictionary<string, int> highestWaveInWorld = new Dictionary<string, int>();
    public int highestWorld;

    public float timeGameMax;
    public float timeGamePlaySpeed;

    public int reviveTowerCount;
    public DateTime timeTargetFullRevive;

    public bool isX2;
    public bool isX3;
    public bool isX4;
    public DateTime timeClaimFreeGemTarget;
    public DateTime timeClaimFreeGoldTarget;

    public DateTime timeTargetBonusGold;
    public int xBonusGoldDailyQuest;
    public float timeBonusGold;

    public Dictionary<string, bool> unlockUltimateWeapon = new Dictionary<string, bool>();
    public Dictionary<string, int> levelUltimateQuantity = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateDmgBonus = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateDurationBonus = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateSizeBonus = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateSlowBonus = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateCooldown = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateChangeCost = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateDmgScale = new Dictionary<string, int>();
    public Dictionary<string, int> levelUltimateAngle = new Dictionary<string, int>();
    public int unlockSlotUltimateWeapon;

    public bool removeAdsForever;

    public float userPower;

    public int secondToGetReward;
    public int secondsAccumulate;
    public DateTime lastLogoutTime;

    public bool isUsingPremiumMileStones;
    public AllItemMileStoneData allItemMileStoneData;

    public bool isUnlockRanking;
    public bool isUnlockAchievement;
    public bool isUnlockLuckyDraw;
    public bool isUnlockDailyGift;
    public bool isUnlockLab;
    public bool isUnlockFeatureCard;
    public bool isUnlockFeatureArena;

    public bool isSpinFree;
    public int countSpinFree;
    public long lastSpinTime;

    public Dictionary<string, bool> dayDailyReceived = new Dictionary<string, bool>();
    public string lastLoginDateDailyReward;
    public int currentDayDailyReward;
    public int accumulateDailyReward;
    public Dictionary<string, bool> claimedAccumulateDailyReward = new Dictionary<string, bool>();

    public string lastLoginDateDailyQuest;
    public Dictionary<string, bool> claimedQuestReward = new Dictionary<string, bool>();
    public Dictionary<string, bool> claimedAccumulateDailyGift = new Dictionary<string, bool>();
    public Dictionary<string, int> totalQuestDone = new Dictionary<string, int>();
    public int accumulateDailyGift;
    public bool isChangeQuestReward;
    public AllDailyQuestData allDailyQuestData;

    public Dictionary<string, int> waveInResume = new Dictionary<string, int>();
    public int worldInResume;
    public Dictionary<string, bool> resumeWave = new Dictionary<string, bool>();
    public bool isActiveWelcomeBack;

    public Dictionary<string, bool> unlockAvatar = new Dictionary<string, bool>();
    public string userId;
    public string userName;
    public int idAvatar;
    public DateTime startDate;
    public Dictionary<string, float> dataProfile = new Dictionary<string, float>();
    public int countUpgraderTime;
    public int countDoneQuest;
    public int countLoseGame;
    public int countEnemyDestroy;

    public bool isFirstPlayArena;
    public int totalArenaRankPlay;
    public int totalArenaRankActive;
    public int currentRank;
    public int totalReplayArena;
    public int highestRank;
    public Dictionary<string, int> indexRank = new Dictionary<string, int>();
    public Dictionary<string, int> highestWaveInRank = new Dictionary<string, int>();
    public bool isResetTournamentArena;
    public bool isClaimRewardArenaRank;
    public AllHistoryArenaData allHistoryArenaData;

    public Dictionary<string, int> levelShockChangeBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelDamageBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelBonusBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelDamageReductionBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelCoolDownBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelRangeBot = new Dictionary<string, int>();
    public Dictionary<string, int> levelDurationBot = new Dictionary<string, int>();
    public Dictionary<string, bool> botChallengeUnlock = new Dictionary<string, bool>();
    public Dictionary<string, bool> themeSongUnlock = new Dictionary<string, bool>();
    public Dictionary<string, bool> relicUnlock = new Dictionary<string, bool>();
    public bool isX2BadgesEvent;
    public bool isRandomRelicReward;
    public string lastCheckNewDayChallenge;
    public DateTime lastCheckTimeChallenge;
    public int totalMissionChallenge;
    public bool isFirstStartEventChallenge;
    public Dictionary<string, int> indexRankMissionChallenge = new Dictionary<string, int>();
    public Dictionary<string, int> progressMissionChallenge = new Dictionary<string, int>();

    public bool isActiveBannerPack;
    public DateTime firstOpenDate;
    public int lastBannerCycle;
    public Dictionary<string, bool> buyBeginnerPack = new Dictionary<string, bool>();

    public Dictionary<string, bool> claimBeginnerQuest = new Dictionary<string, bool>();
    public DateTime timeTargetBeginnerQuest;
    public DateTime timeStart;
    public int countDaysLogin;
    public DateTime timeStart30minRemain;
    public bool activeBeginnerQuests;
    public bool firstInGameActiveBeginnerQuest;
    public Dictionary<string, float> beginnerQuestProgress = new Dictionary<string, float>();

    public Dictionary<string, int> relicEquipped = new Dictionary<string, int>();
    public int indexSlotRelic;

    public int themeSongUsing;
    public DateTime lastModified;
    public int moreTurnPurchaseCount;
    public int claimedX2OfflineReward;

    public GameDatasWrapper()
    {
        lastModified = DateTime.UtcNow;
        gold = GameDatas.Gold;
        gem = GameDatas.Gem;
        powerStone = GameDatas.PowerStone;
        tourament = GameDatas.Tourament;
        badges = GameDatas.Badges;
        armorSphere = GameDatas.ArmorSphere;
        powerSphere = GameDatas.PowerSphere;
        engineSphere = GameDatas.EngineSphere;
        cryogenicSphere = GameDatas.CryogenicSphere;

        activeX2 = GameDatas.activeX2;

        countBuyXUpgrader = GameDatas.CountBuyXUpgrader;
        foreach (UpgraderID id in Enum.GetValues(typeof(UpgraderID)))
            levelUpgraderInforTower[$"{GameKeys.KEY_LevelUpgraderInforTower}{id}"] = GameDatas.GetLevelUpgraderInforTower(id);
        foreach (UpgraderGroupID id in Enum.GetValues(typeof(UpgraderGroupID)))
            levelUpgraderGroupTower[$"{GameKeys.KEY_LevelUpgraderGroupTower}{id}"] = GameDatas.GetLevelUpgraderGroupTower(id);
        foreach (UpgraderCategory id in Enum.GetValues(typeof(UpgraderCategory)))
            clusterUpgrader[$"{GameKeys.KEY_ClusterUpgrader}{id}"] = GameDatas.IsUnlockClusterUpgrader(id);

        lastQuestCheckDate = GameDatas.LastQuestCheckDate;
        countSpinCard = GameDatas.countSpinCard;
        levelCardUnlock = GameDatas.levelCardUnlock;
        indexSlotCardUnlockByGold = GameDatas.IndexSlotCardUnlockByGold;
        indexSlotCard = GameDatas.IndexSlotCard;
        for (int i = 0; i < 100; i++) 
            unlockSlotCard[$"{GameKeys.KEY_IsUnlockSlotCard}{i}"] = GameDatas.IsUnlockSlotCard(i);
        foreach (CardID id in Enum.GetValues(typeof(CardID)))
        {
            unlockCard[$"{GameKeys.KEY_IsUnlockCard}{id}"] = GameDatas.IsUnlockCard(id);
            levelCard[$"{GameKeys.KEY_CardLevel}{id}"] = GameDatas.GetLevelCard(id);
            amountCard[$"{GameKeys.KEY_CardAmount}{id}"] = GameDatas.GetAmountCard(id);
            cardEquipped[$"{GameKeys.KEY_CardEquip}{id}"] = GameDatas.GetCardEquiped(id);
        }

        countSlotLabUnlock = GameDatas.CountSlotLabUnlock;
        foreach (IdSubjectType id in Enum.GetValues(typeof(IdSubjectType)))
        {
            levelSubjectLab[$"{GameKeys.KEY_LevelSubjectLab}{id}"] = GameDatas.GetLevelSubjectLab(id);
            timeTargetSubject[$"{GameKeys.KEY_TimeTargetSubjectLab}{id}"] = GameDatas.GetTimeTargetSubject(id);
        }
        for (int i = 0; i < 10; i++)
            slotSave[$"{GameKeys.KEY_SlotSave}{i}"] = GameDatas.SlotSave(i);
        foreach (LabCategory id in Enum.GetValues(typeof(LabCategory)))
            clusterUnlockLabInfor[$"{GameKeys.KEY_UnlockLabInfor}{id}"] = GameDatas.IsClusterUnlockLabInfor(id);

        isFirstDoneWave40 = GameDatas.IsFirstDoneWave40;
        startTimeFirstInGame = GameDatas.StartTimeFirstInGame;
        isTutPlay = GameDatas.IsTut_Play;
        isTutLab = GameDatas.isTutLab;
        isTutSpeed = GameDatas.isTutSpeed;
        isTutBuildPhase2 = GameDatas.isTutBuildPhase2;
        isFirstTimeGoHome = GameDatas.IsFirstTimeGoHome;
        isFirstClaimOfflineReward = GameDatas.IsFirstClaimOfflineReward;
        isResetData = GameDatas.IsResetData;
        isTut0Upgrade = GameDatas.IsTut_0_Upgrade;
        isTutPlayDemo = GameDatas.IsTut_PlayDemo;
        isEndTutorial = GameDatas.IsEndTutorial;
        isFirstInGame = GameDatas.IsFirstInGame;
        isActiveTutorial = GameDatas.IsActiveTutorial;
        moreTurnPurchaseCount = GameDatas.GetMoreTurnPurchaseCount();
        claimedX2OfflineReward = GameDatas.GetClaimedX2OfflineReward();

        currentWorld = GameDatas.CurrentWorld;
        for (int i = 0; i < 100; i++)
            highestWaveInWorld[$"{GameKeys.KEY_HighestWaveInWorld}{i}"] = GameDatas.GetHighestWaveInWorld(i);
        highestWorld = GameDatas.GetHighestWorld();

        timeGameMax = GameDatas.timeGameMax;
        timeGamePlaySpeed = GameDatas.TimeGamePlaySpeed;

        reviveTowerCount = GameDatas.ReviveTowerCount;
        timeTargetFullRevive = GameDatas.timeTargetFullRevive;

        isX2 = GameDatas.isX2;
        isX3 = GameDatas.isX3;
        isX4 = GameDatas.isX4;
        timeClaimFreeGemTarget = GameDatas.timeClaimFreeGem_Target;
        timeClaimFreeGoldTarget = GameDatas.timeClaimFreeGold_Target;

        timeTargetBonusGold = GameDatas.timeTargetBonusGold;
        xBonusGoldDailyQuest = GameDatas.XBonusGoldDailyQuest;
        timeBonusGold = GameDatas.TimeBonusGold;

        foreach (UW_ID id in Enum.GetValues(typeof(UW_ID)))
        {
            unlockUltimateWeapon[$"{GameKeys.Key_isUnlockUltimateWeapon}{id}"] = GameDatas.isUnlockUltimateWeapon(id);
            levelUltimateQuantity[$"{GameKeys.Key_GetLevelUltimateQuantity}{id}"] = GameDatas.GetLevelUltimateQuantity(id);
            levelUltimateDmgBonus[$"{GameKeys.Key_GetLevelUltimateDmgBonus}{id}"] = GameDatas.GetLevelUltimateDmgBonus(id);
            levelUltimateDurationBonus[$"{GameKeys.Key_LevelUltimateDurationBonus}{id}"] = GameDatas.GetLevelUltimateDurationBonus(id);
            levelUltimateSizeBonus[$"{GameKeys.Key_LevelUltimateSizeBonus}{id}"] = GameDatas.GetLevelUltimateSizeBonus(id);
            levelUltimateSlowBonus[$"{GameKeys.Key_LevelUltimateSlowBonus}{id}"] = GameDatas.GetLevelUltimateSlowBonus(id);
            levelUltimateCooldown[$"{GameKeys.Key_GetLevelUltimateCooldown}{id}"] = GameDatas.GetLevelUltimateCooldown(id);
            levelUltimateChangeCost[$"{GameKeys.Key_LevelUltimateChangeCost}{id}"] = GameDatas.GetLevelUltimateChangeCost(id);
            levelUltimateDmgScale[$"{GameKeys.Key_GetLevelUltimateDmgScale}{id}"] = GameDatas.GetLevelUltimateDmgScale(id);
            levelUltimateAngle[$"{GameKeys.Key_GetLevelUltimateAngle}{id}"] = GameDatas.GetLevelUltimateAngle(id);
        }
        unlockSlotUltimateWeapon = GameDatas.IsUnlockSLotUltimateWeapon();

        removeAdsForever = GameDatas.RemoveAdsForever;

        userPower = GameDatas.userPower;

        secondToGetReward = GameDatas.secondToGetReward;
        secondsAccumulate = GameDatas.SecondsAccumulate;
        lastLogoutTime = GameDatas.LastLogoutTime;

        isUsingPremiumMileStones = GameDatas.IsUsingPremiumMileStones;
        allItemMileStoneData = GameDatas.LoadAllItems() ?? new AllItemMileStoneData();

        isUnlockRanking = GameDatas.isUnlockRanking;
        isUnlockAchievement = GameDatas.isUnlockAchievement;
        isUnlockLuckyDraw = GameDatas.isUnlockLuckyDraw;
        isUnlockDailyGift = GameDatas.isUnlockDailyGift;
        isUnlockLab = GameDatas.isUnlockLab;
        isUnlockFeatureCard = GameDatas.IsUnlockFeatureCard;
        isUnlockFeatureArena = GameDatas.IsUnlockFeatureArena;

        isSpinFree = GameDatas.IsSpinFree;
        countSpinFree = GameDatas.CountSpinFree;
        lastSpinTime = long.Parse(SavePrefs.GetString(GameKeys.Key_lastSpinTime, "0"));

        for (int day = 1; day <= 30; day++)
        {
            dayDailyReceived[$"{GameKeys.Key_IsDayReceived}{day}"] = GameDatas.IsDayDailyReceived(day);
            claimedAccumulateDailyReward[$"{GameKeys.Key_ClaimedAccumulateDailyReward}{day}"] = GameDatas.IsClaimedAccumulateDailyReward(day);
        }
        lastLoginDateDailyReward = GameDatas.IsLastLoginDateDailyReward();
        currentDayDailyReward = GameDatas.IsCurrentDayDailyReward();
        accumulateDailyReward = GameDatas.GetAccumulateDailyReward();

        lastLoginDateDailyQuest = GameDatas.IsLastLoginDateDailyQuest();
        foreach (Enum typeQuest in Enum.GetValues(typeof(DailyQuestType))) // 假设使用 DailyQuestType
            claimedQuestReward[$"{GameKeys.Key_ClaimedAccumulateDailyGift}{typeQuest}"] = GameDatas.IsClaimedQuestReward(typeQuest);
        for (int day = 1; day <= 30; day++)
            claimedAccumulateDailyGift[$"{GameKeys.Key_ClaimedAccumulateDailyGift}{day}"] = GameDatas.IsClaimedAccumulateDailyGift(day);
        foreach (Enum typeQuest in Enum.GetValues(typeof(DailyQuestType)))
            totalQuestDone[$"{GameKeys.Key_TotalQuestDone}{typeQuest}"] = GameDatas.GetTotalQuestDone(typeQuest);
        accumulateDailyGift = GameDatas.GetAccumulateDailyGift();
        isChangeQuestReward = GameDatas.IsChangeQuestReward;
        allDailyQuestData = GameDatas.LoadAllQuests() ?? new AllDailyQuestData();

        for (int world = 0; world < 100; world++)
        {
            waveInResume[$"{GameKeys.Key_GetWaveInResume}{world}"] = GameDatas.GetWaveInResume(world);
            resumeWave[$"{GameKeys.Key_ResumeWave}{world}"] = GameDatas.IsResumeWave(world);
        }
        worldInResume = GameDatas.GetWorldInResume();
        isActiveWelcomeBack = GameDatas.IsActiveWelcomeBack();

        foreach (TypeAvatar type in Enum.GetValues(typeof(TypeAvatar)))
            unlockAvatar[$"{GameKeys.Key_IsUnlockAvatar} {type}"] = GameDatas.IsUnlockAvatar(type);
        userId = GameDatas.userID;
        userName = GameDatas.user_name;
        idAvatar = GameDatas.id_avatar;
        startDate = GameDatas.StartDate;
        foreach (IDInfo id in Enum.GetValues(typeof(IDInfo)))
            dataProfile[$"{GameKeys.Key_DataProfile}{id}"] = GameDatas.GetDataProfile(id);
        countUpgraderTime = GameDatas.countUpgraderTime;
        countDoneQuest = GameDatas.countDoneQuest;
        countLoseGame = GameDatas.countLoseGame;
        countEnemyDestroy = GameDatas.countEnemyDestroy;

        isFirstPlayArena = GameDatas.IsFirstPlayArena;
        totalArenaRankPlay = GameDatas.TotalArenaRankPlay;
        totalArenaRankActive = GameDatas.TotalArenaRankActive;
        currentRank = GameDatas.CurrentRank;
        totalReplayArena = GameDatas.TotalReplayArena;
        highestRank = GameDatas.GetHighestRank();
        foreach (TypeRank type in Enum.GetValues(typeof(TypeRank)))
        {
            indexRank[$"{GameKeys.KEY_IndexRank}{type}"] = GameDatas.GetIndexRank(type);
            highestWaveInRank[$"{GameKeys.KEY_HighestWaveInRank}{type}"] = GameDatas.GetHighestWaveInRank(type);
        }
        isResetTournamentArena = GameDatas.IsResetTouramentArena();
        isClaimRewardArenaRank = GameDatas.IsClaimRewardArenaRank();
        allHistoryArenaData = GameDatas.LoadAllHistoryArenas() ?? new AllHistoryArenaData();

        foreach (TypeBot id in Enum.GetValues(typeof(TypeBot)))
        {
            levelShockChangeBot[$"{GameKeys.Key_LevelShockChangeBot}{id}"] = GameDatas.GetLevelShockChangeBot(id);
            levelDamageBot[$"{GameKeys.Key_LevelDamageBot}{id}"] = GameDatas.GetLevelDamageBot(id);
            levelBonusBot[$"{GameKeys.Key_LevelBonusBot}{id}"] = GameDatas.GetLevelBonusBot(id);
            levelDamageReductionBot[$"{GameKeys.Key_LevelDamageReductionBot}{id}"] = GameDatas.GetLevelDamageReductionBot(id);
            levelCoolDownBot[$"{GameKeys.Key_GetLevelCoolDownBotQuantity}{id}"] = GameDatas.GetLevelCoolDownBot(id);
            levelRangeBot[$"{GameKeys.Key_GetLevelRangeBotQuantity}{id}"] = GameDatas.GetLevelRangeBot(id);
            levelDurationBot[$"{GameKeys.Key_GetLevelDurationBotQuantity}{id}"] = GameDatas.GetLevelDurationBot(id);
            botChallengeUnlock[$"{GameKeys.Key_BotChallengeUnlock}{id}"] = GameDatas.IsBotChallengeUnlock(id);
        }
        foreach (TypeSong id in Enum.GetValues(typeof(TypeSong)))
            themeSongUnlock[$"{GameKeys.Key_ThemeSongUnlock}{id}"] = GameDatas.IsThemeSongUnlock(id);
        foreach (TypeRelic id in Enum.GetValues(typeof(TypeRelic)))
            relicUnlock[$"{GameKeys.Key_RelicUnlock}{id}"] = GameDatas.IsRelicUnlock(id);
        isX2BadgesEvent = GameDatas.IsX2BadgesEvent();
        isRandomRelicReward = GameDatas.IsRandomRelicReward();
        lastCheckNewDayChallenge = GameDatas.LastCheckNewDayChallenge;
        lastCheckTimeChallenge = GameDatas.LastCheckTimeChallenge;
        totalMissionChallenge = GameDatas.TotalMissionChallenge;
        isFirstStartEventChallenge = GameDatas.IsFirstStartEventChallenge();
        foreach (MissionChallengeType mission in Enum.GetValues(typeof(MissionChallengeType)))
        {
            indexRankMissionChallenge[$"{GameKeys.Key_IndexRankMissionChallenge}{mission}"] = GameDatas.GetIndexRankMissionChallenge(mission);
            progressMissionChallenge[$"{GameKeys.Key_ProgressMissionChallenge}{mission}"] = GameDatas.GetProgressMissionChallenge(mission);
        }

        isActiveBannerPack = GameDatas.IsActiveBannerPack();
        firstOpenDate = GameDatas.FirstOpenDate;
        lastBannerCycle = GameDatas.LastBannerCycle;
        for (int i = 0; i < 2; i++) 
            buyBeginnerPack[$"{GameKeys.Key_BuyBeginnerPack}{i}"] = GameDatas.IsBuyBeginnerPack(i);

        foreach (BeginnerQuestID id in Enum.GetValues(typeof(BeginnerQuestID)))
        {
            claimBeginnerQuest[$"{GameKeys.Key_Beginnerclaimed}{id}"] = GameDatas.IsClaimBeginnerQuest(id);
            beginnerQuestProgress[$"{GameKeys.Key_BeginnerQuestProgress}{id}"] = GameDatas.GetBeginnerQuestProgress(id);
        }
        timeTargetBeginnerQuest = GameDatas.timeTargetBeginnerQuest;
        timeStart = GameDatas.timeStart;
        countDaysLogin = GameDatas.CountDaysLogin;
        timeStart30minRemain = GameDatas.timeStart_30minRemain;
        activeBeginnerQuests = GameDatas.ActiveBeginnerQuests;
        firstInGameActiveBeginnerQuest = GameDatas.FirstInGameActiveBeginnerQuest;

        foreach (TypeRelic id in Enum.GetValues(typeof(TypeRelic)))
            relicEquipped[$"{GameKeys.KEY_RelicEquip}{id}"] = GameDatas.GetRelicEquiped(id);
        indexSlotRelic = GameDatas.IndexSlotRelic;

        themeSongUsing = GameDatas.IsThemeSongUsing();
    }
}

public static class GameDatasSerializer
{
    private static string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "GameDatas.json");
    }

    public static void SaveToJson()
    {
        try
        {
            GameDatasWrapper wrapper = new GameDatasWrapper();
            wrapper.lastModified = DateTime.UtcNow; // Cập nhật thời gian sửa đổi
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(GetFilePath(), json);
            Debug.Log($"GameDatas saved to {GetFilePath()}");

            // Lưu lên đám mây nếu đã đăng nhập
            ServiceManager.SaveToCloud();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save GameDatas to JSON: {ex.Message}");
        }
    }

    public static GameDatasWrapper LoadFromJson()
    {
        try
        {
            string filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                GameDatasWrapper wrapper = JsonUtility.FromJson<GameDatasWrapper>(json);
                Debug.Log($"GameDatas loaded from {filePath}");
                return wrapper;
            }
            else
            {
                Debug.LogWarning($"No GameDatas file found at {filePath}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load GameDatas from JSON: {ex.Message}");
            return null;
        }
    }

    public static void ApplyLoadedData(GameDatasWrapper wrapper)
    {
        if (wrapper == null) return;

        try
        {
            GameDatas.Gold = wrapper.gold;
            GameDatas.Gem = wrapper.gem;
            GameDatas.PowerStone = wrapper.powerStone;
            GameDatas.Tourament = wrapper.tourament;
            GameDatas.Badges = wrapper.badges;
            GameDatas.ArmorSphere = wrapper.armorSphere;
            GameDatas.PowerSphere = wrapper.powerSphere;
            GameDatas.EngineSphere = wrapper.engineSphere;
            GameDatas.CryogenicSphere = wrapper.cryogenicSphere;

            GameDatas.CountBuyXUpgrader = wrapper.countBuyXUpgrader;
            foreach (var kvp in wrapper.levelUpgraderInforTower)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUpgraderGroupTower)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.clusterUpgrader)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);

            GameDatas.LastQuestCheckDate = wrapper.lastQuestCheckDate;
            GameDatas.countSpinCard = wrapper.countSpinCard;
            GameDatas.levelCardUnlock = wrapper.levelCardUnlock;
            GameDatas.IndexSlotCardUnlockByGold = wrapper.indexSlotCardUnlockByGold;
            GameDatas.IndexSlotCard = wrapper.indexSlotCard;
            foreach (var kvp in wrapper.unlockSlotCard)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.unlockCard)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.levelCard)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.amountCard)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.cardEquipped)
                SavePrefs.SetInt(kvp.Key, kvp.Value);

            GameDatas.CountSlotLabUnlock = wrapper.countSlotLabUnlock;
            foreach (var kvp in wrapper.levelSubjectLab)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.timeTargetSubject)
                GameDatas.SaveObject(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.slotSave)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.clusterUnlockLabInfor)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);

            GameDatas.IsFirstDoneWave40 = wrapper.isFirstDoneWave40;
            GameDatas.StartTimeFirstInGame = wrapper.startTimeFirstInGame;
            GameDatas.IsTut_Play = wrapper.isTutPlay;
            GameDatas.isTutLab = wrapper.isTutLab;
            GameDatas.isTutSpeed = wrapper.isTutSpeed;
            GameDatas.isTutBuildPhase2 = wrapper.isTutBuildPhase2;
            GameDatas.IsFirstTimeGoHome = wrapper.isFirstTimeGoHome;
            GameDatas.IsFirstClaimOfflineReward = wrapper.isFirstClaimOfflineReward;
            GameDatas.IsResetData = wrapper.isResetData;
            GameDatas.IsTut_0_Upgrade = wrapper.isTut0Upgrade;
            GameDatas.IsTut_PlayDemo = wrapper.isTutPlayDemo;
            GameDatas.IsEndTutorial = wrapper.isEndTutorial;
            GameDatas.IsFirstInGame = wrapper.isFirstInGame;
            GameDatas.IsActiveTutorial = wrapper.isActiveTutorial;
            GameDatas.SetMoreTurnPurchaseCount(wrapper.moreTurnPurchaseCount);
            GameDatas.SetClaimedX2OfflineReward(wrapper.claimedX2OfflineReward);

            GameDatas.CurrentWorld = wrapper.currentWorld;
            foreach (var kvp in wrapper.highestWaveInWorld)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.SetHighestWorld(wrapper.highestWorld);

            GameDatas.timeGameMax = wrapper.timeGameMax;
            GameDatas.TimeGamePlaySpeed = wrapper.timeGamePlaySpeed;

            GameDatas.ReviveTowerCount = wrapper.reviveTowerCount;
            GameDatas.timeTargetFullRevive = wrapper.timeTargetFullRevive;

            GameDatas.isX2 = wrapper.isX2;
            GameDatas.isX3 = wrapper.isX3;
            GameDatas.isX4 = wrapper.isX4;
            GameDatas.timeClaimFreeGem_Target = wrapper.timeClaimFreeGemTarget;
            GameDatas.timeClaimFreeGold_Target = wrapper.timeClaimFreeGoldTarget;

            GameDatas.timeTargetBonusGold = wrapper.timeTargetBonusGold;
            GameDatas.XBonusGoldDailyQuest = wrapper.xBonusGoldDailyQuest;
            GameDatas.TimeBonusGold = wrapper.timeBonusGold;

            foreach (var kvp in wrapper.unlockUltimateWeapon)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.levelUltimateQuantity)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateDmgBonus)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateDurationBonus)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateSizeBonus)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateSlowBonus)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateCooldown)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateChangeCost)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateDmgScale)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelUltimateAngle)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.UnlockSLotUltimateWeapon(wrapper.unlockSlotUltimateWeapon);

            GameDatas.RemoveAdsForever = wrapper.removeAdsForever;

            GameDatas.secondToGetReward = wrapper.secondToGetReward;
            GameDatas.SecondsAccumulate = wrapper.secondsAccumulate;
            GameDatas.LastLogoutTime = wrapper.lastLogoutTime;

            GameDatas.IsUsingPremiumMileStones = wrapper.isUsingPremiumMileStones;
            if (wrapper.allItemMileStoneData != null)
            {
                string filePath = Path.Combine(Application.persistentDataPath, GameConstants.Key_filePathMileStoneUnlock);
                string jsonData = JsonUtility.ToJson(wrapper.allItemMileStoneData, true);
                File.WriteAllText(filePath, jsonData);
            }

            GameDatas.isUnlockRanking = wrapper.isUnlockRanking;
            GameDatas.isUnlockAchievement = wrapper.isUnlockAchievement;
            GameDatas.isUnlockLuckyDraw = wrapper.isUnlockLuckyDraw;
            GameDatas.isUnlockDailyGift = wrapper.isUnlockDailyGift;
            GameDatas.isUnlockLab = wrapper.isUnlockLab;
            GameDatas.IsUnlockFeatureCard = wrapper.isUnlockFeatureCard;
            GameDatas.IsUnlockFeatureArena = wrapper.isUnlockFeatureArena;

            GameDatas.IsSpinFree = wrapper.isSpinFree;
            GameDatas.CountSpinFree = wrapper.countSpinFree;
            SavePrefs.SetString(GameKeys.Key_lastSpinTime, wrapper.lastSpinTime.ToString());

            foreach (var kvp in wrapper.dayDailyReceived)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            GameDatas.LastLoginDateDailyReward(wrapper.lastLoginDateDailyReward);
            GameDatas.CurrentDayDailyReward(wrapper.currentDayDailyReward);
            GameDatas.SetAccumulateDailyReward(wrapper.accumulateDailyReward);
            foreach (var kvp in wrapper.claimedAccumulateDailyReward)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);

            GameDatas.LastLoginDateDailyQuest(wrapper.lastLoginDateDailyQuest);
            foreach (var kvp in wrapper.claimedQuestReward)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.claimedAccumulateDailyGift)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.totalQuestDone)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.SetAccumulateDailyGift(wrapper.accumulateDailyGift);
            GameDatas.IsChangeQuestReward = wrapper.isChangeQuestReward;
            if (wrapper.allDailyQuestData != null)
            {
                string jsonData = JsonUtility.ToJson(wrapper.allDailyQuestData);
                string encodedData = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonData));
                PlayerPrefs.SetString(GameKeys.QuestKey, encodedData);
                PlayerPrefs.Save();
            }
           
            foreach (var kvp in wrapper.waveInResume)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.SetWorldInResume(wrapper.worldInResume);
            foreach (var kvp in wrapper.resumeWave)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            GameDatas.ActiveWelcomeBack(wrapper.isActiveWelcomeBack);

            foreach (var kvp in wrapper.unlockAvatar)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            GameDatas.user_name = wrapper.userName;
            GameDatas.id_avatar = wrapper.idAvatar;
            GameDatas.SaveObject(GameKeys.Key_StartDate, wrapper.startDate);
            foreach (var kvp in wrapper.dataProfile)
                SavePrefs.SetFloat(kvp.Key, kvp.Value);
            GameDatas.countUpgraderTime = wrapper.countUpgraderTime;
            GameDatas.countDoneQuest = wrapper.countDoneQuest;
            GameDatas.countLoseGame = wrapper.countLoseGame;
            GameDatas.countEnemyDestroy = wrapper.countEnemyDestroy;

            GameDatas.IsFirstPlayArena = wrapper.isFirstPlayArena;
            GameDatas.TotalArenaRankPlay = wrapper.totalArenaRankPlay;
            GameDatas.TotalArenaRankActive = wrapper.totalArenaRankActive;
            GameDatas.CurrentRank = wrapper.currentRank;
            GameDatas.TotalReplayArena = wrapper.totalReplayArena;
            GameDatas.SetHighestRank(wrapper.highestRank);
            foreach (var kvp in wrapper.indexRank)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.highestWaveInRank)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.ResetTouramentArena(wrapper.isResetTournamentArena);
            GameDatas.ClaimRewardArenaRank(wrapper.isClaimRewardArenaRank);
            if (wrapper.allHistoryArenaData != null)
            {
                string jsonData = JsonUtility.ToJson(wrapper.allHistoryArenaData);
                string encodedData = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonData));
                PlayerPrefs.SetString(GameKeys.HistoryArenaKey, encodedData);
                PlayerPrefs.Save();
            }

            foreach (var kvp in wrapper.levelShockChangeBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelDamageBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelBonusBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelDamageReductionBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelCoolDownBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelRangeBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.levelDurationBot)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.botChallengeUnlock)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.themeSongUnlock)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            foreach (var kvp in wrapper.relicUnlock)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            GameDatas.X2BadgesEvent(wrapper.isX2BadgesEvent);
            GameDatas.RandomRelicReward(wrapper.isRandomRelicReward);
            GameDatas.LastCheckNewDayChallenge = wrapper.lastCheckNewDayChallenge;
            GameDatas.LastCheckTimeChallenge = wrapper.lastCheckTimeChallenge;
            GameDatas.TotalMissionChallenge = wrapper.totalMissionChallenge;
            GameDatas.FirstStartEventChallenge(wrapper.isFirstStartEventChallenge);
            foreach (var kvp in wrapper.indexRankMissionChallenge)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            foreach (var kvp in wrapper.progressMissionChallenge)
                SavePrefs.SetInt(kvp.Key, kvp.Value);

            GameDatas.ActiveBannerPack(wrapper.isActiveBannerPack);
            GameDatas.FirstOpenDate = wrapper.firstOpenDate;
            GameDatas.LastBannerCycle = wrapper.lastBannerCycle;
            foreach (var kvp in wrapper.buyBeginnerPack)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);

            foreach (var kvp in wrapper.claimBeginnerQuest)
                SavePrefs.SetInt(kvp.Key, kvp.Value ? 1 : 0);
            GameDatas.timeTargetBeginnerQuest = wrapper.timeTargetBeginnerQuest;
            GameDatas.timeStart = wrapper.timeStart;
            GameDatas.CountDaysLogin = wrapper.countDaysLogin;
            GameDatas.timeStart_30minRemain = wrapper.timeStart30minRemain;
            GameDatas.ActiveBeginnerQuests = wrapper.activeBeginnerQuests;
            GameDatas.FirstInGameActiveBeginnerQuest = wrapper.firstInGameActiveBeginnerQuest;
            foreach (var kvp in wrapper.beginnerQuestProgress)
                SavePrefs.SetFloat(kvp.Key, kvp.Value);

            foreach (var kvp in wrapper.relicEquipped)
                SavePrefs.SetInt(kvp.Key, kvp.Value);
            GameDatas.IndexSlotRelic = wrapper.indexSlotRelic;

            GameDatas.ThemeSongUsing((TypeSong)wrapper.themeSongUsing);

            Debug.Log("Loaded data applied to GameDatas");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to apply loaded data to GameDatas: {ex.Message}");
        }
    }
}

#endregion*/
