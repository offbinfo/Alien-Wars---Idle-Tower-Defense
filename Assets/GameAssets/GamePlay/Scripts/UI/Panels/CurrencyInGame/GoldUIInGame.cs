using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldUIInGame : BaseCurrencyUI
{
    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnGoldChanged_ingame, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GPm.GoldInGame);
    }

}
