using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupArena : UIPanel, IBoard
{
    [SerializeField]
    private PanelInfoArena panelInfoArena;
    [SerializeField]
    private PanelHistoryArena historyArena;
    [SerializeField]
    private PanelViewAllPrizesArena viewAllPrizesArena;
    [SerializeField]
    private PanelPrizesArena panelPrizesArena;
    [SerializeField]
    private PanelRewardEndArena panelRewardEndArena;

    [SerializeField]
    private TMP_Text txtGemReward;
    [SerializeField]
    private TMP_Text txtPowerReward;
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private SO_RankData sO_Rank;
    [SerializeField]
    private Obj_ChangeName obj_ChangeName;

    [SerializeField]
    SerializedDictionary<TypeRank, GameObject> iconRankDicts = new();

    [SerializeField]
    private GameObject lockArena;
    private float timeInterval = 20f * 60f;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupArena;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private void CheckResetRank()
    {
        if (GameDatas.IsResetRank())
        {
            string lastLogTimeString = GameDatas.GetLastLogTimeRank();
            DateTime lastLogTime = DateTime.Parse(lastLogTimeString);

            if ((DateTime.Now - lastLogTime).TotalMinutes >= 20)
            {
                LogAndSaveTime();
            }
            else
            {
                viewAllPrizesArena.isResetRank = false;
                //DebugCustom.Log("Chưa đến 20 phút từ lần log cuối.");
            }
        }
        else
        {
            LogAndSaveTime();
        }
    }

    void LogAndSaveTime()
    {
        viewAllPrizesArena.isResetRank = true;
        GameDatas.SetLastLogTimeRank();
    }
    
    private void CheckNewEvent()
    {
        if (GameDatas.CheckNewArenaEvent())
        {
            GameDatas.Tourament = 5;
            if (GameDatas.GetHighestRank() == 0 && GameDatas.GetIndexRank(TypeRank.Recruit) == 30) return;
            if (!GameDatas.IsClaimRewardArenaRank() && GameDatas.IsFirstPlayArena)
            {
                if (GameDatas.GetIndexRank((TypeRank)GameDatas.CurrentRank) <= 4)
                {
                    GameDatas.IncreaseRank();
                    panelRewardEndArena.InitData();
                }

                panelRewardEndArena.gameObject.SetActive(true);
                panelRewardEndArena.InitData();
            }
        }
    }


    private void CheckActiveArena()
    {
        /*DebugCustom.LogColor("Extensions.CheckArenaTime() " + Extensions.CheckArenaTime());
        GameDatas.Tourament = 50;*/

        if (Extensions.CheckArenaTime())
        {
            if(!GameDatas.IsActiveEventArena())
            {
                if (GameDatas.TotalArenaRankActive > GameDatas.TotalArenaRankPlay)
                {
                    GameDatas.DecreaseRank();
                    panelRewardEndArena.InitData();

                    GameDatas.TotalArenaRankActive = 0;
                    GameDatas.TotalArenaRankPlay = 0;
                }
                GameDatas.TotalArenaRankActive += 1;
            }

            GameDatas.IsFirstPlayArena = true;

            CheckNewEvent();

            GameDatas.ActiveEventArena(true);
            if (!GameDatas.IsResetTouramentArena())
            {
                GameDatas.Tourament = 5;
                GameDatas.ResetTouramentArena(true);
            }
            lockArena.SetActive(false);
            GameDatas.ClaimRewardArenaRank(false);
        }
        else
        {
            lockArena.SetActive(true);

            if (!GameDatas.IsClaimRewardArenaRank() && GameDatas.GetIndexRank((TypeRank)GameDatas.CurrentRank) != 30)
            {
                panelRewardEndArena.gameObject.SetActive(true);
            }

            if (!GameDatas.IsActiveEventArena()) return;

            if (GameDatas.IsBattleArena())
            {
                GameDatas.StartBattleArena(false);
                panelRewardEndArena.gameObject.SetActive(true);
            }

            if ((GameDatas.GetIndexRank((TypeRank)GameDatas.CurrentRank)
                >= 27 && GameDatas.CurrentRank >= 2) /*|| !GameDatas.IsArenaChain()*/)
            {
                DebugCustom.LogColor("DecreaseRank");
                GameDatas.DecreaseRank();
                panelRewardEndArena.InitData();
            }
            else
            {
                if (GameDatas.GetIndexRank((TypeRank)GameDatas.CurrentRank) <= 4)
                {
                    DebugCustom.LogColor("IncreaseRank");
                    GameDatas.IncreaseRank();
                    panelRewardEndArena.InitData();
                }
            }

            GameDatas.ArenaChain(false);
            GameDatas.ActiveEventArena(false);
            GameDatas.ResetTouramentArena(false);
            GameDatas.ResetPlayTime();
        }
    }

    private void Init()
    {
        TopUI_Currency.instance.touramanent.SetActive(true);

        CheckResetRank();
        CheckActiveArena();
        if (Gm.isFirstBattleArena) viewAllPrizesArena.gameObject.SetActive(true);

        TypeRank typeRank = (TypeRank)GameDatas.GetHighestRank();
        RankData rankData = sO_Rank.GetRankDataByIndex
            (GameDatas.GetIndexRank(typeRank), typeRank);

        txtGemReward.text = rankData.gem.ToString();
        txtPowerReward.text = rankData.powerStone.ToString();

        ChangeIconRank(typeRank);
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
        } else
        {
            if(GameDatas.Tourament == 0) return;
            GameDatas.Tourament -= 1;
            Gm.isFirstBattleArena = true;
            Gm.PlayArenaGame();
        }
    }

    private void OnDisable()
    {
        TopUI_Currency.instance.touramanent.SetActive(false);
    }

    public void BtnViewAllPrizes()
    {
        panelPrizesArena.gameObject.SetActive(true);
    }

    public void BtnViewRank()
    {
        viewAllPrizesArena.gameObject.SetActive(true);
    }

    public void BtnHistory()
    {
        historyArena.gameObject.SetActive(true);   
    }

    public void BtnInfo()
    {
        panelInfoArena.gameObject.SetActive(true);
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}
