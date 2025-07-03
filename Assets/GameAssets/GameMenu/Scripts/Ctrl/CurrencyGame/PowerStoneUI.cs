using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStoneUI : BaseCurrencyUI
{
    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnPowerStoneChanged, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GameDatas.PowerStone);
    }
}
