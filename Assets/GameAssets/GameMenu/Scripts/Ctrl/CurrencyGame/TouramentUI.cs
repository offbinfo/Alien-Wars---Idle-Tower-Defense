using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouramentUI : BaseCurrencyUI
{

    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnTouramentChanged, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GameDatas.Tourament);
    }
}
