using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPrizesArea : GameMonoBehaviour
{
    [SerializeField]
    private TMP_Text txtRank;
    [SerializeField]
    private TMP_Text txtGem;
    [SerializeField]
    private TMP_Text txtPowerstone;


    public void SetData(RankData rankData, string nameRank)
    {
        txtRank.text =  LanguageManager.GetText("rank")+" " + nameRank;
        txtGem.text = Extensions.FormatNumber(rankData.gem);
        txtPowerstone.text = Extensions.FormatNumber(rankData.powerStone);
    }
    
}
