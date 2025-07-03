using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInforCtrl : GameMonoBehaviour
{
    [SerializeField]
    private SO_UpgradeTowerManager upgradeTowerManager;
    [SerializeField]
    private SO_UpgradeInforManager upgradeInforManager;
    private Dictionary<UpgraderID, Int> dicUpgraderID = new();

    public SO_UpgradeTowerManager UpgradeManager => upgradeTowerManager;
    public SO_UpgradeInforManager UpgradeInforManager => upgradeInforManager;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnResetData, OnResetData);
    }

    private void OnResetData(object o)
    {
        //Dtm.UserGameData.ResetUpgraderTowerDefault();
    }

    public void ResetDictInGame()
    {
        dicUpgraderID.Clear();
    }

    #region Upgrade Tower
    public float GetStat(UpgraderID ID, int level)
    {
        return GetData(ID).GetProperty(level);
    }

    public float GetStatCurrent(UpgraderID ID)
    {
        var data = GetData(ID);
        if (!GameDatas.IsUnlockClusterUpgrader(data.upgraderCategory)) return 0;

        var leveBase = GameDatas.GetLevelUpgraderInforTower(ID);
        var level = GetLevelUpgraderIngame(ID);
        var stat = GetStat(ID, leveBase + level);
        return stat;
    }
    public float GetStatBase(UpgraderID ID)
    {
        var levelBase = GameDatas.GetLevelUpgraderInforTower(ID);
        var statBase = GetStat(ID, levelBase);
        return statBase;
    }

    public int GetLevelUpgraderIngame(UpgraderID ID)
    {
        if (!dicUpgraderID.ContainsKey(ID))
            dicUpgraderID.Add(ID, 0);
        return dicUpgraderID[ID];
    }
    public void SetLevelUpgraderIngame(UpgraderID ID, int level)
    {
        if (!dicUpgraderID.ContainsKey(ID))
            dicUpgraderID.Add(ID, level);
        else
            dicUpgraderID[ID] = level;
        EventDispatcher.PostEvent(EventID.UpgraderTowerInGame, ID);
    }

    public SO_UpgradeInforData GetData(UpgraderID upgraderID)
    {
        return upgradeInforManager.GetUpgradeInforByKey(upgraderID);
    }

    public List<SO_UpgradeInforData> GetListData(UpgraderGroupID groupID)
    {
        return upgradeTowerManager.GetUpgradeDataByKey(groupID).GetAllUpgradeInforTower();
    }
    #endregion

    #region Get price level max ,set price unlock 
    public float GetPriceUnlockGroup(UpgraderGroupID groupId, UpgraderCategory upgraderCategory)
    {
        return upgradeTowerManager.
            GetUpgradeDataByKey(groupId).GetPriceClusterUpgrade(upgraderCategory);
    }

    public List<SO_UpgradeInforData> GetLevelUpgradeByGroup(UpgraderGroupID groupId, UpgraderCategory upgraderCategory)
    {
        return upgradeTowerManager.GetUpgradeDataByKey(groupId).GetDataUpgradeByUpgraderCategory(upgraderCategory);
    }

    public int GetLevelMaxGroup(UpgraderGroupID groupID)
    {
        return upgradeTowerManager.
            GetUpgradeDataByKey(groupID).upgradeDatas.Values.Count;
    }

    #endregion

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnResetData, OnResetData);
    }
}
