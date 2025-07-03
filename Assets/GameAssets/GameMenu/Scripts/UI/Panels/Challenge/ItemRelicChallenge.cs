using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRelicChallenge : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TMP_Text txtCategory;
    [SerializeField]
    private TMP_Text txtPrice;
    private float price;
    private TypeRelic typeRelic = TypeRelic.None;
    [SerializeField]
    private GameObject btnBuy;
    [SerializeField]
    private GameObject btnUnBuy;
    [SerializeField]
    private GameObject bgRare;
    [SerializeField]
    private GameObject bgEpic;

    public void SetUp(RelicData relicData, Sprite iconRelic, float priceBadges)
    {
        typeRelic = relicData.typeRelic;
        txtName.text = LanguageManager.GetText(typeRelic.ToString());
        txtCategory.text = LanguageManager.GetText(relicData.typeRarityRelic.ToString());
        price = priceBadges;
        txtPrice.text = price.ToString();

        if (relicData.typeRarityRelic == TypeRarityRelic.Rare)
        {
            bgRare.gameObject.SetActive(true);
            bgEpic.gameObject.SetActive(false);
        }
        else
        {
            bgRare.gameObject.SetActive(false);
            bgEpic.gameObject.SetActive(true);
        }

        SetImage(iconRelic);
        CheckUnlockRelic();
    }

    void SetImage(Sprite sprite)
    {
        if (sprite == null || icon == null) return;

        icon.sprite = sprite;
        float originalWidth = sprite.texture.width;
        float originalHeight = sprite.texture.height;

        float aspectRatio = originalWidth / originalHeight;

        RectTransform rt = icon.GetComponent<RectTransform>();

        if (aspectRatio > 1)
        {
            rt.sizeDelta = new Vector2(100, 100 / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(100 * aspectRatio, 100);
        }
    }

    public void Buy()
    {
        if (GameDatas.IsRelicUnlock(typeRelic)) return;
        GameDatas.BuyUsingCurrency(CurrencyType.BADGES, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool obj)
    {
        if (obj)
        {
            GameDatas.RelicUnlock(typeRelic);
            CheckUnlockRelic();
        }
    }

    private void CheckUnlockRelic()
    {
        if (typeRelic == TypeRelic.None) return;
        if(GameDatas.IsRelicUnlock(typeRelic))
        {
            btnBuy.SetActive(false);
            btnUnBuy.SetActive(true);
        }
        else
        {
            btnBuy.SetActive(true);
            btnUnBuy.SetActive(false);
        }
    }
}
