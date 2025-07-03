using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicsContainer : MonoBehaviour
{
    [SerializeField]
    private TypeRarityRelic typeRarityRelic;
    [SerializeField]
    private ItemRelicSelect itemRelicSelect;
    [SerializeField]
    private Transform content;
    
    public void SetUp(List<RelicData> relicDatas)
    {
        foreach (RelicData data in relicDatas)
        {
            ItemRelicSelect cellView = Instantiate(itemRelicSelect, content);
            cellView.SetUp(data);
        }
    }

}
