using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Data/Upgrade/UpgradeData", order = 0)]
public class SO_UpgradeData : SerializedScriptableObject
{
   
    public Dictionary<UpgraderCategory, ClusterUpgradeInfor> upgradeDatas;

    public List<SO_UpgradeInforData> GetDataUpgradeByUpgraderCategory(UpgraderCategory upgraderCategory)
    {
        if (upgradeDatas.ContainsKey(upgraderCategory))
        {
            return upgradeDatas[upgraderCategory].upgradeInforDatas;
        }
        return null;
    }

    public ClusterUpgradeInfor GetClusterUpgradeInforByCategory(UpgraderCategory upgraderCategory)
    {
        if (upgradeDatas.ContainsKey(upgraderCategory))
        {
            return upgradeDatas[upgraderCategory];
        }
        return null;
    }

    public ClusterUpgradeInfor GetSingleUpgradeByLevelUnlock(int levelUnlock)
    {
        return upgradeDatas.Values.FirstOrDefault(upgrade => upgrade.levelUnlock == levelUnlock);
    }

    public float GetPriceClusterUpgrade(UpgraderCategory upgraderCategory)
    {
        if (upgradeDatas.ContainsKey(upgraderCategory))
        {
            return upgradeDatas[upgraderCategory].price;
        }
        return 0f;
    }

    public List<SO_UpgradeInforData> GetAllUpgradeInforTower()
    {
       return upgradeDatas.Values
            .SelectMany(cluster => cluster.upgradeInforDatas)
            .ToList();
    }

#if UNITY_EDITOR

    [Button("Async TypeUpgraderCategory " +
        "In ClusterUpgradeInfor")]
    public void AsyncUpgraderCategoryClusterUpgradeInfor()
    {
        if (upgradeDatas == null) return;

        foreach (var kvp in upgradeDatas)
        {
            UpgraderCategory category = kvp.Key;
            kvp.Value.upgraderCategory = category;
        }

        DebugCustom.Log("Đồng bộ UpgraderCategory vào ClusterUpgradeInfor hoàn tất!");
    }

    [Button("AsyncType UpgraderCategory " +
        "In SO_UpgradeInforData")]
    public void AsyncUpgraderCategory()
    {
        if (upgradeDatas == null) return;

        foreach (var kvp in upgradeDatas)
        {
            UpgraderCategory category = kvp.Key; 
            List<SO_UpgradeInforData> upgradeList = kvp.Value.upgradeInforDatas;

            foreach (var data in upgradeList)
            {
                if (data != null)
                {
                    data.upgraderCategory = category; 
                }
            }
        }

        DebugCustom.Log("Đồng bộ UpgraderCategory vào SO_UpgradeInforData hoàn tất!");
    }
#endif
}

public class ClusterUpgradeInfor
{
    public float price;
    public BuyingType buyingType;
    public UpgraderCategory upgraderCategory;
    public int levelUnlock = 0;
    [Space]
    [Title("================ Upgrade Data =================")]
    public List<SO_UpgradeInforData> upgradeInforDatas = new();
}
