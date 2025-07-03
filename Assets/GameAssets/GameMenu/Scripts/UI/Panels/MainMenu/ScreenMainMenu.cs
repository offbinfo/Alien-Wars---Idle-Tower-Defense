using EasyTransition;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenMainMenu : UIPanel, IBoard
{
    [SerializeField]
    private Image imgAvatar;
    [SerializeField]
    private GameObject iconBannerReward;
    [SerializeField]
    private GameObject obj_BtnBeginnerQuest;
    [SerializeField] GameObject obj_notiBeginnerQuest;
    [SerializeField] GameObject obj_notiDailyQuest;
    [SerializeField] GameObject obj_notiDailyReward;
    [SerializeField] GameObject obj_notiMileStone;

    [SerializeField]
    private Animator animatorDropDownChallenge;
    public bool isDropDown;

    private void Awake()
    {
        CheckActiveBeginnerQuests();
    }

    private void CheckActiveBeginnerQuests()
    {
        //logic kích hoạt 
        var timeTargetBQ = GameDatas.timeTargetBeginnerQuest;
        if(!GameDatas.FirstInGameActiveBeginnerQuest)
        {
            timeTargetBQ = DateTime.Now.AddDays(3);
            GameDatas.timeTargetBeginnerQuest = timeTargetBQ;

            GameDatas.FirstInGameActiveBeginnerQuest = true;
        }

        if(DateTime.Now >= timeTargetBQ)
        {
            GameDatas.ActiveBeginnerQuests = true;
        }
        obj_BtnBeginnerQuest.SetActive(DateTime.Now < timeTargetBQ);
        if(GameDatas.ActiveBeginnerQuests)
        {
            obj_BtnBeginnerQuest.SetActive(false);
        }

        //login count
        var timeStart = GameDatas.timeStart;
        if (timeStart.Date != DateTime.Now.Date)
        {
            GameDatas.timeStart = DateTime.Now.Date;
            GameDatas.CountDaysLogin += 1;
        }

        //remain 
        var timeStart30min = GameDatas.timeStart_30minRemain;
        if (timeStart30min == default)
            GameDatas.timeStart_30minRemain = DateTime.Now;
    }

    private void Start()
    {
        CheckShowPopupWelcomeBack();
        StartTutorial();
        EventDispatcher.AddEvent(EventID.OnChangedAvatar, ChangeInforUser);
        ChangeInforUser(null);
        CheckActiveIconBanner();

        ActiveNotiBeginnerQuests();
    }

    private void CheckShowPopupWelcomeBack()
    {
        if(GameDatas.IsFirstTimeGoHome) return;
        if (GameDatas.IsActiveWelcomeBack())
        {
            GameDatas.ActiveWelcomeBack(false);
            Gui.OpenBoard(UiPanelType.PopupResumeGame);
        }
    }

    private void ActiveNotiBeginnerQuests()
    {
        obj_notiBeginnerQuest.SetActive(GameDatas.CheckNotiBeginnerQuest);
        EventDispatcher.AddEvent(EventID.OnBeginnerQuestProgressChanged, (o) =>
        {
            obj_notiBeginnerQuest.SetActive(GameDatas.CheckNotiBeginnerQuest);
        });
        EventDispatcher.AddEvent(EventID.OnBeginnerQuestClaim, (o) =>
        {
            obj_notiBeginnerQuest.SetActive(GameDatas.CheckNotiBeginnerQuest);
        });
    }

    private void ChangeInforUser(object obj)
    {
        imgAvatar.sprite = AvatarSource.instance.dataAvatars[GameDatas.id_avatar].sprite;
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.ScreenMainMenu;
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

    private void CheckActiveIconBanner()
    {
        DateTime now = DateTime.UtcNow;
        DateTime firstOpen = GameDatas.FirstOpenDate;
        double daysSinceOpen = (now - firstOpen).TotalDays;

        if (daysSinceOpen >= 14)
        {
            DebugCustom.LogColor("daysSinceOpen " + daysSinceOpen);
            GameDatas.LastBannerCycle += 1;
            GameDatas.ResetBannerCycle(now);
            firstOpen = now;
            daysSinceOpen = 0;
            GameDatas.ActiveBannerPack(false);
        }

        bool showIcon = GameDatas.LastBannerCycle % 2 != 0
            ? GameDatas.GetHighestWaveInWorld(0) >= 10
            : daysSinceOpen < 3;

        if(daysSinceOpen >= 3)
        {
            GameDatas.ActiveBannerPack(true);
        }

        iconBannerReward.SetActive(showIcon);
        if(GameDatas.IsActiveBannerPack())
        {
            iconBannerReward.SetActive(false);
        }
    }

    private void Init()
    {
        ResetDataTutorial();

        //open Arena
        if(Gm.isBattleArena)
        {
            StartCoroutine(DelayShowArena());
        }
    }

    private IEnumerator DelayShowArena()
    {
        yield return Yielders.Get(1f);
        Gui.OpenBoard(UiPanelType.PopupArena);
    }

    public void ResetDataTutorial()
    {
        if (!GameDatas.IsResetData)
        {
            GameDatas.IsResetData = true;
            GameDatas.ResetUpgraderTowerDefault();
        }
    }

    public void PopupClaimReward()
    {
        Gui.OpenBoard(UiPanelType.PopupClaimReward);
    }

    public void AddGold()
    {
        GameDatas.Gold = 1000000000000;
        GameDatas.PowerStone = 1000000000;
        GameDatas.Gem = 1000000000;
        GameDatas.timeGameMax = 5;
        GameDatas.Tourament = 5;
        GameDatas.Badges = 100000000;

        foreach (TypeRelic type in Enum.GetValues(typeof(TypeRelic)))
        {
            GameDatas.RelicUnlock(type);
        }
    }

    public void BtnCHeatRank()
    {
        GameDatas.SetIndexRank((TypeRank)GameDatas.GetHighestRank(), 5);
    }

    public void StartTutorial()
    {
        GameDatas.IsEndTutorial = true;
        if (GameDatas.IsFirstTimeGoHome)
        {
            TutorialManager.instance.StartTutorial_UpgradeGoldForever();
            GameDatas.IsTut_PlayDemo = false;
            GameDatas.IsFirstTimeGoHome = false;
        }
    }

    public void BtnPlayGame()
    {
        //Gm.PlayGame(false);
        if (!GameDatas.IsResumeWave(GameDatas.CurrentWorld))
        {
            Gm.PlayGame(false);
        }
        else
        {
            Gui.OpenBoard(UiPanelType.PopupResumeGame);
        }
    }

    public void BtnQuestsReward()
    {
        Gui.OpenBoard(UiPanelType.PopupQuestReward);
    }

    public void BtnSetting()
    {
        Gui.OpenBoard(UiPanelType.PopupSettingsHome);
    }

    public void DropDownChallenge()
    {
        if (isDropDown)
        {
            isDropDown = false;
            animatorDropDownChallenge.Play("Close");
        }
        else
        {
            isDropDown = true;
            animatorDropDownChallenge.Play("Open");
        }
    }

    public void BtnEvent()
    {
        TopUI_Currency.instance.gameObject.SetActive(false);
        Gui.OpenBoard(UiPanelType.PopupEvent);
    }

    public void BtnChange()
    {
        Gui.OpenBoard(UiPanelType.PopupChangeRelics);
    }

    public void BtnBannerReward()
    {
        Gui.OpenBoard(UiPanelType.PopupBannerReward);
    }

    public void BtBeginnerQuest()
    {
        Gui.OpenBoard(UiPanelType.PopupBeginnerQuestReward);
    }

    public void BtnArena()
    {
        if (!GameDatas.IsUnlockFeatureArena) return;
        Gui.OpenBoard(UiPanelType.PopupArena);
    }

    public void BtnRank()
    {
        if (!GameDatas.isUnlockRanking) return;
        Gui.ShowPanelNoti();
       // Gui.OpenBoard(UiPanelType.PopupRanking);
    }

    public void BtnUltimateWeapon()
    {
        Gui.OpenBoard(UiPanelType.PopupUltimateWeapon);
    }

    public void BtnMileStones()
    {
        Gui.OpenBoard(UiPanelType.PopupMileStones);
    }

    public void BtnFlipImage()
    {
        Gui.OpenBoard(UiPanelType.PopupFlipImage);
    }

    public void BtnDailyReward()
    {
        if(!GameDatas.isUnlockDailyGift) return;
        //Gui.ShowPanelNoti();
        Gui.OpenBoard(UiPanelType.PopupDailyGift);
    }

    public void BtnDailyQuest()
    {
        //Gui.ShowPanelNoti();
        Gui.OpenBoard(UiPanelType.PopupDailyQuest);
    }

    public void BtnLuckyDraw()
    {
        if (!GameDatas.isUnlockLuckyDraw) return;
        //Gui.ShowPanelNoti();
        Gui.OpenBoard(UiPanelType.PopupLuckyDraw);
    }

    public void BtnNewTextX2()
    {
        Gui.OpenBoard(UiPanelType.PopupNewText);
    }

    public void WaveClaimReward()
    {
        
    }

    public void BtnInfo()
    {
        Gui.OpenBoard(UiPanelType.PopupInfo);
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

    private void OnDisable()
    {
       
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}