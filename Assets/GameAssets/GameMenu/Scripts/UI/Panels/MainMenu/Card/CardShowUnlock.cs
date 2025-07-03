using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using language;
public enum CardShowStatus
{
    LOCK,
    PREVIEW,
    UNLOCK,
}
public class CardShowUnlock : MonoBehaviour
{
    [SerializeField] List<GameObject> icons;
    [SerializeField] GameObject BG_preview;
    [SerializeField] GameObject BG_Common;
    [SerializeField] GameObject BG_Rare;
    [SerializeField] GameObject BG_Epic;
    [SerializeField] GameObject BG_Divine;
    [SerializeField] TMP_Text txt_Status;
    private CardShowStatus status;
    private Card_SO data;
    private int levelUnlock;
    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnLevelUnlockCardChanged, UpdateStatus);
    }
    private void OnEnable()
    {
        UpdateStatus(null);
    }
    public CardShowStatus Status
    {
        get { return status; }
        set
        {
            status = value;
            BG_preview.SetActive(status == CardShowStatus.PREVIEW);
            BG_Common.SetActive(status == CardShowStatus.UNLOCK && data.type == TypeCard.COMMON);
            BG_Rare.SetActive(status == CardShowStatus.UNLOCK && data.type == TypeCard.RARE);
            BG_Epic.SetActive(status == CardShowStatus.UNLOCK && data.type == TypeCard.EPIC);
            BG_Divine.SetActive(status == CardShowStatus.UNLOCK && data.type == TypeCard.DIVINE);

            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].SetActive(i == (int)data.id && status != CardShowStatus.LOCK);
            }

            if (data.isUnlock && status == CardShowStatus.UNLOCK)
                txt_Status.text = LanguageManager.GetText("owned");
            else if (!data.isUnlock && status == CardShowStatus.UNLOCK)
                txt_Status.text = "";
            else if (status == CardShowStatus.PREVIEW)
                txt_Status.text = LanguageManager.GetText("next");
            else
                txt_Status.text = "";

        }
    }
    public void SetUp(Card_SO data, CardShowStatus status, int levelUnlock)
    {
        this.data = data;
        Status = status;
        this.levelUnlock = levelUnlock;
    }
    public void UpdateStatus(object o)
    {
        if (data == null) return;
        var level = GameDatas.levelCardUnlock;
        if (level >= levelUnlock)
            Status = CardShowStatus.UNLOCK;
        else if (level == levelUnlock - 1)
            Status = CardShowStatus.PREVIEW;
        else
            Status = CardShowStatus.LOCK;
    }
}
