using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardManager", menuName = "Data/Cards/CardManager", order = 0)]
public class SO_CardManager : SerializedScriptableObject
{
    public Dictionary<TypeCard, List<Card_SO>> cardDicts = new();
    public Dictionary<CardID, Card_SO> cardInforDataDict = new();

    public List<Card_SO> GetAllCardByType(TypeCard typeCard)
    {
        if (cardDicts.ContainsKey(typeCard))
        {
            return cardDicts[typeCard];
        }
        return null;
    }

    public List<Card_SO> GetAllCard()
    {
        return cardDicts.Values.SelectMany(list => list).ToList();
    }

    public Card_SO GetSingleCardByIdCard(TypeCard typeCard, CardID cardID)
    {
        if (cardDicts.ContainsKey(typeCard))
        {
            return cardDicts[typeCard].FirstOrDefault(card => card.id == cardID);
        }
        return null;
    }

#if UNITY_EDITOR

    [Button("Init Data Infor")]
    public void InitData()
    {
        List<Card_SO> cards = GetAllCard();
        foreach (var data in cards)
        {
            cardInforDataDict[data.id] = data;
        }
    }

#endif
}
