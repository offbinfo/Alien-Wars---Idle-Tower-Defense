using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UW_UpgradeCellData : BaseUICellData
{
    public SO_UW_Base data;

    public SO_UW_Base Data => data; 
    
    public UW_UpgradeCellData(SO_UW_Base data)
    {
        this.data = data;   
    }
}
