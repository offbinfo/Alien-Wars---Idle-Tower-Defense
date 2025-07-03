using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PopupCardNew : UIPanel, IBoard
{
    public List<UnlockCardLevel> levelUnlockCards;

    [SerializeField] PanelListCard obj_ListCard;
    //[SerializeField] PanelUnlockNewCard obj_newCard;
    [SerializeField] GameObject obj_BtnRollX1;
    [SerializeField] GameObject obj_BtnRollX5;
    List<Card_SO> cardsUnlockList = new List<Card_SO>();
    //public List<Transform> ballRolls = new();
    public Transform panelBall;
    [SerializeField]
    private BallUnlock ballUnlock;

    private int priceRollX1 = 25;
    private int priceRollX5 = 115;

    [SerializeField]
    private GameObject btnUnRollX1;
    [SerializeField]
    private GameObject btnUnRollX5;

    [SerializeField]
    private GameObject panelIsRollingx1;
    [SerializeField]
    private GameObject panelIsRollingx5;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupCardNew;
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnLevelUnlockCardChanged, GetListCard);
    }

    private void OnEnable()
    {
        OnAppear();
        GetListCard(null);
        CheckBtnUnRoll();
    }

    private void CheckBtnUnRoll()
    {
        btnUnRollX1.SetActive(priceRollX1 > GameDatas.Gem);
        btnUnRollX5.SetActive(priceRollX5 > GameDatas.Gem);
    }

    private void OnDisable()
    {
        panelBall.gameObject.SetActive(false);
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
        DebugCustom.LogColor("GameDatas.countSpinCard " + GameDatas.countSpinCard);
        panelBall.gameObject.SetActive(true);
    }

    private Card_SO GetCard(CardID id)
    {
        return Cfg.cardCtrl.GetCard(id);
    }
    private void GetListCard(object o)
    {
        cardsUnlockList = new List<Card_SO>();
        var levelUnlockCard = GameDatas.levelCardUnlock;

        for (int i = 0; i < levelUnlockCard + 1; i++)
        {
            var listcard = levelUnlockCards[i].list_card;
            for (int x = 0; x < listcard.Count; x++)
            {
                cardsUnlockList.Add(GetCard(listcard[x]));
            }
        }

        //lọc đi tất cả card đã full
        cardsUnlockList.RemoveAll(x => x.isFullCard);
        obj_BtnRollX1.SetActive(cardsUnlockList.Count > 0);
        obj_BtnRollX5.SetActive(cardsUnlockList.Count > 0);
    }
    #region BUTTON
    public void OnClickbtnRollX1()
    {
        DebugCustom.LogColor("OnClickbtnRollX1 "+ ballUnlock.isRoll);
        if (ballUnlock.isRoll) return;
        if (GameDatas.Gem < priceRollX1)
            return;
        else
            GameDatas.Gem -= priceRollX1;

        ActivePanelRolling(true);
        var random = UnityEngine.Random.Range(0, cardsUnlockList.Count);
        var data = cardsUnlockList[random];
        ballUnlock.RollX1(data, RollEnd);

        if (data.isUnlock)
            data.amountCard += 1;
        else
            GameDatas.UnlockCard(data.id);

        GameDatas.countSpinCard += 1;
        QuestEventManager.CardPurchased(1);
        EventChallengeListenerManager.CardPurchased(1);
        GetListCard(null);
        CheckBtnUnRoll();
    }

    private void RollEnd()
    {
        ActivePanelRolling(false);
    }

    private void ActivePanelRolling(bool isActive)
    {
        panelIsRollingx1.SetActive(isActive);
        panelIsRollingx5.SetActive(isActive);
    }

    public void OnClickbtnRollX3()
    {
        DebugCustom.LogColor("OnClickbtnRollX3 "+ ballUnlock.isRoll);
        if (ballUnlock.isRoll) return;
        if (GameDatas.Gem < priceRollX5)
            return;
        else
            GameDatas.Gem -= priceRollX5;

        ActivePanelRolling(true);
        //x5
        cardsUnlockList = cardsUnlockList.OrderBy(_ => Guid.NewGuid()).ToList();
        var listcard = new List<Card_SO>();
        for (int i = 0; i < Mathf.Min(cardsUnlockList.Count, 5); i++)
        {
            listcard.Add(cardsUnlockList[i]);
        }
        ballUnlock.RollX5(listcard, RollEnd);

        foreach (var data in listcard)
        {
            if (data.isUnlock)
                data.amountCard += 1;
            else
                GameDatas.UnlockCard(data.id);

            GameDatas.countSpinCard += 1;
            QuestEventManager.CardPurchased(1);
            EventChallengeListenerManager.CardPurchased(1);
        }

        GetListCard(null);
        CheckBtnUnRoll();
    }
    public void OnClickBtnListCard()
    {
        if(ballUnlock.isRoll) return;
        obj_ListCard.gameObject.SetActive(true);
    }

    public void OnClickClose()
    {
        if (ballUnlock.isRoll) return;
        Close();
    }
    #endregion

    /*[SerializeField]
    private PanelListCard panelListCard;
    [SerializeField]
    private PanelUnlockNewCard panelUnlockNewCard;

    private List<Card_SO> cardsUnlockList = new();
    public List<CardID> levelUnlockCards;

    [SerializeField]
    private int priceRollX1 = 25;
    [SerializeField]
    private int priceRollX5 = 115;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupCardNew;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    private void Start()
    {
        cardsUnlockList = Cfg.cardCtrl.SO_CardManager.GetAllCard();
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

    public void BtnRollX1()
    {
        QuestEventManager.CardPurchased(1);
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceRollX1, OnBuyRollX1Success);
    }

    private void OnBuyRollX1Success(bool success)
    {
        if (success)
        {
            var random = UnityEngine.Random.Range(0, cardsUnlockList.Count);
            var data = cardsUnlockList[random];

            if (data.isUnlock)
                data.amountCard += 1;
            else
                data.isUnlock = true;

            panelUnlockNewCard.ShowCard(data);
            //GetListCard(null);
        }
    }

    private void OnBuyRollX5Success(bool success)
    {
        if (success)
        {
            cardsUnlockList = cardsUnlockList.OrderBy(_ => Guid.NewGuid()).ToList();
            var listcard = new List<Card_SO>();
            for (int i = 0; i < Mathf.Min(cardsUnlockList.Count, 5); i++)
            {
                listcard.Add(cardsUnlockList[i]);
                QuestEventManager.CardPurchased(1);
            }

            foreach (var data in listcard)
            {
                if (data.isUnlock)
                    data.amountCard += 1;
                else
                    data.isUnlock = true;
            }

            panelUnlockNewCard.Show5Card(listcard);
            //GetListCard(null);    
        }
    }

    public void BtnRollX5()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceRollX5, OnBuyRollX5Success);
    }

    public void BtnShowListCard()
    {
        ActivePanelListCard(true);
    }

    public void BtnClosePanelNewCard()
    {
        ActivePanelNewCard(false);
    }

    public void BtnCLosePanelListCard()
    {
        ActivePanelListCard(false);
    }

    private void ActivePanelNewCard(bool isActive)
    {
        panelUnlockNewCard.gameObject.SetActive(isActive);
    }

    private void ActivePanelListCard(bool isActive)
    {
        panelListCard.gameObject.SetActive(isActive);
    }*/

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

[System.Serializable]
public struct UnlockCardLevel
{
    public List<CardID> list_card;
}
