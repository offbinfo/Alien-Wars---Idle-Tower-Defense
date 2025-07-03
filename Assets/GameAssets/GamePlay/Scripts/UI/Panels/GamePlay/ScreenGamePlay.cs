using DG.Tweening;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenGamePlay : UIPanel, IBoard
{
    [SerializeField]
    private WaveInfoUI waveInfoUI;
    [SerializeField] private Animator animContentPanelTop;
    [SerializeField] private Animator animContentPanelBottom;
    [SerializeField] private Animator animContainerTab;
    [SerializeField] private RectTransform buttonSpeed;
    [SerializeField] private RectTransform buttonBonusGem;
    [SerializeField] private RectTransform containerUpgrader;

    [SerializeField]
    private List<Animator> animatorBtns;
    [SerializeField]
    private List<GameObject> tabUpgrades;
    [SerializeField]
    private TMP_Text txtTimeSpeed;
    [SerializeField]
    private MenuSettingGame menuSettingGame;
    [SerializeField]
    private Animator animGemReward;

    private const string nameCloseBtnAnim = "Close";
    private const string nameOpenBtnAnim = "Open";
    public bool isActiveTab = true;

    private int indexTab;
    public int IndexTab 
    {  
        get { return indexTab; }
        set {
            if (indexTab != value)
            {
                indexTab = value;
                InActiveTab();
                animatorBtns[indexTab].Play(nameOpenBtnAnim);
                tabUpgrades[indexTab].SetActive(true);
            }
        }
    }

    public List<Animator> AnimatorBtns { get => animatorBtns;  }
    public List<GameObject> TabUpgrades { get => tabUpgrades; }

    public static ScreenGamePlay instance;
    private float FiveMinutesOpenGemReward = 300;
    private bool isOpenGemReward = false;
    [SerializeField]
    private GameObject popupTurotialSpeed;
    public GameObject tutorialPressAndHold;

    [SerializeField]
    private TMP_Text txt_timeBonusGold;

    private void Awake()
    {
        instance = this;
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.ScreenGamePlay;
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnAddTimeBonusGold, (o) => {
            StartCoroutine(I_CountTimeBonus());
        });
        InitDefault();
        OnClickBtnTab(0);
    }

    private void OnEnable()
    {
        OnAppear();
        StartCoroutine(I_CountTimeBonus());
    }

    private IEnumerator I_CountTimeBonus()
    {
        while (GameDatas.timeTargetBonusGold > DateTime.Now)
        {
            txt_timeBonusGold.text = (GameDatas.timeTargetBonusGold - DateTime.Now).Display();
            yield return new WaitForSeconds(1f);
        }
        txt_timeBonusGold.text = LanguageManager.GetText("inactive");

    }

    private float timer = 0f;

    private void Update()
    {
        CheckOpenBonusGem();
    }

    private void CheckOpenBonusGem()
    {
        if (isOpenGemReward) return;
        timer += Time.deltaTime;
        if (timer >= FiveMinutesOpenGemReward)
        {
            animGemReward.Play(nameOpenBtnAnim);
            isOpenGemReward = true;
        }
    }

    public void BonusGemReward()
    {
        WatchAds.WatchRewardedVideo(() => {
            GameDatas.Gem += 5;
            GPm.GemInGame += 5;
            animGemReward.Play(nameCloseBtnAnim);
            isOpenGemReward = false;
            timer = 0f;
        }, "Bonus Gem Reward");
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
    }

    public void BtnSettings()
    {
        menuSettingGame.gameObject.SetActive(true);
    }

    private void InitDefault()
    {
        if(!GameDatas.IsFirstTimeGoHome)
        {
            GameDatas.timeGameMax = ConfigManager.instance.labCtrl.LapManager.GetSingleSubjectById
            (IdSubjectType.GAME_SPEED).GetCurrentProperty();

            TimeGame.TimeGameplay = GameDatas.TimeGamePlaySpeed;
        }
        SetTimeSpeed();
        InActiveTab(1);
        tabUpgrades[0].SetActive(true);
    }

    private void SetTimeSpeed()
    {
        txtTimeSpeed.text = "x" + TimeGame.TimeGameplay;
    }

    private void InActiveTab(int indexFirst = 0)
    {
        for(int i = indexFirst; i < tabUpgrades.Count; i++)
        {
            animatorBtns[i].Play(nameCloseBtnAnim);
            tabUpgrades[i].SetActive(false);
        }
    }

    [SerializeField]
    private GameObject panelUW;
    public void OpenPanelUW()
    {
        panelUW.SetActive(!panelUW.activeSelf);
    }

    private bool isTabClicked = false;
    public void OnClickBtnTab(int indexTab)
    {
        if (isTabClicked) return;

        isTabClicked = true;
        IndexTab = indexTab;
        panelUW.SetActive(false);
        //ActiveCinematicTab(indexTab);

        if (containerUpgrader.sizeDelta.y < 90f)
        {
            animatorBtns[indexTab].Play(nameOpenBtnAnim);
            OnOpenTabBottom();
        } else
        {
            if (indexTab == lastClickedButton)
            {
                containerUpgrader.DOSizeDelta(new Vector2(containerUpgrader.sizeDelta.x, 80f), 0.5f)
                                      .SetEase(Ease.Linear);
                buttonSpeed.DOAnchorPosY(-840f, 0.7f)
                    .SetEase(Ease.Linear);
                buttonBonusGem.DOAnchorPosY(-840f, 0.7f)
                    .SetEase(Ease.Linear);

                CameraController.instance.ActiveCamCinematic();

                isActiveTab = false;
                animatorBtns[indexTab].Play(nameCloseBtnAnim);
            }
            else
            {
                animatorBtns[indexTab].Play(nameOpenBtnAnim);
                OnOpenTabBottom();
            }
        }

        lastClickedButton = indexTab;

        Invoke(nameof(ResetTabClick), 0.2f);
    }
    private void ResetTabClick()
    {
        isTabClicked = false;
    }

    public int curIndex = 0;
    private int lastClickedButton = -1;

