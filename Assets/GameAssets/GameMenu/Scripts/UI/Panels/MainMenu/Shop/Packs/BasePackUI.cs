using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class BasePackUI : GameMonoBehaviour
{

    [SerializeField] protected PurchaserPrice price;
    [SerializeField] protected TMP_Text txtAmount;
    [SerializeField] protected BasePack basePack;
    [SerializeField] protected GameObject obj_claimed;
    [SerializeField] protected string key;
    [SerializeField]
    protected TMP_Text txtGift;
    [SerializeField]
    protected virtual bool claimed
    {
        get;
    }

    private void OnEnable()
    {
        ClaimedActive();
    }

    public virtual void BuyItem()
    {
        GamePurchaser.BuyProduct(key, () => {
            OnBought();
            ClaimedActive();
        });
    }

    public abstract void OnBought();
    
    public virtual void ClaimedActive()
    {
        if (obj_claimed != null)
        {
            obj_claimed.SetActive(claimed);
        }
    }

    public virtual void OnConllect(int amountGem)
    {
        GameDatas.Gem += amountGem;
        GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amountGem);
        var targetUI = TopUI_Currency.instance ? TopUI_Currency.instance.gemIcon.transform.position : CurrencyContainer.instance._trans_gem.position;
        ObjectUI_Fly_Manager.instance.Get(20, transform.position, targetUI, CurrencyType.GEM);
    }

    public virtual void SetUpTextAmountPackGolden(CurrencyType currencyType
    , TypeXBonusCurrency typeXBonusCurrency, int amountGold, int amountGem)
    {
        txtGift.text = typeXBonusCurrency.ToString();
        switch (currencyType)
        {
            case CurrencyType.GOLD:
                txtAmount.text = amountGold.ToString();
                break;
            case CurrencyType.GEM:
                txtAmount.text = amountGem.ToString();
                break;
            default:
                break;
        }
    }
}
