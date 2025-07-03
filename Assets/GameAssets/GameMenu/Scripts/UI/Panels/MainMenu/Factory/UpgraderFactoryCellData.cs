using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderFactoryCellData : BaseUICellData
{
    private SO_UpgradeInforData sO_UpgradeInforData;

    public SO_UpgradeInforData DataUpgrade => sO_UpgradeInforData;

    public UpgraderFactoryCellData(SO_UpgradeInforData upgradeInforData)
    {
        this.sO_UpgradeInforData = upgradeInforData;
    }
}
