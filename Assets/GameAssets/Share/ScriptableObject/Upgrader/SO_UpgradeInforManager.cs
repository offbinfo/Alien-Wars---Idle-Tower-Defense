using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeInforManager", menuName = "Data/Upgrade/UpgradeInforManager", order = 0)]
public class SO_UpgradeInforManager : SerializedScriptableObject
{
    public List<SO_UpgradeInforData> upgradeInforDatas = new();
    public Dictionary<UpgraderID, SO_UpgradeInforData> upgradeInforDicts = new();

/*    public SO_UpgradeInforData GetUpgradeInforByKey(UpgraderID upgraderID)
    {
        foreach (var upgrade in upgradeInforDatas)
        {
            if(upgrade.upgraderID == upgraderID) return upgrade;
        }
        return null;
    }*/

    public SO_UpgradeInforData GetUpgradeInforByKey(UpgraderID upgraderID)
    {
        if(upgradeInforDicts.ContainsKey(upgraderID))
        {
            return upgradeInforDicts[upgraderID];
        }
        return null;
    }

#if UNITY_EDITOR

    [Button("ConvertListToDictionary")]
    private void ConvertListToDictionary()
    {
        upgradeInforDicts.Clear();
        foreach (var data in upgradeInforDatas)
        {
            if (data != null)
            {
                upgradeInforDicts[data.upgraderID] = data;
            }
        }
    }

#endif
}
