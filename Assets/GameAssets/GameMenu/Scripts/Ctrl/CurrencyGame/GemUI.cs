using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemUI : BaseCurrencyUI
{
    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnGemChanged, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GameDatas.Gem);
    }
}
