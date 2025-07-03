using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UWoptionElementCellData : BaseUICellData
{
    private SO_UW_Base SO_UW_Base;

    public SO_UW_Base Data => SO_UW_Base;

    public UWoptionElementCellData(SO_UW_Base sO_UW_Base)
    {
        SO_UW_Base = sO_UW_Base;
    }
}
