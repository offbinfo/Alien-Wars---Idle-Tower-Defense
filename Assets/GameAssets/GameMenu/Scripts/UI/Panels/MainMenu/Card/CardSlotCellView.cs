using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotCellView : BaseUICellView
{

    [SerializeField]
    private GameObject lockResource;
    [SerializeField]
    private GameObject lockIAP;
    [SerializeField]
    private TMP_Text txtPrice;
    [SerializeField]
    private int indexSlot;
    [SerializeField]
    private int priceGemUnlock;
    public bool isOpenSlot;
    [SerializeField]
    private Sprite buy;
    [SerializeField]
    private Sprite unBuy;
    [SerializeField]
    private Image btnBuy;

    [SerializeField]
    private GameObject btnUnBuy;
    public bool isUnlockTech = false;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnGemChanged, OnRefresh);
        Init();
    }

    private void OnRefresh(object obj)
    {
        btnBuy.sprite = priceGemUnlock > GameDatas.Gem ? unBuy : buy;
    }

    private void OnEnable()
    {
        OnRefresh(null);
        //btnUnBuy.SetActive(priceGemUnlock > GameDatas.Gem);
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);
    }

    public void CheckUnlockCardSlot()
    {
        if(isUnlockTech)
        {
            bool isunlock = GameDatas.GetLevelSubjectLab(IdSubjectType.CARD_PRESET) == 2;
            if (isunlock)
            {
                lockResource.SetActive(!isunlock);
                isOpenSlot = isunlock;
                gameObject.SetActive(isOpenSlot);
            } else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            lockResource.SetActive(!GameDatas.IsUnlockSlotCard(indexSlot));
            isOpenSlot = GameDatas.IsUnlockSlotCard(indexSlot);
            CheckActiveCardNext();
        }
    }

    private void CheckActiveCardNext()
    {
        int index = GameDatas.IndexSlotCardUnlockByGold + 1;
        gameObject.SetActive(index >= indexSlot);
    }

    private void Init()
    {
        txtPrice.text = priceGemUnlock.ToString();
        CheckUnlockCardSlot();
    }

    public void OnClickUnlockUsingIAP()
    {

    }

    public void OnClickUnlockUsingResource()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceGemUnlock, OnBuySuccess);
    }

    private void OnBuySuccess(bool success)
    {
        if (success)
        {
            GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.UNLOCK_1_MORE_SLOT_CARD, 1);
            GameDatas.IndexSlotCardUnlockByGold += 1;
            GameDatas.UnlockSlotCard(indexSlot);
            EventDispatcher.PostEvent(EventID.OnRefreshUnlockSlotCard, 0);
        }
    }
}
