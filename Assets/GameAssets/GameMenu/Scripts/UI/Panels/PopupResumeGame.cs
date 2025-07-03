using language;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupResumeGame : UIPanel, IBoard
{

    [SerializeField]
    private Transform content;
    [SerializeField]
    private TMP_Text txtWorld;
    [SerializeField]
    private TMP_Text txtWave;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupResumeGame;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    private void Start()
    {
        BuildData();
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
        txtWorld.text = LanguageManager.GetText("world") + " "+ (GameDatas.CurrentWorld + 1);
        txtWave.text = LanguageManager.GetText("wave") + " " + GameDatas.GetWaveInResume(GameDatas.CurrentWorld);
    }

    private void BuildData()
    {
        
    }

    public void BtnEndRound()
    {
        GameDatas.ResumeWave(GameDatas.CurrentWorld,false);
        Gm.PlayGame(false);
    }

    public void BtnResumeRound()
    {
        Gm.PlayGame(true);
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
