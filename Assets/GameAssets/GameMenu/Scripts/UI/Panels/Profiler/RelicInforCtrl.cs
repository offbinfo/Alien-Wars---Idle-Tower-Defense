using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInforCtrl : GameMonoBehaviour
{

    [SerializeField]
    private Transform content;
    public HashSet<TypeRelic> relics = new();
    [SerializeField]
    private List<ItemRelicInfor> itemRelics = new();
    [SerializeField]
    private RelicManagerSO relicManagerSO;

    private void Start()
    {
        OnRefreshRelicEquip(null);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, OnRefreshRelicEquip);
    }

    private void OnRefreshRelicEquip(object obj)
    {
        relics.Clear();
        foreach (TypeRelic typeRelic in Enum.GetValues(typeof(TypeRelic)))
        {
            if(GameDatas.IsRelicEquiped(typeRelic))
            {
                relics.Add(typeRelic);
            } 
        }
        int index = 0;
        foreach (var relic in relics)
        {
            RelicData cellData = relicManagerSO.GetRelicData(relic);
            itemRelics[index].SetUp(cellData);
            index++;
        }

        int activeCount = GameDatas.IndexSlotRelic();
        for (int i = 0; i < itemRelics.Count; i++)
        {
            itemRelics[i].gameObject.SetActive(i < activeCount);
        }
    }
}