/*    private void ActiveCinematicTab(int index)
    {
        if (isActiveTab && tabUpgrades[index].activeSelf)
        {
            isActiveTab = false;
            animatorBtns[index].Play(nameCloseBtnAnim);
        }
        else
        {
            animatorBtns[index].Play(nameOpenBtnAnim);
            OnOpenTabBottom();
        }
    }*/

    public void OnOpenTabBottom()
    {
        isActiveTab = true;

        int targetHeight = Mathf.Clamp((int)(Screen.height * 0.407f), 800, 880);

        containerUpgrader.DOSizeDelta(new Vector2(containerUpgrader.sizeDelta.x, targetHeight), 0.5f)
                  .SetEase(Ease.Linear);
        buttonSpeed.DOAnchorPosY(0f, 0.5f)
            .SetEase(Ease.Linear);
        buttonBonusGem.DOAnchorPosY(0f, 0.5f)
            .SetEase(Ease.Linear);

        CameraController.instance.InActiveCamCinematic();
    }

    public void BtnSpeed()
    {
        if (TimeGame.TimeGameplay < GameDatas.timeGameMax) TimeGame.TimeGameplay += 0.5f;
        else TimeGame.TimeGameplay = 1f;
        SetTimeSpeed();

        if(!GameDatas.isTutSpeed && TimeGame.TimeGameplay == 2)
        {
            StartCoroutine(ShowTutSpeed());
        }
    }

    private void OnDisable()
    {
        GameDatas.TimeGamePlaySpeed = TimeGame.TimeGameplay;
    }

    private IEnumerator ShowTutSpeed()
    {
        popupTurotialSpeed.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        popupTurotialSpeed.SetActive(false);
        GameDatas.isTutSpeed = true;
    }

    public void PlayCloseAnimation()
    {
        PlayAnimator(animContentPanelTop, $"{nameCloseBtnAnim}Top");
        PlayAnimator(animContentPanelBottom, $"{nameCloseBtnAnim}Bottom");
        //SetGameObjectActive(buttonSpeed, false);
        if (CameraController.instance == null) return;
        if (TutorialManager.instance.isActiveTutorial) return;
        CallCameraAction(CameraController.instance.ActiveCamCinematic);
    }

    public void PlayOpenAnimation()
    {
        PlayAnimator(animContentPanelTop, $"{nameOpenBtnAnim}Top");
        PlayAnimator(animContentPanelBottom, $"{nameOpenBtnAnim}Bottom");
        //SetGameObjectActive(buttonSpeed, true);
        if (CameraController.instance == null) return;
        if (TutorialManager.instance.isActiveTutorial) return;
        CallCameraAction(CameraController.instance.InActiveCamCinematic);
    }

    private void PlayAnimator(Animator animator, string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }

/*    private void SetGameObjectActive(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }*/

    private void CallCameraAction(System.Action cameraAction)
    {
        cameraAction?.Invoke();
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

    public override void Close()
    {
        base.Close();
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}