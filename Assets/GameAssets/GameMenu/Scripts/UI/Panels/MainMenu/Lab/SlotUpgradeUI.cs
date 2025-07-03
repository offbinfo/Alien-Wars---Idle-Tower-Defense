using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUpgradeUI : GameMonoBehaviour
{
    public GameObject obj_bgoffline => obj_BgOffline;

    [SerializeField] int index;
    [SerializeField] TMP_Text txt_title;
    [SerializeField] TMP_Text txt_ID;
    [SerializeField] TMP_Text txt_info;
    [SerializeField] TMP_Text text_rushCost;
    [SerializeField] TMP_Text text_timeFinish;
    [SerializeField] TMP_Text txt_PriceUnlock;
    [SerializeField] Image img_process;

    [SerializeField] GameObject obj_BgOffline;
    [SerializeField] GameObject obj_BgOnline;
    [SerializeField] GameObject obj_BgLock;
    private bool isUnlockSlot = false;

    public Action<int> OnOpenLab;

    [SerializeField]
    private int priceUnlock;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnStartUpgradeSubject, (o) => {
            if ((int)o != index)
                return;
            UpdateUI();
            UpdateInfo();
        });
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, (o) => {
            if ((int)o != index)
                return;
            UpdateUI();
        });
    }

    public void SetData(int indexSlot ,int price)
    {
        index = indexSlot;
        priceUnlock = price;

        //Cfg.labCtrl.slotUpgrade.OnUpgrade += UpdateInfo;
        Cfg.labCtrl.AddUpgradeListener(index, UpdateInfo);

        UpdateUI();
    }

    private void UpdateUI()
    {
        txt_title.text = LanguageManager.GetText("Lab") + " " + (index + 1);

        txt_PriceUnlock.text = "<sprite name=icon_gem>"+priceUnlock.ToString();
        CheckSlotUpgradeUnlock();
    }

    private void UpdateInfo()
    {
        if (!isUnlockSlot || !Cfg.labCtrl.IsInUpgrade(index))
            return;

        obj_BgOnline.SetActive(isUnlockSlot && Cfg.labCtrl.IsInUpgrade(index));
        Subject_SO subject = Cfg.labCtrl.GetSubjectBySlot(index);
        txt_ID.text = LanguageManager.GetText(subject.id.ToString());

        var format = subject.format switch
        {
            Format.NUMBER => "",
            Format.PERCENT => "%",
            Format.SECOND => "s",
            _ => ""
        };

        float currentProperty = Mathf.Abs(subject.GetCurrentProperty());
        float nextProperty = Mathf.Abs(subject.GetNextProperty());

        txt_info.text = currentProperty + format + "<sprite name=mui ten ngang>" + nextProperty + format;
        text_rushCost.text = "<sprite name=icon_gem> " + Cfg.labCtrl.GetGemRush(index);

        var totalTime = GameDatas.GetTimeTargetSubject(subject.id) - DateTime.Now;
        text_timeFinish.text = totalTime.Display();

        var time = subject.GetCurrentTime() - (float)totalTime.TotalSeconds;
        img_process.fillAmount = time / subject.GetCurrentTime();
    }
    public void BtnRush()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, Cfg.labCtrl.GetGemRush(index), OnBuySkipUpgradeSuccess);
    }

    private void OnBuySkipUpgradeSuccess(bool obj)
    {
        if (obj)
        {
            Cfg.labCtrl.RushUpgrade(index);
        }
    }

    public void CheckSlotUpgradeUnlock()
    {
        isUnlockSlot = GameDatas.CountSlotLabUnlock - 1 >= index;
        obj_BgLock.SetActive(GameDatas.CountSlotLabUnlock - 1 < index);
        obj_BgOffline.SetActive(isUnlockSlot);
        CheckActiveSlotNext();
        obj_BgOffline.SetActive(isUnlockSlot && !Cfg.labCtrl.IsInUpgrade(index));
        obj_BgOnline.SetActive(isUnlockSlot && Cfg.labCtrl.IsInUpgrade(index));
    }

    private void CheckActiveSlotNext()
    {
        int indexSlot = GameDatas.CountSlotLabUnlock;
        gameObject.SetActive(indexSlot >= index);
    }

    public void UnlockSlot()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceUnlock, OnBuySuccess);
    }

    private void OnBuySuccess(bool isSuceess)
    {
        if (isSuceess) 
        {
            GameDatas.CountSlotLabUnlock += 1;
            EventDispatcher.PostEvent(EventID.OnRefreshUnlockSlotLab, 0);
        }
    }

    public void OnClick()
    {
        OnOpenLab?.Invoke(index);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnStartUpgradeSubject, (o) => {
            if ((int)o != index)
                return;
            UpdateUI();
            UpdateInfo();
        });
        EventDispatcher.RemoveEvent(EventID.OnUpgradeSubjectSuccessLab, (o) => {
            if ((int)o != index)
                return;
            UpdateUI();
        });

        if(Cfg != null)
        {
            Cfg.labCtrl.RemoveUpgradeListener(index, UpdateInfo);
        }
    }
}
