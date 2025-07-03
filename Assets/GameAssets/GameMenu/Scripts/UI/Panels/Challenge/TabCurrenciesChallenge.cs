using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPackConfig
{
    public float amount;
    public float badgePrice;
}

public class TabCurrenciesChallenge : MonoBehaviour
{
    [SerializeField]
    private List<ItemCurrenciesChallenge> packCurrenciesBig = new();
    [SerializeField]
    private List<ItemCurrenciesChallenge> packCurrenciesSmall = new();

    [Header("Big Pack Configuration")]
    [SerializeField] private CurrencyPackConfig gemConfig = new() { amount = 150, badgePrice = 40 };
    [SerializeField] private CurrencyPackConfig powerStoneConfig = new() { amount = 15, badgePrice = 40 };

    [Header("Small Pack Configuration")]
    [SerializeField] private CurrencyPackConfig upgradeConfig = new() { amount = 250, badgePrice = 40 };

    private void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        packCurrenciesBig[0].SetUp(gemConfig.amount, gemConfig.badgePrice);
        packCurrenciesBig[1].SetUp(powerStoneConfig.amount, powerStoneConfig.badgePrice);
        foreach (var item in packCurrenciesSmall)
        {
            item.SetUp(upgradeConfig.amount, upgradeConfig.badgePrice);
        }
    }
}
