using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class PropertyUIBot : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TMP_Text textInfor;
    [SerializeField]
    private TMP_Text txt_price;
    public float priceUpgrader;
    [SerializeField]
    private GameObject btnUnBuy;
    private int index = -1;
    private SO_Bot_Base data;
    private TypeBot id;
    private int price;
    private bool isDuration;

    private void Start()
    {
        
    }

    public void SetData(SO_Bot_Base data, int index)
    {
        this.data = data;
        this.index = index;
        id = data.typeBot;
        Refresh();
    }

    private void Refresh()
    {
        if (data == null)
            return;
        txtName.text = data.statName[index];

        float property = float.Parse(data.GetStatProperty()[index].ToString());

        textInfor.text = data.GetFormat[index] == "s"
            ? property.ToString() + data.GetFormat[index]
            : data.GetFormat[index] + property.ToString();
        isDuration = data.GetFormat[index] == "s";

        if(data.GetFormat[index] == "%")
        {
            textInfor.text = property.ToString() + data.GetFormat[index];
        }

        price = data.GetPrice()[index];
        txt_price.text = price.ToString();

        if (data.Level[index] >= data.LevelMax[index])
            txt_price.text = "Level Max";

        CheckUpgrade();
    }

    private void CheckUpgrade()
    {
        if (data == null)
            return;
        price = data.GetPrice()[index];
        btnUnBuy.SetActive(price > GameDatas.Badges);
    }

    public void BtnUpgrader()
    {
        if (data.Level[index] >= data.LevelMax[index])
            return;
        GameDatas.BuyUsingCurrency(CurrencyType.BADGES, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool iSuccess)
    {
        if (iSuccess)
        {
            data.GetActionUpgrade()[index]?.Invoke();
            Refresh();
        }
    }

}
