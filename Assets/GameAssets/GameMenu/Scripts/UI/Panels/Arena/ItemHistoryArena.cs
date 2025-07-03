using AYellowpaper.SerializedCollections;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHistoryArena : MonoBehaviour
{
    [SerializeField]
    [SerializedDictionary("TypeRank", "Icon")]
    SerializedDictionary<TypeRank, GameObject> iconRankDict = new();

    [SerializeField]
    private TMP_Text txtRank;
    [SerializeField]
    private TMP_Text txtMaxWave;
    [SerializeField]
    private TMP_Text txtDate;
    [SerializeField]
    private TMP_Text txtDesc;

    public void SetData(ItemHistoryArenaData itemHistoryArenaData)
    {
        foreach(var item in iconRankDict.Values)
        {
            item.gameObject.SetActive(false);
        }
        iconRankDict[itemHistoryArenaData.typeRank].gameObject.SetActive(true);

        txtRank.text = itemHistoryArenaData.typeRank.ToString();
        txtMaxWave.text = string.Format(LanguageManager.GetText("max_wave_x"), itemHistoryArenaData.maxWave);

        DateTime parsedTime = DateTime.Parse(itemHistoryArenaData.time);
        txtDate.text = parsedTime.ToString("d/M/yyyy");
        txtDesc.text = string.Format(LanguageManager.GetText("you_finished_at_rank_x"), itemHistoryArenaData.indexRank);
    }

}
