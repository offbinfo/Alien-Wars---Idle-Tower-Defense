using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupChangeRelics : UIPanel, IBoard
{
    [SerializeField]
    private SimpleScrollSnap simpleScrollSnap;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject panelInfor;
    [SerializeField]
    private GameObject panelBottom;

    [SerializeField]
    private List<GameObject> selectedTab;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupChangeRelics;
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

    public void OpenTabThemes()
    {
        scrollRect.enabled = true;
        simpleScrollSnap.GoToPanel(0);
        scrollRect.enabled = false;
        panelBottom.SetActive(true);

        selectedTab[0].gameObject.SetActive(true);
        selectedTab[1].gameObject.SetActive(false);
    }

    public void OpenTabRelics()
    {
        scrollRect.enabled = true;
        simpleScrollSnap.GoToPanel(1);
        scrollRect.enabled = false;
        panelBottom.SetActive(false);

        selectedTab[0].gameObject.SetActive(false);
        selectedTab[1].gameObject.SetActive(true);
    }

    private void Init()
    {
        selectedTab[0].gameObject.SetActive(true);
        selectedTab[1].gameObject.SetActive(false);
        simpleScrollSnap.GoToPanel(0);
        panelBottom.SetActive(true);
        scrollRect.enabled = false;
    }

    public void OpenPanelInfor()
    {
        panelInfor.SetActive(true);
    }

    private void OnDisable()
    {
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
