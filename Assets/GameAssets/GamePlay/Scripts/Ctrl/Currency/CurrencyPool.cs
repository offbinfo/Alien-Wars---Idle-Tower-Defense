using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyPool : Object_Pool
{
    public TMP_Text txt_amountCoin;
    private void Awake()
    {
        AddEventInit((obj) => {
            txt_amountCoin.text = obj.ToString();
        });
    }
}
