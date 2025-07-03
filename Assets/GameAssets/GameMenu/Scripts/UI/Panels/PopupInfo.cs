using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfo : UIPanel, IBoard
{
    [SerializeField] List<DataProfile> contentPlayer;
    [SerializeField] List<DataProfile> contentGameplay;

    [SerializeField] List<InfoElement> infoElementPlayer;
    [SerializeField] List<InfoElement> infoElementGameplay;
    [SerializeField] TMP_Text txt_name;

    [SerializeField] Image img_avatar;

    [SerializeField] TMP_Text txtHighWave;
    [SerializeField] TMP_Text txtHighWaveInWorld;
    [SerializeField] TMP_Text txtHighRank;
    [SerializeField] TMP_Text txtGold;

    private void Start()
    {
        img_avatar.sprite = AvatarSource.instance.dataAvatars[GameDatas.id_avatar].sprite;
        EventDispatcher.AddEvent(EventID.OnChangedAvatar, (o) =>
        {
            img_avatar.sprite = AvatarSource.instance.dataAvatars[GameDatas.id_avatar].sprite;
        });
        EventDispatcher.AddEvent(EventID.OnChangedName, (obj) => {
            txt_name.text = GameDatas.user_name;
        });
    }

    private void OnEnable()
    {
        OnAppear();
        txt_name.text = GameDatas.user_name;
        SetUp();
        TopUI_Currency.instance.gameObject.SetActive(false);
    }

    public override void Close()
    {
        base.Close();
        TopUI_Currency.instance.gameObject.SetActive(true);
    }

    public void SetUp()
    {
        for (int i = 0; i < infoElementPlayer.Count; i++)
        {
            infoElementPlayer[i].Setup(contentPlayer[i]);
        }
        for (int i = 0; i < infoElementGameplay.Count; i++)
        {
            infoElementGameplay[i].Setup(contentGameplay[i]);
        }

        txtHighWaveInWorld.text = LanguageManager.GetText("higest_wave_x_in_world_x") + " " + (GameDatas.GetHighestWorld() + 1);
        txtHighWave.text = GameDatas.
            GetHighestWaveInWorld(GameDatas.GetHighestWorld()).ToString();
        txtHighRank.text = LanguageManager.GetText(((TypeRank)GameDatas.GetHighestRank()).ToString());
        txtGold.text = Extensions.FormatNumber(GameDatas.Gold);
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupInfo;
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

[Serializable]
public class DataProfile
{
    public Sprite icon;
    public IDInfo id;
    public string title => LanguageManager.GetText(id.ToString());
    public string info
    {
        get
        {
            if (id == IDInfo.StartDate)
                return GameDatas.StartDate.ToString("dd/MM/yyyy");
            if (id == IDInfo.HighestPlayCount)
            {
                var max = 0;
                for (int i = 0; i < 5; i++)
                {
                    var w = GameDatas.GetHighestWaveInWorld(i);
                    if (w > max)
                    {
                        max = w;
                    }
                }
                return max.ToString();
            }
            if (id == IDInfo.WorldUnlocked)
            {
                return (GameDatas.GetHighestWorld() + 1).ToString();
            }
            if (id == IDInfo.NumberofMonstersDefeated)
                return GameDatas.countEnemyDestroy.ToString();
            if (id == IDInfo.NumberofUpgradesMade)
                return GameDatas.countUpgraderTime.ToString();
            if (id == IDInfo.NumberofCardsReceived)
            {
                var countAll = 0;
                var list_card = ConfigManager.instance.cardCtrl.SO_CardManager.GetAllCard();
                for (int i = 0; i < list_card.Count; i++)
                {
                    countAll += list_card[i].amountCard;
                }
                return countAll.ToString();
            }
            return GameDatas.GetDataProfile(id).ToString();
        }
    }
}
public enum IDInfo
{
    StartDate,//
    HighestPlayCount,//
    WorldUnlocked,//
    BattleTime,//
    HighestBattleGoldEarned,//
    TotalCoinsEarned,//
    TotalGoldEarned,//
    TotalGemsEarned,//
    NumberofMonstersDefeated,//
    NumberofBossesDefeated,//
    NumberofUpgradesMade,//
    NumberofDailyGiftsReceived,//
    NumberofEventsParticipated,
    NumberofCardsReceived,
}
