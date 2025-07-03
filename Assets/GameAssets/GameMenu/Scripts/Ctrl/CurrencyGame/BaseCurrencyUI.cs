using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseCurrencyUI : GameMonoBehaviour
{
    [SerializeField]
    protected TMP_Text txtCurrency;

    private void OnEnable()
    {
        OnRefresh(null);
    }

    protected abstract void OnRefresh(object o);
    
}
