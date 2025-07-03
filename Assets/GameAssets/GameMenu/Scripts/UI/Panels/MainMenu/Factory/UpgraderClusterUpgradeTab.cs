using language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UpgraderClusterUpgradeTab : GameMonoBehaviour
{

    [SerializeField]
    private UpgraderGroupID upgraderGroupID;
    [SerializeField]
    private UpgraderCategory upgraderCategory;
    [SerializeField]
    private TMP_Text txtContent;
    [SerializeField]
    private TMP_Text txtDesc;
    [SerializeField]
    private TMP_Text txtPrice;
    [SerializeField]
    private Button btn_buy;
    [SerializeField]
    private Image img_btnBuy;
    [SerializeField]
    private TabUpgradeFactory upgradeFactory;
    [SerializeField]
    private Material grayScaleMaterial;
    private SO_UpgradeData _UpgradeData;

    [SerializeField]
    private GameObject btnUnBuy;
    private int price;

    private void Start()
    {
        //EventDispatcher.AddEvent(EventID.OnRefeshUIUpgrade, OnRefreshClusterUpgrade);
        EventDispatcher.AddEvent(EventID.OnGoldChanged, OnRefreshUI);
    }

    private void OnEnable()
    {
        if (this._UpgradeData == null) return;
            InitInforUpgradeGroup();
    }

    private void OnRefreshUI(object obj)
    {
        btnUnBuy.SetActive(price > GameDatas.Gold);
    }

    public void OnRefreshClusterUpgrade(/*object o*/)
    {
        if (GameDatas.IsUnlockClusterUpgrader(upgraderCategory))
        {
            GameDatas.countUpgraderTime += 1;

            int indexLevelGroup = GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID) + 1;
            GameDatas.SetLevelUpgraderGroupTower(upgraderGroupID, indexLevelGroup);
            GameDatas.UnlockClusterUpgrader(upgraderCategory);
            EventDispatcher.PostEvent(EventID.OnBuyClusterUpgrader, upgraderCategory);
            InitInforUpgradeGroup();
            OnRefreshUI(null);
        }
    }

    public void SetData(SO_UpgradeData SO_UpgradeData, UpgraderGroupID upgraderGroupID)
    {
        this._UpgradeData = SO_UpgradeData;
        this.upgraderGroupID = upgraderGroupID;
        InitInforUpgradeGroup();
    }

    public bool IsUpgradeUnlocked(int level)
    {
        return GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID) >= level;
    }

    private bool CheckUnlockClusterUpgrade()
    {
        ClusterUpgradeInfor clusterUpgradeInfor = 
            _UpgradeData.GetSingleUpgradeByLevelUnlock(GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID));

        if(clusterUpgradeInfor == null) return false;
        return !clusterUpgradeInfor.upgradeInforDatas
            .Any(upgrade => GameDatas.GetLevelUpgraderInforTower(upgrade.upgraderID) == 1);
    }

    private void UpgradeCluster()
    {
        int index = GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID) + 1;
        GameDatas.SetLevelUpgraderGroupTower(upgraderGroupID, index);
        GameDatas.UnlockClusterUpgrader(upgraderCategory);
    }

    private void InitInforUpgradeGroup()
    {
        if (CheckUnlockClusterUpgrade())
        {
            UpgradeCluster();
        }

        int indexLevelGroup = GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID);

        ClusterUpgradeInfor clusterUpgradeInfor = _UpgradeData.GetSingleUpgradeByLevelUnlock(indexLevelGroup);

        if (clusterUpgradeInfor != null)
        {
            upgraderCategory = clusterUpgradeInfor.upgraderCategory;
            txtContent.text = LanguageManager.GetText(upgraderCategory.ToString());
            txtDesc.text = LanguageManager.GetText($"desc_{upgraderCategory}");
            txtPrice.text = Extensions.FormatNumber((int)clusterUpgradeInfor.price);
            price = (int)clusterUpgradeInfor.price;

            InActiveContent(true);
        }
        else
        {
            InActiveContent(false);
        }
        btnUnBuy.SetActive(price > GameDatas.Gold);

        if (GameDatas.IsUnlockClusterUpgrader(upgraderCategory))
        {
            GameDatas.countUpgraderTime += 1;

            UpgradeCluster();

            int levelgroup = GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID);

            ClusterUpgradeInfor cluster = _UpgradeData.GetSingleUpgradeByLevelUnlock(levelgroup);
            if (cluster != null)
            {
                upgraderCategory = cluster.upgraderCategory;
                txtContent.text = LanguageManager.GetText($"name_{upgraderCategory}");
                txtDesc.text = LanguageManager.GetText($"describe_{upgraderCategory}");
                txtPrice.text = Extensions.FormatNumber((int)cluster.price);
                price = (int)cluster.price;

                InActiveContent(true);
            }
            else
            {
                InActiveContent(false);
            }
            btnUnBuy.SetActive(price > GameDatas.Gold);
        }
       
    }

    private void InActiveContent(bool isActive)
    {
        gameObject.SetActive(isActive);
        upgradeFactory.ExtentGrid(isActive);
    }

    public void OnClickBuy()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GOLD, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool isSuccess)
    {
        if (!isSuccess) return;
        GameDatas.countUpgraderTime += 1;

        int indexLevelGroup = GameDatas.GetLevelUpgraderGroupTower(upgraderGroupID) + 1;
        GameDatas.SetLevelUpgraderGroupTower(upgraderGroupID, indexLevelGroup);
        GameDatas.UnlockClusterUpgrader(upgraderCategory);
        EventDispatcher.PostEvent(EventID.OnBuyClusterUpgrader, upgraderCategory);
        InitInforUpgradeGroup();
        OnRefreshUI(null);
        InitInforUpgradeGroup();
        EventDispatcher.PostEvent(EventID.OnBeginnerQuestProgressChanged, 0);
    }
}
