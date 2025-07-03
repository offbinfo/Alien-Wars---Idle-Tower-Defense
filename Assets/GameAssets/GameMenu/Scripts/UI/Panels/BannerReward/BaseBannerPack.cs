using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseBannerPack : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtNamePack;
    [SerializeField]
    private TMP_Text txtAmount1;
    [SerializeField]
    private TMP_Text txtAmount2;
    [SerializeField]
    protected int indexPack;
    [SerializeField]
    protected string key;
    [SerializeField]
    private BannerPackSO bannerPackSO;
    protected TypeBannerPack typeBannerPack;
    protected float amountGem;
    protected float amountGold;
    protected float amountPowerstone;
    [SerializeField] protected PurchaserPrice price;
    protected bool isBuy = true;

    protected virtual void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        price.Setup(key);

        txtNamePack.text = LanguageManager.GetText(bannerPackSO.typeBannerPack.ToString()) + " "+(indexPack + 1);
        typeBannerPack = bannerPackSO.typeBannerPack;

        amountGem = bannerPackSO.amountGem;
        switch(typeBannerPack)
        {
            case TypeBannerPack.StonePack:
                amountPowerstone = bannerPackSO.amountPowerStone;
                txtAmount1.text = amountPowerstone.ToString();
                txtAmount2.text = amountGem.ToString();
                break;
            case TypeBannerPack.BeginnerPack:
                amountGold = bannerPackSO.amountGold;
                txtAmount1.text = amountGold.ToString();
                txtAmount2.text = amountGem.ToString();
                break;
            default:
                break;
        }
    }

    public void BuyPack()
    {
        if(!isBuy) return;
        GamePurchaser.BuyProduct(key, () => {
            OnBought();
        });
    }

    public virtual void OnBought()
    {
        if (typeBannerPack == TypeBannerPack.BeginnerPack)
        {
            GameDatas.BuyBeginnerPack(indexPack, true);
        }
        AddCurrency(amountGem, TopUI_Currency.instance.gemIcon.transform, CurrencyType.GEM);
        AddCurrency(amountGold, TopUI_Currency.instance.goldIcon.transform, CurrencyType.GOLD);
        AddCurrency(amountPowerstone, TopUI_Currency.instance.powerStoneIcon.transform, CurrencyType.POWER_STONE);
    }

    private void AddCurrency(float amount, Transform icon, CurrencyType type)
    {
        if (amount <= 0) return;

        switch(type)
        {
            case CurrencyType.GOLD:
                GameDatas.Gold += amount;
                break;
            case CurrencyType.GEM:
                GameDatas.Gem += amount;
                break;
            case CurrencyType.POWER_STONE:
                GameDatas.PowerStone += amount;
                break;
            default:
                break;
        }

        ObjectUI_Fly_Manager.instance.Get(10, transform.position, icon.position, type);
    }
}

