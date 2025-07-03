using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHistoryArena : MonoBehaviour
{
    [SerializeField]
    private ItemHistoryArena itemHistoryArena;
    [SerializeField]
    private Transform content;
    private int totalHistoryArea;

    private void OnEnable()
    {
        InitData();
    }

    private void InitData()
    {
        AllHistoryArenaData allHistoryArena = GameDatas.LoadAllHistoryArenas();
        if(allHistoryArena == null) return;
        if (totalHistoryArea == allHistoryArena.items.Count) return;
        totalHistoryArea = allHistoryArena.items.Count;
        foreach (var arena in allHistoryArena.items)
        {
            ItemHistoryArena itemHistoryCellView = Instantiate(itemHistoryArena, content);
            itemHistoryCellView.SetData(arena);
        }
    } 

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
