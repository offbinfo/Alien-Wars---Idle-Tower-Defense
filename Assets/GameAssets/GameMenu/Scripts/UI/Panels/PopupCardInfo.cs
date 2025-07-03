using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupCardInfo : UIPanel, IBoard
{
    [SerializeField]private TMP_Text txt_ID;
    [SerializeField]private TMP_Text txt_description;
    [SerializeField]private TMP_Text txt_type;
    [SerializeField]private TMP_Text txt_Collect;
    [SerializeField]private TMP_Text txt_currentStat;
    [SerializeField]private TMP_Text txt_nextStat;
    [SerializeField]private TMP_Text txt_Amount;
    [SerializeField]private TMP_Text txt_buttonEquipOrUnEquip;
    [SerializeField]private TMP_Text txt_priceGem;
    [SerializeField]private TMP_Text txt_priceGemGray;
    [SerializeField]private List<GameObject> objs_Stars;
    [SerializeField]private GameObject obj_Common;
    [SerializeField]private GameObject obj_Rare;
    [SerializeField]private GameObject obj_Epic;
    [SerializeField]private GameObject obj_Divine;
    [SerializeField]private GameObject obj_Tich_V;
    [SerializeField]private GameObject obj_buttonContainer;
    [SerializeField]private GameObject obj_txt_collect;
    [SerializeField]private GameObject obj_btnUpgradeGem;
    [SerializeField]private GameObject obj_btnUpgradeGemGray;
    [SerializeField]private Image icons;
    private Card_SO cardData;

    [SerializeField]
    private GameObject btnUnBuy;
    [SerializeField]
    private GameObject panelUpgradeFree;

    private int gemUpgrade
    {
        get
        {
            return cardData.currentlevel switch
            {
                0 => 0,
                1 => 40,
                2 => 100,
                3 => 160,
                4 => 240,
                5 => 600,
                6 => 600,
                _ => 0,
            };
        }
    }

    public static PopupCardInfo Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupCardInfo;
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
        if (cardData == null) return;
        CheckGemUpgrader();
    }

    private void CheckGemUpgrader()
    {
        btnUnBuy.SetActive(gemUpgrade > GameDatas.Gem);
        panelUpgradeFree.SetActive(gemUpgrade == 0);
    }

    public void InitData(Card_SO cardData)
    {
        this.cardData = cardData;
        icons.sprite = cardData.icon;   

        obj_Common.SetActive(cardData.type == TypeCard.COMMON);
        obj_Rare.SetActive(cardData.type == TypeCard.RARE);
        obj_Epic.SetActive(cardData.type == TypeCard.EPIC);
        obj_Divine.SetActive(cardData.type == TypeCard.DIVINE);

        txt_type.text = LanguageManager.GetText(cardData.type.ToString());

        txt_priceGem.text = gemUpgrade.ToString();
        txt_priceGemGray.text = gemUpgrade.ToString();
        //obj_btnUpgradeGem.SetActive(GameDatas.Gem >= gemUpgrade);
        //obj_btnUpgradeGemGray.SetActive(GameDatas.Gem < gemUpgrade);
        //level
        for (int i = 0; i < objs_Stars.Count; i++)
        {
            objs_Stars[i].SetActive(i < cardData.currentlevel);
        }

        var format = cardData.format switch
        {
            Format.NUMBER => "",
            Format.PERCENT => "%",
            Format.SECOND => "s",
        };
        txt_currentStat.text = string.Format("{0}{1}", cardData.GetCurrentStat(), format);
        txt_nextStat.gameObject.SetActive(cardData.currentlevel < 6);
        txt_nextStat.text = string.Format("{0} {1}{2}", "<sprite name=mui ten ngang>", cardData.GetNextStat(), format);

        txt_ID.text = LanguageManager.GetText(cardData.id + "_ID");
        txt_description.text = string.Format(LanguageManager.GetText(cardData.id + "_DES"), cardData.GetCurrentStat());
        txt_Amount.text = string.Format("{0}/5", cardData.amountCard);

        txt_Collect.text = string.Format(LanguageManager.GetText("card_info_des"), 5);

        CheckEquipOrUnEquip();

        obj_txt_collect.SetActive(cardData.amountCard < 5);
        obj_buttonContainer.SetActive(cardData.amountCard >= 5);
        CheckGemUpgrader();
    }

    public void BtnEquipOrUnEquip()
    {
        /*if (*//*Cfg.cardCtrl.currentIndexSet > GameDatas.IndexSlotCard && *//*!GameDatas.IsCardEquiped(cardData.id))
            return;*/
        if (TabCardCtrl.instance.CheckFullSlotCard() && !GameDatas.IsCardEquiped(cardData.id))
            return;

        if (GameDatas.IsCardEquiped(cardData.id))
        {
            Cfg.cardCtrl.currentIndexSet--;
            GameDatas.UnEquipedCard(cardData.id);
            EventDispatcher.PostEvent(EventID.OnUnequipcard, cardData.id);
        }
        else
        {
            Cfg.cardCtrl.currentIndexSet += 1;
            GameDatas.SetCardEquiped(cardData.id);
            EventDispatcher.PostEvent(EventID.OnEquipCard, cardData.id);

        }
        EventDispatcher.PostEvent(EventID.OnSetChanged, cardData.id);
        CheckEquipOrUnEquip();
    }

    private void CheckEquipOrUnEquip()
    {
        obj_Tich_V.SetActive(GameDatas.IsCardEquiped(cardData.id));
        txt_buttonEquipOrUnEquip.text = GameDatas.IsCardEquiped(cardData.id) ? LanguageManager.GetText("unequip") : LanguageManager.GetText("equip");
    }

    public void BtnUpgradeUsingAds()
    {
        WatchAds.WatchRewardedVideo(() =>
        {
            cardData.Upgrade();
            InitData(cardData);
        }, "UpgradeCard");
    }

    public void BtnUpgradeUsingResource()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, gemUpgrade, OnBuySuccess);
    }

    private void OnBuySuccess(bool isSuccess)
    {
        if (isSuccess)
        {
            cardData.Upgrade();
            InitData(cardData);
        }
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
