using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using language;

public class GoldShopElement : MonoBehaviour
{
    [SerializeField] TMP_Text txt_Gold;
    [SerializeField] TMP_Text txt_Price;
    [SerializeField] TMP_Text txt_PriceHide;
    [SerializeField] TMP_Text txt_time;
    [SerializeField] Button btn_freeClaim;
    [SerializeField] Transform center;
    [SerializeField] GameObject obj_BuyHide;
    [SerializeField] GameObject obj_Buy;
    private int gold;
    [SerializeField]
    private int price;
    [SerializeField]
    private BasePack basePack;
    private GoldPack goldPack;

    private void Awake()
    {
        goldPack = basePack as GoldPack;
        gold = goldPack.amount;
    }

    private int Gold => (int)(gold/* * GameDatas.userPower / GameDatas.basePower*/);
    public void Claim()
    {
        if (GameDatas.Gem >= price)
        {
            GameDatas.Gem -= price;
            var targetUI = TopUI_Currency.instance ? TopUI_Currency.instance.goldIcon.transform.position : CurrencyContainer.instance._trans_gold.position;
            ObjectUI_Fly_Manager.instance.Get(20, center.position, targetUI, CurrencyType.GOLD);
            GameDatas.Gold += Gold;
        }
    }
    public void ClaimFree()
    {
        if (DateTime.Now >= GameDatas.timeClaimFreeGold_Target)
        {
            GameDatas.timeClaimFreeGold_Target = DateTime.Now.AddHours(3);
            var targetUI = TopUI_Currency.instance ? TopUI_Currency.instance.goldIcon.transform.position : CurrencyContainer.instance._trans_gold.position;
            ObjectUI_Fly_Manager.instance.Get(20, center.position, targetUI, CurrencyType.GOLD);
            GameDatas.Gold += Gold;
            StartCoroutine(I_CountTime());
        }
    }
    IEnumerator I_CountTime()
    {
        btn_freeClaim.enabled = false;
        while (DateTime.Now < GameDatas.timeClaimFreeGold_Target)
        {
            var time = GameDatas.timeClaimFreeGold_Target - DateTime.Now;
            txt_time.text = time.Display();
            yield return new WaitForSecondsRealtime(1f);
        }
        txt_time.text = LanguageManager.GetText("free");
        btn_freeClaim.enabled = true;
    }
    private void OnEnable()
    {
        if (price <= 0) StartCoroutine(I_CountTime());
    }
    private void Start()
    {
        Setup();
        OnGemChanged(null);
        EventDispatcher.AddEvent(EventID.OnGemChanged, OnGemChanged);
    }
    private void OnGemChanged(object obj)
    {
        if (price <= 0)
            return;
        obj_Buy.SetActive(GameDatas.Gem >= price);
        obj_BuyHide.SetActive(!obj_Buy.activeInHierarchy);
    }
    private void Setup()
    {
        txt_Gold.text = Gold.ToString();
        var text = price.AddIcon(TextIcon.gem);

        txt_Price.text = text;
        txt_PriceHide.text = text;
    }
}

public static class TextIcon
{
    public static string gold = "<sprite name=icon_gold>";
    public static string gem = "<sprite name=icon_gem>";

    public static string AddIcon(this object s, string iconName)
    {
        return string.Format("{0} {1}", iconName, s);
    }
}