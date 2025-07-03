using System.Collections;
using UnityEngine;
using language;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] Dialog dialog;
    [SerializeField] Hand hand;
    [SerializeField] GameObject obj_Mask_tutorial;
    [SerializeField] RectTransform mask;

    public bool isActiveTutorial = false;
    #region Mask
    private void SetMask(Vector3 posTarget)
    {
        var canvas = GetComponentInParent<Canvas>();
        mask.anchoredPosition = Extensions.ConvertWorldPointToCanvas(canvas, posTarget, null);
    }
    private void SetActiveObjMask(bool isActive)
    {
        obj_Mask_tutorial.SetActive(isActive);
    }
    private void SetActiveMask(bool isActive)
    {
        mask.gameObject.SetActive(isActive);
    }
    #endregion

    #region Tutorial Play Demo
    //lần đầu vào game hướng dẫn ng chơi nâng cấp
    //sau khi con creep đầu tiên bị tiêu diệt
    public void StartTutorialPlayDemo()
    {
        GameAnalytics.LogEvent_PlayTutorial(1);
        StartCoroutine(Tutorial_PlayDemo());
    }

    private IEnumerator Tutorial_PlayDemo()
    {
        isActiveTutorial = true;
        yield return Yielders.Get(2f);
        TimeGame.PauseTutorial = true;
        yield return dialog.Action(LanguageManager.GetText("tutorial_playdemo"), () => Input.GetMouseButtonDown(0));
        TimeGame.PauseTutorial = false;
        isActiveTutorial = false;
    }
    public void StartTutorialPlayDemo1()
    {
        StartCoroutine(Tutorial_PlayDemo1());
    }

    private IEnumerator Tutorial_PlayDemo1()
    {
        isActiveTutorial = true;
        TimeGame.PauseTutorial = true;
        yield return dialog.Action(LanguageManager.GetText("tutorial_playdemo_1"), () => Input.GetMouseButtonDown(0));
        TimeGame.PauseTutorial = false;
        isActiveTutorial = false;
    }
    public void StartTutorialPlayDemo2()
    {
        StartCoroutine(Tutorial_PlayDemo2());
    }

    private IEnumerator Tutorial_PlayDemo2()
    {
        isActiveTutorial = true;
        TimeGame.PauseTutorial = true;
        yield return dialog.Action(LanguageManager.GetText("tutorial_playdemo_2"), () => Input.GetMouseButtonDown(0));
        GameAnalytics.LogEvent_PlayTutorial(2);
        TimeGame.PauseTutorial = false;
        GameDatas.IsTut_PlayDemo = false;
        BlockInputUI.instance.blockInput(true);
        EventDispatcher.PostEvent(EventID.OnTutorialPlayDemoEnd, null);
        LoadSceneManager.instance.LoadScene(TypeScene.GamePlay);
        isActiveTutorial = false;
    }

    #endregion

    #region Tutorial 0 Attack
    //khi người chơi làm xong tut và chơi lại game từ đầu
    public void StartTutorial_buildBase()
    {
        GameAnalytics.LogEvent_PlayTutorial(3);
        StartCoroutine(Tutorial_buildBase());
    }
    private IEnumerator Tutorial_buildBase()
    {
        isActiveTutorial = true;
        yield return new WaitForSeconds(2f);
        TimeGame.PauseTutorial = true;
        SetActiveObjMask(true);
        SetActiveMask(false);
        yield return dialog.Action(LanguageManager.GetText("tutorial_play"), () => Input.GetMouseButtonDown(0), true, false);
        yield return null;
        SetActiveMask(true);
        SetMask(TowerCtrl.instance.transform.position);
        yield return dialog.Action(LanguageManager.GetText("tutorial_play1"), () => Input.GetMouseButtonDown(0), false, true);
        TimeGame.PauseTutorial = false;
        SetActiveObjMask(false);
        isActiveTutorial = false;
    }
    public void StartTutorial_buildBase1()
    {
        StartCoroutine(Tutorial_buildBase1());
    }
    //upgrade ingame tut
    private IEnumerator Tutorial_buildBase1()
    {
        isActiveTutorial = true;
        //kiểm tra xem silver có đủ để mua attack không?
        //nếu không thì cho nó silver để đủ mua 
        var price = Cfg.upgraderCtrl.GetData(UpgraderID.attack_damage).
            GetPriceItemUpgradeInGame(Cfg.upgraderCtrl.GetLevelUpgraderIngame(UpgraderID.attack_damage) + 1);
        var silver = GPm.SliverInGame;
        if (price > silver)
            GPm.SliverInGame = price;


        GameObject buttonAttack = ScreenGamePlay.instance.AnimatorBtns[0].gameObject;
        GameObject buttonUpgradeAttack = ScreenGamePlay.instance.TabUpgrades[0].GetComponent<TabUpgradeView>().listUpgraderUI_ref[0].obj_btnUpgrader_ref;
        GameObject objPanelAttack = ScreenGamePlay.instance.TabUpgrades[0].gameObject;
        //khóa input

        TimeGame.PauseTutorial = true;
        //UpgradeUI.instance.OpenPanelAttack();
        yield return dialog.Action(LanguageManager.GetText("tutorial_play2"), () => Input.GetMouseButtonDown(0), true, false);
        yield return null;
        yield return dialog.Action(LanguageManager.GetText("tutorial_play3"), () => Input.GetMouseButtonDown(0), false, true);


        yield return null;
        if (!ScreenGamePlay.instance.isActiveTab)
        {
            TimeGame.PauseTutorial = false;
            ScreenGamePlay.instance.OnOpenTabBottom();
            yield return Yielders.Get(1f);
            TimeGame.PauseTutorial = true;
        }
        if (ScreenGamePlay.instance.IndexTab != 0)
        {
            ScreenGamePlay.instance.OnClickBtnTab(0);
        }
        //yield return hand.Action(buttonAttack, () => objPanelAttack.activeSelf);

        var currentStatAttack = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage);
        yield return hand.Action(buttonUpgradeAttack, () => currentStatAttack != Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage));

        GameAnalytics.LogEvent_PlayTutorial(4);
        TimeGame.PauseTutorial = false;
        isActiveTutorial = false;
        //mở input
    }

    public void StartTutorial_buildBase2()
    {
        StartCoroutine(Tutorial_buildBase2());
    }
    //upgrade ingame tut
    private IEnumerator Tutorial_buildBase2()
    {
        isActiveTutorial = true;

        yield return null;
        ScreenGamePlay.instance.tutorialPressAndHold.SetActive(true);
        TimeGame.PauseTutorial = true;
        ControlAllButton.DeactiveAllButton();

        yield return new WaitUntil(() => TouchHandCtrl.instance.isTouchAttackTutorial);
        ScreenGamePlay.instance.tutorialPressAndHold.SetActive(false);

        GameAnalytics.LogEvent_PlayTutorial(4);
        TimeGame.PauseTutorial = false;
        isActiveTutorial = false;
        ControlAllButton.ActiveAllButton();
        //mở input
    }
    #endregion

    #region Tutorial up speed game
    //nếu lần thứ 2 vào game mà chưa ấn vào nút spd thì hiện tut này
    public void StartTutorial_SpdGame()
    {
        StartCoroutine(Tutorial_SpdGame());
    }
    private IEnumerator Tutorial_SpdGame()
    {
        //khóa input
        yield return dialog.Action(LanguageManager.GetText("tutorial_spdgame"), () => Input.GetMouseButtonDown(0));
        //mở input
    }
    #endregion


    #region Tutorial Upgrade Gold Forever
    //ra home lần đầu tiên sẽ hiển thị tut này 
    //tặng cho ng chơi 20p idle reward để claim và nâng cấp luôn
    public void StartTutorial_UpgradeGoldForever()
    {
        GameAnalytics.LogEvent_PlayTutorial(5);
        StartCoroutine(Tutorial_UpgradeGoldForever());
    }
    private IEnumerator Tutorial_UpgradeGoldForever()
    {
        yield return null;
        GameObject idleRewardPanelPos = RewardTimeHome.instance.gameObject;
        GameObject buttonUpgradePanel = TabCtrlBottom.instance.buttons_tab[0].gameObject;

        GameObject buttonBattle = TabCtrlBottom.instance.buttons_tab[2].gameObject;
        GameObject objUpgrader = TabUpgraderManager.instance.gameObject;
        //GameObject objBattle = TabCtrlBottom.instance.tabs[2].gameObject;

        GameDatas.SecondsAccumulate = 20 * 60; // tặng cho ng chơi 20 phút

        yield return dialog.Action(LanguageManager.GetText("tutorial_idlereward"), () => Input.GetMouseButtonDown(0));
        var goldCurrent = GameDatas.Gold;
        yield return hand.Action(idleRewardPanelPos, () => GameDatas.Gold != goldCurrent);

        yield return dialog.Action(LanguageManager.GetText("tutorial_upgradegoldforever"), () => Input.GetMouseButtonDown(0));
        yield return hand.Action(buttonUpgradePanel, () => TabCtrlBottom.instance.IndexTab == 0);
        yield return null;

        yield return new WaitUntil(() => TabCtrlBottom.instance.IndexTab == 0);

        yield return Yielders.Get(0.5f);
        //nếu không thì cho nó gold để đủ mua 
        int price = 40;
        var gold = GameDatas.Gold;
        if (price > gold)
            GameDatas.Gold = price;

        GameObject buttonUpgradeDamage = TabUpgraderManager.instance.TabUpgrades[0].GetComponent<TabUpgradeFactory>().ElementUpgraders[0].obj_btnUpgrader_ref;
        var leveldmg = GameDatas.GetLevelUpgraderInforTower(UpgraderID.attack_damage);
        yield return hand.Action(buttonUpgradeDamage, () => GameDatas.GetLevelUpgraderInforTower(UpgraderID.attack_damage) != leveldmg);
        yield return dialog.Action(LanguageManager.GetText("tutorial_upgradegoldforever1"), () => Input.GetMouseButtonDown(0), true, false);
        yield return null;
        yield return dialog.Action(LanguageManager.GetText("tutorial_upgradegoldforever2"), () => Input.GetMouseButtonDown(0), false, false);
        yield return null;
        yield return dialog.Action(LanguageManager.GetText("tutorial_upgradegoldforever3"), () => Input.GetMouseButtonDown(0), false, true);
        yield return null;
        yield return hand.Action(buttonBattle, () => TabCtrlBottom.instance.IndexTab == 2);
        GameAnalytics.LogEvent_PlayTutorial(6);
    }
    #endregion

    #region Tutorial Lab
    public void StartTutorialLab()
    {
        StartCoroutine(I_TutorialLab());
    }
    private IEnumerator I_TutorialLab()
    {
        yield return dialog.Action(LanguageManager.GetText("lab_tut_0"), () => Input.GetMouseButtonDown(0));
        yield return null;
        GameObject btn_Lab = TabCtrlBottom.instance.buttons_tab[3].gameObject;
        //GameObject lab = TabCtrlBottom.instance.tabs[3].gameObject;
        yield return hand.Action(btn_Lab, () => TabCtrlBottom.instance.IndexTab == 3);

        yield return null;

        var scrollrect = LabCtrl.instance.ScrollRect;

        yield return new WaitUntil(() => TabCtrlBottom.instance.IndexTab == 3);
        scrollrect.enabled = false;

        yield return Yielders.Get(0.5f);
        yield return hand.Action(LabCtrl.instance.Slots[0].obj_bgoffline, () => LabCtrl.instance.SubjectManager.gameObject.activeInHierarchy);
        scrollrect.enabled = true;

        yield return null;
        var srSubject = LabCtrl.instance.SubjectManager.ScrollRect;
        //srSubject.verticalNormalizedPosition = 0.55f;

        //nếu không thì cho nó gold để đủ mua 
        int price = 40;
        var gold = GameDatas.Gold;
        if (price > gold)
            GameDatas.Gold = price;

        srSubject.enabled = false;
        yield return hand.Action(LabCtrl.instance.SubjectManager.obj_AttackResearch.GetComponentInChildren<Button>().gameObject
            , () => !LabCtrl.instance.SubjectManager.gameObject.activeInHierarchy);

        srSubject.enabled = true;

        yield return dialog.Action(LanguageManager.GetText("lab_tut_1"), () => Input.GetMouseButtonDown(0), true, false);
        yield return null;
        yield return dialog.Action(LanguageManager.GetText("lab_tut_2"), () => Input.GetMouseButtonDown(0), false, false);
        yield return null;
        yield return dialog.Action(LanguageManager.GetText("lab_tut_3"), () => Input.GetMouseButtonDown(0), false, true);

    }
    #endregion
}
