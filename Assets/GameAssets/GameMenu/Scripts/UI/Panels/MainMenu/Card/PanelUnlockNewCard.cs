using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUnlockNewCard : GameMonoBehaviour
{
    [SerializeField] List<CardElement> cardsUI;

    private void OnEnable()
    {
        foreach (var item in cardsUI)
        {
            item.gameObject.SetActive(false);
        }
    }
    public void ShowCard(Card_SO cardData)
    {
        gameObject.SetActive(true);
        cardsUI[0].gameObject.SetActive(true);
        CardElementData cellData = new(cardData);
        cardsUI[0].isCardIventory = false;
        cardsUI[0].SetData(cellData);
    }
    public void Show5Card(List<Card_SO> cardsData)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < cardsData.Count; i++)
        {
            cardsUI[i].gameObject.SetActive(true);
            CardElementData cellData = new(cardsData[i]);
            cardsUI[i].isCardIventory = false;
            cardsUI[i].SetData(cellData);
        }
    }
    public void OnClickBtnClose()
    {
        gameObject.SetActive(false);
    }

    /*private void ResetUICardUnlock()
    {
        foreach (var card in cardsUI) card.gameObject.SetActive(false);
    }

    public void ShowCard(Card_SO card)
    {
        gameObject.SetActive(true);
        cardsUI[0].gameObject.SetActive(true);
        CardElementData cellData = new(card);
        cardsUI[0].isCardIventory = false;
        cardsUI[0].SetData(cellData);
    }

    public void Show5Card(List<Card_SO> cardsData)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < cardsData.Count; i++)
        {
            cardsUI[i].gameObject.SetActive(true);
            CardElementData cellData = new(cardsData[i]);
            cardsUI[i].isCardIventory = false;
            cardsUI[i].SetData(cellData);
        }
    }

    private void OnDisable()
    {
        ResetUICardUnlock();
    }*/
}
