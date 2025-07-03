using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldPanelHome : MonoBehaviour
{
    [SerializeField]
    private SimpleScrollSnap simpleScrollSnap;
    private int indexWorld;
    [SerializeField]
    private TMP_Text txtWorld;
    [SerializeField]
    private InforBannerHome inforBannerHome;

    private void Awake()
    {
        InitFirstInGame();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
        OnRefeshUIHome(null);
    }

    private void OnRefeshUIHome(object obj)
    {
        InitDefaultWorld();
    }

    private void InitFirstInGame()
    {
        GameDatas.CurrentWorld = GameDatas.GetHighestWorld();

        SetData();
    }

    private void SetData()
    {
        indexWorld = GameDatas.CurrentWorld;
        simpleScrollSnap.StartingPanel = indexWorld;
        ChangeTxtWorld();
        simpleScrollSnap.GoToPanel(indexWorld);
    }

    private void InitDefaultWorld()
    {
        SetData();
    }

    private void ChangeTxtWorld()
    {
        txtWorld.text = LanguageManager.GetText("world")+" " + (GameDatas.CurrentWorld + 1);
        inforBannerHome.InitData();
    }

    public void Previous()
    {
        if (indexWorld > 0) 
            indexWorld--;
        simpleScrollSnap.GoToPanel(indexWorld);
        GameDatas.CurrentWorld = indexWorld;
        ChangeTxtWorld();
    }

    public void Next()
    {
        if (indexWorld < GameDatas.GetHighestWorld())
            indexWorld++;
        simpleScrollSnap.GoToPanel(indexWorld);
        GameDatas.CurrentWorld = indexWorld;
        ChangeTxtWorld();
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshUIHome, OnRefeshUIHome);
    }

}
