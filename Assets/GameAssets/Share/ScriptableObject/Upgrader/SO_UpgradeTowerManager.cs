using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeTowerManager", menuName = "Data/Upgrade/UpgradeTowerManager", order = 0)]
public class SO_UpgradeTowerManager : SerializedScriptableObject
{
    public Dictionary<UpgraderGroupID, SO_UpgradeData> upgraderManager = new();

    public SO_UpgradeData GetUpgradeDataByKey(UpgraderGroupID UpgraderGroupID)
    {
        if (upgraderManager.ContainsKey(UpgraderGroupID))
        {
            return upgraderManager[UpgraderGroupID];
        }
        return null;
    }
}
