using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldUI : BaseCurrencyUI
{
    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnGoldChanged, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GameDatas.Gold);
    }
}
