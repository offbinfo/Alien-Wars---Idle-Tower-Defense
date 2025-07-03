using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSettingGame : GameMonoBehaviour
{
    [SerializeField]
    private GameObject btnMenuSetting;
    private const string nameCloseAnim = "Close";
    private const string nameOpenAnim = "Open";
    [SerializeField] private Animator anim;

    [SerializeField] TMP_Text txt_timeBonusGold;

    [SerializeField]
    private GameObject tabCards;
    [SerializeField]
    private GameObject tabLabs;
    [SerializeField]
    private GameObject tabShop;

    [SerializeField]
    private GameObject parentTab;

    [SerializeField]
    private GameObject btnTabCards;
    [SerializeField]
    private GameObject btnTabLabs;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnAddTimeBonusGold, (o) => {
            StartCoroutine(I_CountTimeBonus());
        });
    }

    private void OnEnable()
    {
        btnTabCards.SetActive(GameDatas.IsUnlockFeatureCard);
        btnTabLabs.SetActive(GameDatas.isUnlockLab);

        anim.Play(nameOpenAnim);
        btnMenuSetting.gameObject.SetActive(false);
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

    public void OpenSetting()
    {
        Gui.OpenBoard(UiPanelType.PopupSetting);
    }

    public void OpenShop()
    {
        OpenTab();
        tabShop.SetActive(true);
    }

    public void BonusReward()
    {
        if (GameDatas.timeTargetBonusGold > DateTime.Now)
            return;
        TimeGame.Pause = true;
        Gui.OpenBoard(UiPanelType.PopupBonusGold);
    }

    public void OpenLab()
    {
        OpenTab();
        tabLabs.SetActive(true);
    }

    public void OpenCard()
    {
        OpenTab();
        tabCards.SetActive(true);
    }

    public void OpenChallenge()
    {
        Gui.OpenBoard(UiPanelType.PopupEvent);
        TimeGame.Pause = true;
    }

    public void CloseTab()
    {
        //Gm.isPaused = false;
        TimeGame.Pause = false;
        tabCards.SetActive(false);
        tabLabs.SetActive(false);
        tabShop.SetActive(false);
        parentTab.SetActive(false);
    }

    public void OpenTab()
    {
        //Gm.isPaused = true;
        TimeGame.Pause = true;
        parentTab.SetActive(true);
    }

    public void EndWave()
    {
        Gui.OpenBoard(UiPanelType.PopupSetting);
    }

    public void Close()
    {
        anim.Play(nameCloseAnim);
    }

    public void ActiveBtnMenu()
    {
        btnMenuSetting.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
