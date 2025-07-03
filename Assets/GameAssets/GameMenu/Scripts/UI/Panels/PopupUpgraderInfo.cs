using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupUpgraderInfo : UIPanel, IBoard
{
    [SerializeField]
    private TMP_Text txtNameUpgrader;
    [SerializeField]
    private TMP_Text txtDesc;
    [SerializeField]
    private TMP_Text txtCurLevel;
    [SerializeField]
    private TMP_Text txtMaxLevel;
    public bool isUpgraderInGame;

    public static PopupUpgraderInfo Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        OnAppear();
    }

    public override void Close()
    {
        TimeGame.Pause = false;
        base.Close();
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupUpgraderInfo;
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

    public void SetUp(SO_UpgradeInforData data)
    {
        TimeGame.Pause = true;
        txtNameUpgrader.text = LanguageManager.GetText(data.upgraderID.ToString());
        txtDesc.text = LanguageManager.GetText(data.upgraderID.ToString());

        int indexLevel = GameDatas.GetLevelUpgraderInforTower(data.upgraderID);
        if (isUpgraderInGame)
        {
            int curLevel = Mathf.Min(indexLevel + 
                Cfg.upgraderCtrl.GetLevelUpgraderIngame(data.upgraderID), data.maxLevel);
            txtCurLevel.text = curLevel.ToString();
        }
        else
        {
            txtCurLevel.text = indexLevel.ToString();
        }
        txtMaxLevel.text = data.maxLevel.ToString();
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
