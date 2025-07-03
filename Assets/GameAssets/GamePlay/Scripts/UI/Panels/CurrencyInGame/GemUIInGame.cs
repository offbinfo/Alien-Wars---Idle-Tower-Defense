using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemUIInGame : BaseCurrencyUI
{
    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnGemChanged_ingame, OnRefresh);
    }

    protected override void OnRefresh(object o)
    {
        txtCurrency.text = GPm.GemInGame.ToString();
    }

    
}
