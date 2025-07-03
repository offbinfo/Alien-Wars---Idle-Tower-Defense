using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUnlockNewWorld : UIPanel, IBoard
{

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupUnlockNewWorld;
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

    private void Init()
    {
        TimeGame.Pause = true;
    }

    public override void Close()
    {
        base.Close();
        TimeGame.Pause = false;
    }

    public void BtnEndRound()
    {

    }

    public void BtnContinue()
    {
        Close();
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
