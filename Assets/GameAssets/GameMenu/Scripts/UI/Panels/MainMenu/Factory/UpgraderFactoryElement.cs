using DG.Tweening;
using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgraderFactoryElement : BaseUICellView
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

    private UpgraderFactoryCellData upgradeCellData;
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

    private void OnEnable()
    {
        OnResferhData();
        ScaleUp();
    }

    [SerializeField] private float duration = 0.25f;
    private Tween scaleTween;
    private static readonly Vector3 hiddenScale = Vector3.zero;
    private static readonly Vector3 shownScale = Vector3.one;

    private void ScaleUp()
    {
        transform.localScale = hiddenScale;
        scaleTween?.Kill();
        scaleTween = transform.DOScale(shownScale, duration)
                              .SetEase(Ease.OutBack)
                              .SetUpdate(true);
    }

    private void ScaleDown()
    {
        scaleTween?.Kill();
        transform.DOScale(hiddenScale, duration)
                 .SetEase(Ease.InBack)
                 .SetUpdate(true);
    }

    private void OnDisable()
    {
        ScaleDown();
    }

    public void OnChangePriceByCountBuyX()
    {
        Refresh(upgradeCellData);
        OnSilverChanged();
    }

    public void OnSilverChanged()
    {
        disableBuy.SetActive(currentPrice > GameDatas.Gold);
    }

    public void ShowInfor()
    {
        Board_UIs.instance.OpenBoard(UiPanelType.PopupUpgraderInfo);
        PopupUpgraderInfo.Instance.SetUp(upgradeCellData.DataUpgrade);
    }

    public void OnResferhData()
    {
        if (upgradeCellData != null)
        {
            Refresh(upgradeCellData);
        }
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);
        UpgraderFactoryCellData cellData = data as UpgraderFactoryCellData;
        upgraderID = cellData.DataUpgrade.upgraderID;
        groupID = cellData.DataUpgrade.upgraderGroupID;
        upgraderCategory = cellData.DataUpgrade.upgraderCategory;
        upgradeCellData = cellData;

        Refresh(upgradeCellData);
    }

    public void CheckUnlockItemUpgrade()
    {
        if (GameDatas.GetLevelUpgraderInforTower(upgraderID) > 1)
        {
            gameObject.SetActive(true);
        }
        else
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
    }

    public void Refresh(UpgraderFactoryCellData cellData, bool isMax = false)
    {
        var data = cellData.DataUpgrade;
        if (data == null) return;

        int maxLevel = data.maxLevel;
        int indexLevelUpgrade = GameDatas.GetLevelUpgraderInforTower(upgraderID);
        int curLevelUpgrade = GameDatas.GetLevelUpgraderInforTower(upgraderID);
        int indexBuyX = 1;

        if (GameDatas.GetLevelUpgraderInforTower(upgraderID) != maxLevel)
        {
            indexBuyX = GameDatas.CountBuyXUpgrader;
            indexLevelUpgrade = Mathf.Min(GameDatas.GetLevelUpgraderInforTower(upgraderID) + 
                (indexBuyX == 1 ? 0 : indexBuyX), maxLevel);

            //indexLevelUpgrade -= indexLevelUpgrade == maxLevel ? 1 : 0;
            indexLevelUpgrade = indexLevelUpgrade >= maxLevel ? maxLevel - 1 : indexLevelUpgrade;
        }

        id_Text.text = LanguageManager.GetText(upgraderID.ToString());

        var bonus = 0f;
        if (upgraderID == UpgraderID.gold_bonus_each_wave)
            bonus = Cfg.cardCtrl.GetCurrentStat(CardID.RARE_GOLD_RWD);

        var multiply = 1 + Cfg.cardCtrl.GetCardStatBonus(upgraderID) / 100f;

        float currentInfo = (data.GetProperty(curLevelUpgrade) * multiply + bonus);

        float nextInfo = (data.GetProperty(indexLevelUpgrade + 1) 
            * multiply + bonus);

        float currentInfoNew = currentInfo;
        float nextInfoNew = nextInfo;
        if (upgraderID == UpgraderID.coin_per_kill || upgraderID == UpgraderID.coin_per_wave)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        if (upgraderID == UpgraderID.attack_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        if (upgraderID == UpgraderID.health)
        {
            DebugCustom.LogColor("djdj " + Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, (data.GetProperty(curLevelUpgrade) * multiply + bonus)));
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        if (upgraderID == UpgraderID.dodge_chance)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        if (upgraderID == UpgraderID.damage_return_power)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        if (upgraderID == UpgraderID.critical_shot_damage)
        {
            currentInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, (data.GetProperty(curLevelUpgrade) * multiply + bonus));
            nextInfoNew = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, (data.GetProperty(indexLevelUpgrade + 1) * multiply + bonus));
        }

        currentInfoText.text = Extensions.FormatCompactNumber(currentInfoNew).ToString() + format;
        nextInfoText.text = Extensions.FormatCompactNumber(nextInfoNew).ToString() + format;

        var discountUpgrade = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.discount_of_coin_price);
        var discountCard = Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_REDUCE_PRICE);
        var Discount = 1 - (discountUpgrade + discountCard) / 100f;
        currentPrice = (int)(data.GetPriceItemUpgradeBase(indexLevelUpgrade) * Discount);
        priceText.text = Extensions.FormatNumber(currentPrice);
        disableBuy.SetActive(currentPrice > GameDatas.Gold);

        //lock or unlock 
        CheckUnlockItemUpgrade();

        //limit level skill
        btnUpgrader.SetActive(!(data.maxLevel > 0 && indexLevelUpgrade >= data.maxLevel));
        txtMaxUpgrade.SetActive(data.maxLevel > 0 && indexLevelUpgrade >= data.maxLevel);
        nextInfoText.gameObject.SetActive(!(data.maxLevel > 0 && indexLevelUpgrade >= data.maxLevel));
    }

    public void OnclickBtnBuy()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GOLD, currentPrice, OnBuySuccess);
    }

    private void OnBuySuccess(bool isSuccess)
    {
        if (isSuccess)
        {
            GameAnalytics.LogEvent_UpgraderByGold(upgraderID);

            int maxLevel = upgradeCellData.DataUpgrade.maxLevel;
            int indexLevelUpgrade = GameDatas.GetLevelUpgraderInforTower(upgraderID);
            indexLevelUpgrade = Mathf.Min(GameDatas.GetLevelUpgraderInforTower(upgraderID) + GameDatas.CountBuyXUpgrader, maxLevel);

            GameDatas.SetLevelUpgraderInforTower(upgraderID, indexLevelUpgrade);
            EventDispatcher.PostEvent(EventID.UpgraderTowerBaseGame, null);
            Refresh(upgradeCellData);
            EventDispatcher.PostEvent(EventID.OnBeginnerQuestProgressChanged, 0);

            QuestEventManager.Upgrade(1);
            EventChallengeListenerManager.Upgrade(1);
        }
    }
}
