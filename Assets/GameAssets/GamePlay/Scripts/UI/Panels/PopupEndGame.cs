using DG.Tweening;
using language;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupEndGame : UIPanel, IBoard
{
    public Transform arrowCenter;
    public Transform X4;
    public Transform X2_5;
    public Transform X1;
    public Transform X1_2;
    public Transform X1_4;
    [SerializeField]
    private Transform trans_x2;
    [SerializeField]
    private BonusRewardEndGame obj_confirmRwdAds;

    public RectTransform trans_btnClaim;
    public DOTweenAnimation dt_RotateArrow;
    public TMP_Text txt_world;
    public TMP_Text txt_wave;
    public TMP_Text txt_highest_wave;
    public TMP_Text txt_gold;
    public TMP_Text txt_gem;
    bool isClaimed = false;
    long gold;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupEndGame;
    }
    private void OnEnable()
    {
        GameAnalytics.LogEventEndPlay(GameDatas.CurrentWorld, GPm.wavePlaying);
        OnAppear();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private void Init()
    {
        GPm.EndRoundWave(false);

        GameAnalytics.LogEvent_LoseGameWorld_x(GameDatas.CurrentWorld + 1, GPm.wavePlaying);
        TimeGame.Pause = true;

        var multiply = 1f 
            /*+ (Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.GOLD_BONUS).GetCurrentProperty() / 100f)*/;

        isClaimed = false;
        dt_RotateArrow.DOPlay();

        if(Gm.isBattleArena)
        {
            TypeRank typeRank = (TypeRank)GameDatas.CurrentRank;

            CheckAndUpdateRank(typeRank);

            txt_world.text = LanguageManager.GetText("rank") + " " +LanguageManager.GetText(typeRank.ToString());
            txt_highest_wave.text = string.Format(LanguageManager.GetText("higest_wave_x"), GameDatas.GetHighestWaveInRank(typeRank));
        } else
        {
            txt_world.text = string.Format(LanguageManager.GetText("world_x"), GameDatas.CurrentWorld + 1);
            txt_highest_wave.text = string.Format(LanguageManager.GetText("higest_wave_x"), GameDatas.GetHighestWaveInWorld(GameDatas.CurrentWorld));
        }

        txt_wave.text = string.Format(LanguageManager.GetText("wave_x"), GPm.wavePlaying);
        txt_gold.text = ((int)(GPm.GoldInGame * multiply)).ToString();
        txt_gem.text = GPm.GemInGame.ToString();
        //reset pos x2rwd;
        trans_x2.gameObject.SetActive(GameDatas.activeX2);


        if (GameDatas.activeX2)
        {
            gold = (int)(GPm.GoldInGame * 2 * multiply);
            StartCoroutine(IE_X2Event(multiply));
        }
        else
            gold = (int)(GPm.GoldInGame * multiply);
    }
    IEnumerator IE_X2Event(float multiply)
    {
        float gold = GPm.GoldInGame;
        bool isFirstGoldTouch = false;
        ObjectUI_Fly_Manager.instance.Get(15, trans_x2.position, txt_gold.transform.position, CurrencyType.GOLD, null, () =>
        {
            isFirstGoldTouch = true;
        });
        yield return new WaitUntil(() => isFirstGoldTouch);
        var sumgold = (int)(gold * 2 * multiply);
        DOVirtual.Float(gold, sumgold, 0.75f, (i) =>
        {
            txt_gold.text = i.ToString();
        }).OnComplete(() =>
        {
            txt_gold.text = sumgold.ToString();
        }).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.75f);
    }

    public float CalculateRewardMultiply()
    {
        var angle = Vector2.Angle(-arrowCenter.right, Vector2.right);
        var angleX4 = Vector2.Angle((X4.position - arrowCenter.position).normalized, Vector2.right);
        var angleX2_5 = Vector2.Angle((X2_5.position - arrowCenter.position).normalized, Vector2.right);
        var angleX1 = Vector2.Angle((X1.position - arrowCenter.position).normalized, Vector2.right);
        var angleX1_2 = Vector2.Angle((X1_2.position - arrowCenter.position).normalized, Vector2.right);
        if (angle <= angleX4)
            return 4f;
        else if (angle <= angleX2_5)
            return 2.5f;
        else if (angle <= angleX1)
            return 1f;
        else if (angle <= angleX1_2)
            return 1.2f;
        else
            return 1.4f;
    }

    private void ClaimRwd(float multiply)
    {
        var totalGoldEarn = (int)(gold * multiply);
        GameDatas.Gold += totalGoldEarn/* - GPm.GoldInGame*/;
        GameAnalytics.LogEvent_EarnGold("play_game", (totalGoldEarn - GPm.GoldInGame));
        GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + totalGoldEarn);

        var highestGoldEarn = GameDatas.GetDataProfile(IDInfo.HighestBattleGoldEarned);
        if (totalGoldEarn > highestGoldEarn)
            GameDatas.SetDataProfile(IDInfo.HighestBattleGoldEarned, totalGoldEarn);

        ObjectUI_Fly_Manager.instance.Get(20, trans_btnClaim.position, CurrencyContainer.instance.trans_gold.position, CurrencyType.GOLD, () =>
        {
            TimeGame.Pause = false;
            LoadSceneManager.instance.LoadScene(TypeScene.GameMenu);
        });
    }

    public void OnClickBtnClaim()
    {
        if (isClaimed) return;
        isClaimed = true;
        dt_RotateArrow.DOPause();
        var multiply = CalculateRewardMultiply();
        if (multiply >= 2.5f)
        {
            var totalGoldEarn = (int)(gold * multiply);
            obj_confirmRwdAds.Open(multiply, gold);
        }
        else
            ClaimRwd(multiply);
    }

    private static readonly int botEasyTime = 20;
    private static readonly int botMidTime = 30;
    private static readonly int botHardTime = 40;

    /*    private static readonly int botEasyTime = 1;
        private static readonly int botMidTime = 2;
        private static readonly int botHardTime = 3;*/

    /*    private void CheckSaveFirstHistoryArena(TypeRank typeRank)
        {
            if (!GameDatas.IsSaveFirstHistoryArena)
            {
                AllHistoryArenaData allHistoryArenaData = GameDatas.LoadAllHistoryArenas();
                if (allHistoryArenaData == null)
                {
                    ItemHistoryArenaData itemHistoryArena = new(typeRank, GameDatas.GetHighestWaveInRank(typeRank),
                        DateTime.Now.ToString("o"), GameDatas.GetIndexRank(typeRank));
                    GameDatas.SaveAllHistoryArena(itemHistoryArena);
                }
                GameDatas.IsSaveFirstHistoryArena = true;
            }
        }*/

    public void CheckAndUpdateRank(TypeRank typeRank)
    {
        //CheckSaveFirstHistoryArena(typeRank);
        GameDatas.StopBattleAndAddTime();

        int playTimeMinutesPerDay = GameDatas.GetPlayTimeMinutesPerDay();

        // Lấy indexRank của người chơi
        int currentRank = GameDatas.GetIndexRank(typeRank);

        DebugCustom.LogColor("playTimeMinutesPerDay " + playTimeMinutesPerDay);
        // Nếu người chơi chơi đủ 20 phút (vượt qua bot Easy), giảm indexRank
        if (playTimeMinutesPerDay > botEasyTime)
        {
            // Giảm indexRank của người chơi, nhưng không nhỏ hơn 1
            currentRank = Mathf.Max(currentRank - 1, 1);
        }

        // Nếu người chơi chơi đủ 30 phút (vượt qua bot Mid), giảm indexRank
        if (playTimeMinutesPerDay > botMidTime)
        {
            currentRank = Mathf.Max(currentRank - 1, 1);
        }

        // Nếu người chơi chơi đủ 40 phút (vượt qua bot Hard), giảm indexRank
        if (playTimeMinutesPerDay > botHardTime)
        {
            currentRank = Mathf.Max(currentRank - 1, 1);
        }

        if (GameDatas.GetHighestWaveInRank(typeRank) < GPm.wavePlaying)
        {
            ItemHistoryArenaData itemHistoryArena = new(typeRank, GameDatas.GetHighestWaveInRank(typeRank), DateTime.Now.ToString("o"), currentRank);
            GameDatas.SaveAllHistoryArena(itemHistoryArena);

            GameDatas.SetIndexRank(typeRank, currentRank);
        }
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