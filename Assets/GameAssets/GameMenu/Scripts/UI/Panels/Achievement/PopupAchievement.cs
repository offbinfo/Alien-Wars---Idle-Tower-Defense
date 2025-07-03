using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupAchievement : UIPanel, IBoard
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private AchievementElement achievementElement;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupAchievement;
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
    }

    private void BuildData()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(achievementElement, content);
        }
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
