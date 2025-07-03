using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabCardCtrl : GameMonoBehaviour
{

    [SerializeField]
    private CardSlotCellView[] slotCards = new CardSlotCellView[15];
    [SerializeField]
    private Transform parentCard;
    [SerializeField]
    private Transform contentSet;
    [SerializeField]
    private CardElement cardElement;
    List<CardElement> cards_Iventory = new List<CardElement>();
    List<CardElement> cards_Set = new List<CardElement>();

    private List<Card_SO> cards = new();
    private bool isReloadSet = false;

    public ScrollRect scrollRect;

    public static TabCardCtrl instance;

    private void Awake()
    {
        cards = Cfg.cardCtrl.SO_CardManager.GetAllCard();
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        ScrollToTop();
    }

    /*    public bool CheckFullSlotCard()
        {
            int indexCardEquip = 0;
            foreach (var card in cards_Iventory)
            {
                if (GameDatas.IsCardEquiped(card.cardID))
                {
                    indexCardEquip += 1;
                }
            }
            if (indexCardEquip >= GameDatas.IndexSlotCard + 1)
            {
                return true;
            }
            return false;
        }*/

    public bool CheckFullSlotCard()
    {
        int totalSlot = 0;
        foreach(CardSlotCellView slot in slotCards)
        {
            if(slot.isOpenSlot)
            {
                totalSlot++;
            }
        }
        DebugCustom.LogColor("totalSlot " + totalSlot);
        return cards_Iventory.Count(card => GameDatas.IsCardEquiped(card.cardID)) > (totalSlot - 1)/*GameDatas.IndexSlotCard*/;
    }

    private void Start()
    {
        isReloadSet = false;
        InitEvent();
        ShowIventory();
        ShowSet(null);

        DebugCustom.LogColor("GameDatas.IndexSlotCard " + GameDatas.IndexSlotCard);
    }

    public void ScrollToTop()
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    private void Update()
    {
        if (!isReloadSet)
        {
            ShowSet(null);
            isReloadSet = true;
        }
    }


    private void InitEvent()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshUnlockSlotCard, OnRefreshUnlockSlotCard);
        EventDispatcher.AddEvent(EventID.OnSetChanged, ShowSet);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, OnRefreshUILevelCard);
        EventDispatcher.AddEvent(EventID.OnAmountCardChanged, OnRefreshAmountCard);
        EventDispatcher.AddEvent(EventID.OnSetChanged, UpdateStatusEquip);
        EventDispatcher.AddEvent(EventID.OnNewCardUnlock, ReloadIventory);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccess_SubjectId, OnUpgradeSubjectSuccess_SubjectId);
    }

    public void OnRefreshUnlockSlotCard(object o)
    {
        foreach (var card in slotCards)
        {
            card.CheckUnlockCardSlot();
        }
    }

    public void OnUpgradeSubjectSuccess_SubjectId(object o)
    {
        IdSubjectType idSubjectType = (IdSubjectType)o;
        if(idSubjectType == IdSubjectType.CARD_PRESET)
        {
            GameDatas.IndexSlotCard += 5;
            foreach (var card in slotCards)
            {
                card.CheckUnlockCardSlot();
            }
        }
    }

    public void OnRefreshUILevelCard(object o)
    {
        for (int i = 0; i < cards_Iventory.Count; i++)
        {
            cards_Iventory[i].OnLevelCardChanged();
            cards_Set[i].OnLevelCardChanged();
        }
    }

    public void OnRefreshAmountCard(object o)
    {
        for (int i = 0; i < cards_Iventory.Count; i++)
        {
            cards_Iventory[i].OnAmountCardChanged();
            cards_Set[i].OnAmountCardChanged();
        }
    }

    public void OnClickNewCard()
    {
        Gui.OpenBoard(UiPanelType.PopupCardNew);
    }

    private void ShowIventory()
    {
        foreach (var t in cards)
        {
            var card = Instantiate(cardElement, parentCard);
            CardElementData data = new CardElementData(t);
            card.isCardIventory = true;
            card.SetData(data);
            cards_Iventory.Add(card);

            var cardShow = Instantiate(cardElement, contentSet);
            cardShow.isCardIventory = false;
            cardShow.SetData(data);
            cardShow.gameObject.SetActive(false);
            cards_Set.Add(cardShow);
        }
    }

    private CardElement currentCard;
    private void ShowSet(object indexSet)
    {
        foreach (var card in cards_Set)
        {
            card.gameObject.SetActive(false);
        }

        int count = GameDatas.
            GetLevelSubjectLab(IdSubjectType.CARD_PRESET) == 1 ? 5 : 0;
        foreach (var data in cards)
        {
            if (count >= slotCards.Length) break;
            if (!slotCards[count].isOpenSlot) continue;
            if (!GameDatas.IsCardEquiped(data.id)) continue;

            var card = cards_Set.Find(c => c.cardID == data.id);
            if (card != null)
            {
                card.gameObject.SetActive(true);
                card.transform.position = slotCards[count].transform.position;
                card.transform.SetParent(slotCards[count].transform);
                currentCard = card;
                slotCards[count].isOpenSlot = true;
                count++;
            }
        }
    }

    private void ReloadIventory(object o)
    {
        foreach (var card in cards_Iventory)
        {
            card.CheckUnlockCard();
        }
    }

    private void UpdateStatusEquip(object o)
    {
        foreach (var card in cards_Iventory)
        {
            card.CheckEquipCard();
        }
    }

    private void RemoveEvent()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefreshUnlockSlotCard, OnRefreshUnlockSlotCard);
        EventDispatcher.RemoveEvent(EventID.OnSetChanged, ShowSet);
        EventDispatcher.RemoveEvent(EventID.OnSetChanged, UpdateStatusEquip);
        EventDispatcher.RemoveEvent(EventID.OnLevelCardChanged, OnRefreshUILevelCard);
        EventDispatcher.RemoveEvent(EventID.OnAmountCardChanged, OnRefreshAmountCard);
        EventDispatcher.RemoveEvent(EventID.OnNewCardUnlock, ReloadIventory);
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }
}
