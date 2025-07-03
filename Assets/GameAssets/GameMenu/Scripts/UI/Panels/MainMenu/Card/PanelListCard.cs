using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelListCard : GameMonoBehaviour
{

    [SerializeField] PopupCardNew popup_cardNew;
    [SerializeField] CardShowUnlock cardPrefab;
    [SerializeField] Transform parentContent;
    public Image img_slider;

    private List<CardShowUnlock> cardsShow = new List<CardShowUnlock>();
    private void Awake()
    {
        SpawnallCard();
    }
    private void OnEnable()
    {
        img_slider.fillAmount = 1f / 30f * GameDatas.countSpinCard;
    }
    public void SpawnallCard()
    {
        cardsShow.Clear();
        var list_data = popup_cardNew.levelUnlockCards;
        var level = GameDatas.levelCardUnlock;
        for (int i = 0; i < list_data.Count; i++)
        {
            var data = list_data[i].list_card;
            for (int x = 0; x < data.Count; x++)
            {
                var cardShow = Instantiate(cardPrefab, parentContent);
                cardsShow.Add(cardShow);
                var data1 = Cfg.cardCtrl.GetCard(data[x]);
                var status = CardShowStatus.LOCK;
                if (level >= i)
                    status = CardShowStatus.UNLOCK;
                else if (level == i - 1)
                    status = CardShowStatus.PREVIEW;
                else
                    status = CardShowStatus.LOCK;
                cardShow.SetUp(data1, status, i);
            }
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
