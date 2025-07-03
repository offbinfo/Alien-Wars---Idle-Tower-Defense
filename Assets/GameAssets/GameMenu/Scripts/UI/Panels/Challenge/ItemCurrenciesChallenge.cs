using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCurrenciesChallenge : MonoBehaviour
{
    private float amout;
    private float priceBadges;
    [SerializeField]
    private TMP_Text txtPriceBadges;
    [SerializeField]
    private TMP_Text txtAmountGem;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private CurrencyType currencyType;

    public void SetUp(float amount, float priceBadges)
    {
        this.amout = amount;
        this.priceBadges = priceBadges; 
        txtAmountGem.text = "x " + amout;
        txtPriceBadges.text = priceBadges.ToString();
    }

    public void Buy()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.BADGES, priceBadges, OnBuySuccess);
    }

    public void OnBuySuccess(bool isSuccess)
    {
        if(isSuccess)
        {
            switch(currencyType)
            {
                case CurrencyType.GEM:
                    GameDatas.Gem += amout;
                    GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amout);
                    ObjectUI_Fly_Manager.instance.Get(7, transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.GEM);
                    break;
                case CurrencyType.POWER_STONE:
                    GameDatas.PowerStone += amout;
                    ObjectUI_Fly_Manager.instance.Get(7, transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.POWER_STONE);
                    break;
                case CurrencyType.ARMOR_SPHERE:
                    GameDatas.ArmorSphere += amout;
                    break;
                case CurrencyType.POWER_SPHERE:
                    GameDatas.PowerSphere += amout;
                    break;
                case CurrencyType.ENGINE_SPHERE:
                    GameDatas.EngineSphere += amout;
                    break;
                case CurrencyType.CRYOGENIC_SPHERE:
                    GameDatas.CryogenicSphere += amout;
                    break;
            }
        }
    }
}
