using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjBuyPremiumPass : MonoBehaviour
{
    [SerializeField] private PurchaserPrice price;
    public string key;
    public int amountGem = 1100;
    public int amountPowerstone = 1100;
    [SerializeField]
    private TMP_Text txt_gem;
    [SerializeField]
    private TMP_Text txt_powerStone;

    private void Start()
    {
        price.Setup(key);
    }

    public void OnClick()
    {
        GamePurchaser.BuyProduct(key, () =>
        {
            GameDatas.IsUsingPremiumMileStones = true;

            GameDatas.Gem += amountGem;
            GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amountGem);
            ObjectUI_Fly_Manager.instance.Get(10, txt_gem.transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.GEM);

            GameDatas.PowerStone += amountPowerstone;
            ObjectUI_Fly_Manager.instance.Get(10, txt_powerStone.transform.position,
            TopUI_Currency.instance.powerStoneIcon.transform.position, CurrencyType.POWER_STONE);
            PanelClaimReward.instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
