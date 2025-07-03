using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabCtrlBottom : Singleton<TabCtrlBottom>
{
    [SerializeField]
    private SimpleScrollSnap simpleScrollSnap;
    [SerializeField]
    private List<GameObject> uiArmHome;
    [SerializeField]
    private List<GameObject> frameChooseTab;
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private GameObject unlockBtnCards;
    [SerializeField]
    private GameObject lockBtnCards;
    [SerializeField]
    private GameObject unlockBtnLabs;
    [SerializeField]
    private GameObject lockBtnLabs;

    public List<GameObject> tabs;
    public List<GameObject> buttons_tab;
    public int IndexTab = 2;

    public SimpleScrollSnap SimpleScrollSnap { get => simpleScrollSnap; }

    private void Start()
    {
        frameChooseTab[2].SetActive(true);
        EventDispatcher.AddEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
        OnRefeshUIHome(null);
    }

    private void OnRefeshUIHome(object obj)
    {
        unlockBtnCards.SetActive(GameDatas.IsUnlockFeatureCard);
        lockBtnCards.SetActive(!GameDatas.IsUnlockFeatureCard);

        unlockBtnLabs.SetActive(GameDatas.isUnlockLab);
        lockBtnLabs.SetActive(!GameDatas.isUnlockLab);
    }

    public void OnClickTab(int indextab)
    {
        if ((indextab == 1 && !GameDatas.IsUnlockFeatureCard) ||
        (indextab == 3 && !GameDatas.isUnlockLab))
        {
            return;
        }
        foreach (GameObject frameChoose in frameChooseTab) frameChoose.SetActive(false);

        if (!tabs[indextab].activeSelf)
        {
            tabs[indextab].SetActive(true);
        }

        ShopCtrl.instance.StopScrolling();

        scrollRect.enabled = true;
        InActiveIconArmHome(indextab == 2);
        simpleScrollSnap.GoToPanel(indextab);
        frameChooseTab[indextab].SetActive(true);
        scrollRect.enabled = false;
        IndexTab = indextab;
    }

    private void InActiveIconArmHome(bool isActive)
    {
        foreach (GameObject arm in uiArmHome) arm.SetActive(isActive);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
    }
}
