using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardSpinElement : MonoBehaviour
{
    [SerializeField]
    private GameObject iconGold;
    [SerializeField]
    private GameObject iconGem;
    [SerializeField]
    private GameObject iconPowerStone;
    [SerializeField]
    private GameObject iconGlow;
    [SerializeField]
    private TMP_Text txtAmount;

    public void SetData(CurrencyType currencyType, int amount)
    {
        switch (currencyType)
        {
            case CurrencyType.GOLD:
                iconGold.gameObject.SetActive(true);
                break;
            case CurrencyType.GEM:
                iconGem.gameObject.SetActive(true);
                break;
            case CurrencyType.POWER_STONE:
                iconPowerStone.gameObject.SetActive(true);  
                break;
            default:
                break;
        }

        txtAmount.text = Extensions.FormatNumber(amount);
    }

}
