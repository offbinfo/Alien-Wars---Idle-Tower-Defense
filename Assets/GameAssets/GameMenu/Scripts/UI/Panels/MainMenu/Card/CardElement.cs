using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardElement : BaseUICellView
{
    public CardID cardID;
    [SerializeField]private TMP_Text txt_count;
    [SerializeField]private TMP_Text txt_level;
    [SerializeField]private TMP_Text txt_ID;
    [SerializeField]private GameObject obj_equip;
    [SerializeField]private GameObject obj_ButtonPick;
    [SerializeField]private GameObject common;
    [SerializeField]private GameObject rare;
    [SerializeField]private GameObject epic;
    [SerializeField]private GameObject divine;
    [SerializeField]private List<GameObject> objStarMain;
    [SerializeField]private Image iconCard;
    private Card_SO cardData;
    public bool isCardIventory = true;
    public bool isShowInforCard;

    private bool isEquip;

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);
        CardElementData cellData = data as CardElementData;

        cardData = cellData.Card_SO;
        if (cardData != null)
        {
            cardID = cardData.id;

            iconCard.sprite = cardData.icon;    

            common.SetActive(cardData.type == TypeCard.COMMON);
            rare.SetActive(cardData.type == TypeCard.RARE);
            epic.SetActive(cardData.type == TypeCard.EPIC);
            divine.SetActive(cardData.type == TypeCard.DIVINE);

            OnAmountCardChanged();
            OnLevelCardChanged();
            txt_ID.text = cardData.id.ToString();

            CheckEquipCard();
            CheckUnlockCard();
        }
        obj_ButtonPick.SetActive(isCardIventory);
    }

    public void CheckUnlockCard()
    {
        gameObject.SetActive(GameDatas.IsUnlockCard(cardID));
    }

    public void CheckEquipCard()
    {
        isEquip = GameDatas.IsCardEquiped(cardID);
        obj_equip.SetActive(isEquip);
    }

    public void OnAmountCardChanged()
    {
        txt_count.text = string.Format("{0}/5", cardData.amountCard);
    }
    public void OnLevelCardChanged()
    {
        for (int i = 0; i < objStarMain.Count; i++)
        {
            objStarMain[i].SetActive(i < cardData.currentlevel);
        }
    }

    public void OnCLickEquipCard()
    {
        if (TabCardCtrl.instance.CheckFullSlotCard() && !GameDatas.IsCardEquiped(cardID))
            return;

        bool isCurrentlyEquipped = GameDatas.IsCardEquiped(cardID);

        if (isCurrentlyEquipped)
        {
            UnEquipCard();
        }
        else
        {
            EquipCard();
        }

        EventDispatcher.PostEvent(EventID.OnSetChanged, cardID);
    }

    private void UnEquipCard()
    {
        Cfg.cardCtrl.currentIndexSet--;
        GameDatas.UnEquipedCard(cardID);
        EventDispatcher.PostEvent(EventID.OnUnequipcard, cardID);
        isEquip = false;
        obj_equip.SetActive(false);
    }

    private void EquipCard()
    {
        Cfg.cardCtrl.currentIndexSet += 1;
        GameDatas.SetCardEquiped(cardID);
        EventDispatcher.PostEvent(EventID.OnEquipCard, cardID);
        isEquip = true;
        obj_equip.SetActive(true);
    }

    public void OnCLickShowInforCard()
    {
        if(isShowInforCard)
        {
            //Gui.OpenBoard(UiPanelType.PopupCardInfoRoll);
            Gui.DicBoards[UiPanelType.PopupCardInfoRoll].gameObject.SetActive(true);
            PopupCardInfoRoll.Instance.InitData(cardData);
        } else
        {
            Gui.OpenBoard(UiPanelType.PopupCardInfo);
            PopupCardInfo.Instance.InitData(cardData);
        }
    }
}
