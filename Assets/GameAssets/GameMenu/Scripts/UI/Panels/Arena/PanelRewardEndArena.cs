using AYellowpaper.SerializedCollections;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelRewardEndArena : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtGem;
    [SerializeField]
    private TMP_Text txtPowerStone;
    [SerializeField]
    private TMP_Text txtCurRank;
    [SerializeField]
    private TMP_Text indexRank;
    [SerializeField]
    SerializedDictionary<TypeRank, GameObject> iconRankDicts = new();
    [SerializeField]
    private SO_RankData sO_Rank;

    private float gem;
    private float powerStone;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnOnRefreshUIRankArena, OnRefreshUI);
    }

    private void OnRefreshUI(object obj)
    {
        InitData();
    }

    private void OnEnable()
    {
        InitData();
    }

    public void InitData()
    {
        TypeRank typeRank = (TypeRank)GameDatas.GetHighestRank();

        TypeRank typeRankOld = GetAdjustedRank(typeRank);

        RankData rankData = sO_Rank.GetRankDataByIndex
            (GameDatas.GetIndexRank(typeRankOld), typeRankOld);

        gem = rankData.gem;
        powerStone = rankData.powerStone;

        txtCurRank.text = LanguageManager.GetText("new_rank_") +" " + typeRank;
        txtPowerStone.text = gem.ToString();
        txtGem.text = powerStone.ToString();

        indexRank.text = LanguageManager.GetText("index_old_rank") + " " +GameDatas.GetIndexRank(typeRankOld);

        ChangeIconRank(typeRank);
    }

    public TypeRank GetAdjustedRank(TypeRank typeRank)
    {
        if (typeRank == TypeRank.Recruit || typeRank == TypeRank.General)
        {
            return typeRank;
        }

        return (TypeRank)((int)typeRank - 1);
    }

    private void ChangeIconRank(TypeRank typeRank)
    {
        foreach (GameObject obj in iconRankDicts.Values)
        {
            obj.SetActive(false);
        }
        iconRankDicts[typeRank].gameObject.SetActive(true);
    }

    public void Claimed()
    {
        GameDatas.ClaimRewardArenaRank(true);
        if (gem > 0)
        {
            GameDatas.Gem += gem;
            GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + gem);
            ObjectUI_Fly_Manager.instance.Get(7, txtGem.transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.GEM);
        }

        if (powerStone > 0)
        {
            GameDatas.PowerStone += powerStone;
            ObjectUI_Fly_Manager.instance.Get(7, txtPowerStone.transform.position,
            TopUI_Currency.instance.powerStoneIcon.transform.position, CurrencyType.POWER_STONE);
        }
        gameObject.SetActive(false);
    }
}
