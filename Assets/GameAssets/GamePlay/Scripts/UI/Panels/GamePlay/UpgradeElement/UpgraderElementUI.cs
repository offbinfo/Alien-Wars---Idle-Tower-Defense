using DG.Tweening;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UpgraderElementUI : BaseUICellView
{
    [SerializeField]
    private TMP_Text id_Text;
    [SerializeField]
    private TMP_Text currentInfoText;
    [SerializeField]
    private TMP_Text nextInfoText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private GameObject txtMaxUpgrade;
    [SerializeField]
    private GameObject disableBuy;
    [SerializeField] 
    private GameObject btnUpgrader;

    private UpgraderID upgraderID;
    private UpgraderCategory upgraderCategory;
    private UpgraderGroupID groupID;

    private UpgraderElementData upgradeCellData;
    private TowerData towerData;

    private int currentPrice;

    public GameObject obj_btnUpgrader_ref => btnUpgrader;

    string format
    {
        get
        {
            switch (upgradeCellData.DataUpgrade.format)
            {
                case Format.NUMBER:
                    return "";
                case Format.PERCENT:
                    return "%";
            }
            return "";
        }
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnSilverChanged_ingame, OnSilverChanged_ingame);
        EventDispatcher.AddEvent(EventID.OnEquipCard, OnRefreshData);
        EventDispatcher.AddEvent(EventID.OnUnequipcard, OnRefreshData);
        EventDispatcher.AddEvent(EventID.ShockwaveBonusHealth, OnRefreshData);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, OnRefreshData);
    }

    private void OnRefreshData(object obj)
    {
        if (upgradeCellData != null) Refresh(upgradeCellData);
    }

    private void OnSilverChanged_ingame(object obj)
    {
        disableBuy.SetActive(currentPrice > GPm.SliverInGame);
    }

    public void ShowInfor()
    {
        Board_UIs.instance.OpenBoard(UiPanelType.PopupUpgraderInfo);
        PopupUpgraderInfo.Instance.SetUp(upgradeCellData.DataUpgrade);
    }

    public void OnChangePriceByCountBuyX()
    {
        Refresh(upgradeCellData);
        OnSilverChanged_ingame(null);
    }

    private void OnEnable()
    {
        OnRefreshData(null);
        ScaleUp();
    }

    [SerializeField] private float duration = 0.25f;
    private Tween scaleTween;
    private static readonly Vector3 hiddenScale = Vector3.zero;
    private static readonly Vector3 shownScale = Vector3.one;

    private void ScaleUp()
    {
        if(GameDatas.IsFirstTimeGoHome) return;
        transform.localScale = hiddenScale;
        scaleTween?.Kill();
        scaleTween = transform.DOScale(shownScale, duration)
                              .SetEase(Ease.OutBack)
                              .SetUpdate(true);
    }

    private void ScaleDown()
    {
        if (GameDatas.IsFirstTimeGoHome) return;
        scaleTween?.Kill();
        transform.DOScale(hiddenScale, duration)
                 .SetEase(Ease.InBack)
                 .SetUpdate(true);
    }

    private void OnDisable()
    {
        ScaleDown();
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);
        UpgraderElementData cellData = data as UpgraderElementData;
        upgraderID = cellData.DataUpgrade.upgraderID;
        groupID = cellData.DataUpgrade.upgraderGroupID;
        upgraderCategory = cellData.DataUpgrade.upgraderCategory;
        upgradeCellData = cellData;

        Refresh(upgradeCellData);
    }

    private void CheckUnlockItemUpgrade()
    {
        if (upgraderCategory == UpgraderCategory.WORKSHOP_BASE)
        {
            //base data
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(GameDatas.IsUnlockClusterUpgrader(upgraderCategory));
        }
    }

    public void Refresh(UpgraderElementData cellData)
    {
        var data = cellData.DataUpgrade;
        if (data == null) return;

        towerData ??= GPm.Tower.TowerData;

        id_Text.text = LanguageManager.GetText(upgraderID.ToString());

        //int levelInGame = Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID);

        int maxLevel = data.maxLevel;
        int levelInGame = Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID);
        int sumLevel = levelInGame + GameDatas.GetLevelUpgraderInforTower(upgraderID);
        int curSumLevel = levelInGame + GameDatas.GetLevelUpgraderInforTower(upgraderID);
        int indexBuyX = 1;
        int nextLevel = 1;

        float multiply = 1 + Cfg.cardCtrl.GetCardStatBonus(upgraderID) / 100f;
        float bonus = upgraderID == UpgraderID.gold_bonus_each_wave
            ? Cfg.cardCtrl.GetCurrentStat(CardID.RARE_GOLD_RWD)
            : 0f;
