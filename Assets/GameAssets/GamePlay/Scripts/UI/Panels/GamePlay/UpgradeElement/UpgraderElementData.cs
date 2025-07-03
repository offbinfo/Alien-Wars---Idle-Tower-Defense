using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderElementData : BaseUICellData
{
    private SO_UpgradeInforData data;

    public SO_UpgradeInforData DataUpgrade => data;

    public UpgraderElementData(SO_UpgradeInforData data)
    {
        this.data = data;
    }
}
