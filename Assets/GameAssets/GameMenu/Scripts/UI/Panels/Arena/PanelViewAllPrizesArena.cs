using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelViewAllPrizesArena : GameMonoBehaviour
{
    [SerializeField]
    private ItemRankArena itemRankArena;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TMP_Text txtTimeTourament;
    [SerializeField]
    private PanelPrizesArena panelPrizesArena;
    [SerializeField]
    private PanelHistoryArena panelHistoryArena;
    [SerializeField]
    private PanelInfoArena panelInfoArena;
    private int defaultUser = 30;
    [SerializeField]
    private SO_RankBotData SO_RankBotData;

    [SerializeField]
    SerializedDictionary<TypeRank, GameObject> iconRankDicts = new();

    private List<ItemRankArena> itemRankArenas = new();
    public bool isResetRank = false;
    [SerializeField]
    private Obj_ChangeName obj_ChangeName;
    private bool onRefreshRank;

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        InitDataRank();

        StartCoroutine(ReloadRankLazy());
    }

    private IEnumerator ReloadRankLazy()
    {
        yield return new WaitForSecondsRealtime(1f);
        DebugCustom.LogColor("ReloadRankLazy");
        OnRefreshData(null);
    }

    private void OnEnable()
    {
        TypeRank typeRank = (TypeRank)GameDatas.GetHighestRank();
        ChangeIconRank(typeRank);

        if (itemRankArenas.Count == 0) return;
        OnRefreshData(null);
    }

    private void InitDataRank()
    {
        TypeRank typeRank = (TypeRank)GameDatas.CurrentRank;
        StartCoroutine(SO_RankBotData.SwapWaveBotRanks(GameDatas.GetIndexRank(typeRank),
            GameDatas.GetHighestWaveInRank(typeRank)));

        for (int i = 0; i < defaultUser; i++)
        {
            ItemRankArena cellView = Instantiate(itemRankArena, content);
            cellView.SetData(SO_RankBotData.botList[i]);
            itemRankArenas.Add(cellView);
        }

        OnRefreshData(null);
    }


    public int indexTest;
    [Button("Test")]
    public void TEst()
    {
        GameDatas.SetHighestWaveInRank(TypeRank.Recruit, indexTest);

        OnRefreshData(null);
    }

    private void OnRefreshData(object o)
    {
        TypeRank typeRank = (TypeRank)GameDatas.CurrentRank;
        if (isResetRank)
        {
            StartCoroutine(SO_RankBotData.SwapBotRanks(GameDatas.GetIndexRank(typeRank),
                GameDatas.GetHighestWaveInRank(typeRank)));
        }
        else
        {
            StartCoroutine(SO_RankBotData.SwapWaveBotRanks(GameDatas.GetIndexRank(typeRank),
                 GameDatas.GetHighestWaveInRank(typeRank)));
        }

        for (int i = 0; i < itemRankArenas.Count; i++)
        {
            itemRankArenas[i].SetData(SO_RankBotData.botList[i]);
        }
    }

    private void ChangeIconRank(TypeRank typeRank)
    {
        foreach (GameObject obj in iconRankDicts.Values)
        {
            obj.SetActive(false);
        }
        iconRankDicts[typeRank].gameObject.SetActive(true);
    }

    public void BtnBattle()
    {
        if (GameDatas.user_name == "playerid_1")
        {
            obj_ChangeName.gameObject.SetActive(true);
        }
        else
        {
            if (GameDatas.Tourament == 0) return;
            GameDatas.Tourament -= 1;
            gameObject.SetActive(false);
            Gm.isFirstBattleArena = true;
            Gm.PlayArenaGame();
        }
    }

    public void BtnViewAllPrizes()
    {
        panelPrizesArena.gameObject.SetActive(true);
    }

    public void BtnInfo()
    {
        panelInfoArena.gameObject.SetActive(true);
    }

    public void BtnHistoryArena()
    {
        panelHistoryArena.gameObject.SetActive(true);
    }
    public void BtnClose()
    {
        gameObject.SetActive(false);
    }
}
