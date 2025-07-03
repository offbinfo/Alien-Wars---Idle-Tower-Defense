using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardElementData : BaseUICellData
{
    private Card_SO card_SO;

    public Card_SO Card_SO => card_SO;

    public CardElementData(Card_SO card_SO)
    {
        this.card_SO = card_SO; 
    }

}
