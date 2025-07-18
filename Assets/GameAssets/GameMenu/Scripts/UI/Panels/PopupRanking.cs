using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupRanking : UIPanel, IBoard
{

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupRanking;
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
    }

    public void BtnClaimNow()
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
