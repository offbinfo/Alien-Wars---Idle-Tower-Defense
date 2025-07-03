using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PopupEvent : UIPanel, IBoard
{
    [SerializeField]
    private SimpleScrollSnap simpleScrollSnap;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject panelInfor;
    [SerializeField]
    private List<GameObject> selectedTab;
    [SerializeField]
    private List<GameObject> tabs;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupEvent;
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

    public void OpenTabMissions()
    {
        tabs[0].SetActive(true);
        tabs[1].SetActive(false);

        selectedTab[0].gameObject.SetActive(true);
        selectedTab[1].gameObject.SetActive(false);
    }

    public void OpenTabEventShop()
    {
        tabs[1].SetActive(true);
        tabs[0].SetActive(false);

        selectedTab[0].gameObject.SetActive(false);
        selectedTab[1].gameObject.SetActive(true);
    }

    private void Init()
    {
        selectedTab[0].gameObject.SetActive(true);
        selectedTab[1].gameObject.SetActive(false);
        tabs[0].SetActive(true);
        tabs[1].SetActive(false);
    }

    public void OpenPanelInfor()
    {
        panelInfor.SetActive(true);
    }

    public void ClosePanelInfor()
    {
        panelInfor.SetActive(false);
    }

    private void OnDisable()
    {
    }
    public override void Close()
    {
        base.Close();
        TimeGame.Pause = false;
        TopUI_Currency.instance.gameObject.SetActive(true);
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