/*        float bonusHPShockwave = upgraderID == UpgraderID.health
            ? towerData.bonusHpShockWave
            : 0f;*/

        if (sumLevel < maxLevel)
        {
            indexBuyX = GameDatas.CountBuyXUpgrader;
            levelInGame = Mathf.Min(Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID) +
                (indexBuyX == 1 ? 0 : indexBuyX), maxLevel);

            levelInGame -= levelInGame == maxLevel ? 1 : 0;
            sumLevel = Mathf.Min(levelInGame + GameDatas.GetLevelUpgraderInforTower(upgraderID), maxLevel - 1);
            nextLevel = /*indexBuyX == 1 ? */sumLevel + 1/* : sumLevel*/;
        }
        else
        {
            sumLevel = maxLevel;
            curSumLevel = maxLevel;
            nextLevel = maxLevel;
        }

        bool isHealth = upgraderID == UpgraderID.health;
        float extra = isHealth ? towerData.HPPerEnemyKillByShockWave : 0f;

        float currentInfo = (data.GetProperty(curSumLevel) * multiply) + bonus + extra;
        float nextInfo = (data.GetProperty(nextLevel) * multiply) + bonus + extra;

        float currentInfoNew = currentInfo;
        float nextInfoNew = nextInfo;
        if (upgraderID == UpgraderID.coin_per_kill || upgraderID == UpgraderID.coin_per_wave)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, nextInfo);
        }

        if (upgraderID == UpgraderID.attack_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, nextInfo);
        }

        if (upgraderID == UpgraderID.health)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, nextInfo);
        }

        if (upgraderID == UpgraderID.dodge_chance)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, nextInfo);
        }

        if (upgraderID == UpgraderID.damage_return_power)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, nextInfo);
        }

        if (upgraderID == UpgraderID.critical_shot_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, nextInfo);
        }

        currentInfoText.text = Extensions.FormatCompactNumber(currentInfoNew).ToString() +" "+ format;
        nextInfoText.text = $"<sprite name=mui ten ngang> "+ Extensions.FormatCompactNumber(nextInfoNew).ToString() + " " + format;

        float discountUpgrade = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.discount_of_coin_price);
        float discountCard = Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_REDUCE_PRICE);
        float discountMultiplier = 1 - (discountUpgrade + discountCard) / 100f;

        currentPrice = (int)(data.GetPriceItemUpgradeInGame(levelInGame + 1) * discountMultiplier);
        priceText.text = Extensions.FormatNumber(currentPrice);
        disableBuy.SetActive(currentPrice > GPm.SliverInGame);

        CheckUnlockItemUpgrade();

        bool isMaxLevel = data.maxLevel > 0 && sumLevel >= data.maxLevel;
        btnUpgrader.SetActive(!isMaxLevel);
        txtMaxUpgrade.SetActive(isMaxLevel);
        nextInfoText.gameObject.SetActive(!isMaxLevel);
    }

    private void OnRefreshUpgrader(UpgraderElementData cellData)
    {
        var data = cellData.DataUpgrade;
        int levelInGame = Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID);
        int sumLevel = Mathf.Min(levelInGame + GameDatas.GetLevelUpgraderInforTower(upgraderID), data.maxLevel);
        int nextLevel = sumLevel + 1;

        float multiply = 1 + Cfg.cardCtrl.GetCardStatBonus(upgraderID) / 100f;
        float bonus = (upgraderID == UpgraderID.gold_bonus_each_wave) ? Cfg.cardCtrl.GetCurrentStat(CardID.RARE_GOLD_RWD) : 0f;

        bool isHealth = upgraderID == UpgraderID.health;
        float extra = isHealth ? towerData.HPPerEnemyKillByShockWave : 0f;

        float currentInfo = (data.GetProperty(sumLevel) * multiply) + bonus + extra;
        float nextInfo = (data.GetProperty(nextLevel) * multiply) + bonus + extra;

        float currentInfoNew = currentInfo;
        float nextInfoNew = nextInfo;
        if (upgraderID == UpgraderID.coin_per_kill || upgraderID == UpgraderID.coin_per_wave)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, nextInfo);
        }

        if (upgraderID == UpgraderID.attack_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, nextInfo);
        }

        if (upgraderID == UpgraderID.health)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, nextInfo);
        }

        if (upgraderID == UpgraderID.dodge_chance)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, nextInfo);
        }

        if (upgraderID == UpgraderID.damage_return_power)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, nextInfo);
        }

        if (upgraderID == UpgraderID.critical_shot_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, currentInfo);
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, nextInfo);
        }

        currentInfoText.text = Extensions.FormatCompactNumber(currentInfo).ToString() + " " + format;
        nextInfoText.text = $"<sprite name=mui ten ngang> " + Extensions.FormatCompactNumber(nextInfo).ToString() + " " + format;

        float discountMultiplier = 1 - (Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.discount_of_coin_price) + Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_REDUCE_PRICE)) / 100f;

        currentPrice = (int)(data.GetPriceItemUpgradeInGame(levelInGame + 1) * discountMultiplier);
        priceText.text = Extensions.FormatNumber(currentPrice);
        disableBuy.SetActive(currentPrice > GPm.SliverInGame);

        bool isMaxLevel = data.maxLevel > 0 && sumLevel >= data.maxLevel;
        bool canUpgrade = !isMaxLevel;

        btnUpgrader.SetActive(canUpgrade);
        txtMaxUpgrade.SetActive(isMaxLevel);
        nextInfoText.gameObject.SetActive(canUpgrade);
    }

    public void OnclickBtnBuy()
    {
        if (GPm.SliverInGame < currentPrice) return;

        GameAnalytics.LogEvent_UpgradeByCoin(upgraderID);
        GPm.SliverInGame -= currentPrice;

        UpgradeItem();
    }

    public void UpgradeItem()
    {
        int maxLevel = upgradeCellData.DataUpgrade.maxLevel;
        int levelInGame = Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID);
        levelInGame = Mathf.Min(Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID) + GameDatas.CountBuyXUpgrader, maxLevel);

        //Cfg.upgraderCtrl.SetLevelUpgraderIngame(upgraderID, Cfg.upgraderCtrl.GetLevelUpgraderIngame(upgraderID) + 1);
        Cfg.upgraderCtrl.SetLevelUpgraderIngame(upgraderID, levelInGame);
        OnRefreshUpgrader(upgradeCellData);

        GPm.SpawnTextUpgrader("<sprite name=mui ten ngang> " + currentInfoText.text, id_Text.text);

        QuestEventManager.UpgradedInBattle(1);
        QuestEventManager.Upgrade(1);
        EventDispatcher.PostEvent(EventID.OnUpgradeInGame, upgraderID);
        EventDispatcher.PostEvent(EventID.OnScaleMapByRangedTower, upgraderID);
    }
}
