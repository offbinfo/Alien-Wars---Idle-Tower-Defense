using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupRevive : UIPanel, IBoard
{
    [SerializeField]
    private TMP_Text txtTime;
    private bool isRevived = false;
    private bool isCounting = false;

    public bool pause;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupRevive;
    }

    private void OnEnable()
    {
        OnAppear();
        Init();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();
    }

    private void Init()
    {
        isRevived = false;
        isCounting = true;
        StartCoroutine(I_Count(3));
        TimeGame.Pause = true;
    }

    public void BtnContinue()
    {
        pause = true;
        WatchAds.WatchRewardedVideo(() => {
            //revive
            GPm.RevivalTower();
            Close();

        }, () => {
            pause = false;
        }, "revive");
    }

    public override void Close()
    {
        base.Close();
        TimeGame.Pause = false;
    }

    public void OnClickClose()
    {
        Close();
        Gm.OnLoseGame(1);
    }

    IEnumerator I_Count(int secondsLeft)
    {
        while (secondsLeft > 0)
        {
            txtTime.text = secondsLeft.ToString();

            //yield return new WaitForSeconds(1f * Time.timeScale);
            var time = Time.unscaledTime + 1;
            yield return new WaitUntil(() => Time.unscaledTime >= time && !pause);
            secondsLeft--;
        }
        txtTime.text = "0";
        Close();
        Gm.OnLoseGame(1);
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
