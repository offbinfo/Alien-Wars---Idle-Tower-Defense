using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliverUIInGame : BaseCurrencyUI
{
    private float defaultSliverInGame = 50f;

    private void OnEnable()
    {
        SetBaseSilver();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnSilverChanged_ingame, OnRefresh);
    }
    protected override void OnRefresh(object o)
    {
        txtCurrency.text = Extensions.FormatNumber(GPm.SliverInGame);
    }

    private void SetBaseSilver()
    {
        //default amount
        var bonusCoin = Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.STARTING_COIN).GetCurrentProperty();
        GPm.SliverInGame = defaultSliverInGame + (int)bonusCoin;
        OnRefresh(null);
    }
}
