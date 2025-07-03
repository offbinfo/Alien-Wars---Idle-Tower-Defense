using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeUI : BaseCurrencyUI
{
    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnBadgesChanged, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GameDatas.Badges);
    }
}
