using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupQuit : UIPanel, IBoard
{

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupQuit;
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

    public void BtnQuit()
    {

    }

    public void BtnContinue()
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
