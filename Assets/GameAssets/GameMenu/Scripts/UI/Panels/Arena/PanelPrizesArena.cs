using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPrizesArena : GameMonoBehaviour
{

    [SerializeField]
    private List<ItemPrizesArea> itemPrizesArea;
    [SerializeField]
    private List<PrizesElement> prizesElements;
    [SerializeField]
    private SO_RankData rankDataSO;

    public GameObject curPrizeSelect;

    private void Start()
    {
        BuildData();
        OnRefreshDataItemPrizesArena(TypeRank.Recruit);
    }

    private void BuildData()
    {
        foreach (var item in prizesElements)
        {
            item.OnClick = OnClickPrizeElement;
        }
    }

    private void OnClickPrizeElement(TypeRank typeRank, GameObject frameSelect)
    {
        if (curPrizeSelect != null)
        {
            curPrizeSelect.SetActive(false);

            OnRefreshDataItemPrizesArena(typeRank);
        }
        curPrizeSelect = frameSelect;
        curPrizeSelect.SetActive(true);
    }

    private void OnRefreshDataItemPrizesArena(TypeRank typeRank)
    {
        List<RankData> rankDatas = rankDataSO.GetAllRankDataByIndex(typeRank);
        for (int i = 0; i < rankDatas.Count; i++)
        {
            itemPrizesArea[i].SetData(rankDatas[i], rankDataSO.textRank[i]);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


}
