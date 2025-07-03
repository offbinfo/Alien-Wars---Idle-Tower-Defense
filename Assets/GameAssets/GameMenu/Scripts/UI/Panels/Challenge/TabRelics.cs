using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabRelics : MonoBehaviour
{
    [SerializeField]
    private RelicManagerSO relicManagerSO;
    public float priceRare = 350f;
    public float priceEpic = 700f;
    [SerializeField]
    private List<ItemRelicChallenge> itemRelics;

    private void Start()
    {
         BuildData();
    }

    private void BuildData()
    {
        AllChallengeData data = GameDatas.LoadAllRelicItems();
        if (data.items == null) return;
        if (data.items.Count > 0)
        {
            gameObject.SetActive(true);
            itemRelics[0].SetUp(data.items[2], relicManagerSO.GetIconRelicData(data.items[2].typeRelic),
                GetPriceRelic(data.items[2].typeRarityRelic));
            itemRelics[1].SetUp(data.items[3], relicManagerSO.GetIconRelicData(data.items[3].typeRelic),
                GetPriceRelic(data.items[3].typeRarityRelic));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public float GetPriceRelic(TypeRarityRelic typeRarityRelic)
    {
        switch (typeRarityRelic)
        {
            case TypeRarityRelic.Rare: return priceRare;
            case TypeRarityRelic.Epic: return priceEpic;
            default: return priceRare;
        }
    }

}
