using language;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBotChallenge : MonoBehaviour
{
    [SerializeField]
    private List<PropertyUIBot> propertyUIs;
    [SerializeField]
    private TMP_Text txtNameBot;
    [SerializeField]
    private GameObject panelLock;
    [SerializeField]
    private GameObject panelInfor;
    [SerializeField]
    private TypeBot typeBot;
    private SO_Bot_Base _data;
    [SerializeField]
    private LabCategory labCategory;
    [SerializeField]
    private GameObject blurInfor;
    [SerializeField]
    private TMP_Text txtDesc;
    private float priceBadeges = 3000;

    public void SetUp(SO_Bot_Base data)
    {
        _data = data;
        this.typeBot = data.typeBot;  
        txtNameBot.text = LanguageManager.GetText(typeBot.ToString());
        txtDesc.text = LanguageManager.GetText("desc_"+ typeBot.ToString());

        CheckUnlock();
    }

    private void OnEnable()
    {
        if(_data == null) return;
        CheckUnlock();
    }

    public void CheckUnlock()
    {
        if(GameDatas.IsBotChallengeUnlock(typeBot))
        {
            panelLock.SetActive(false);
            panelInfor.SetActive(true);
        } else
        {
            panelLock.SetActive(true);
            panelInfor.SetActive(false);
        }

        SetPropertyUIBot();
    }

    public void ShowInfoBot()
    {
        blurInfor.SetActive(true);
    }

    public void BtnUnlock()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.BADGES, priceBadeges, OnBuySuccess);
    }

    private void OnBuySuccess(bool obj)
    {
        if(obj)
        {
            GameDatas.BotChallengeUnlock(typeBot);
            GameDatas.UnlockClusterLabInfor(labCategory);
            EventDispatcher.PostEvent(EventID.OnRefeshUILabs, 0);
            CheckUnlock();
        }
    }

    private void SetPropertyUIBot()
    {
        int index = 0;
        if (_data.levelMaxDuration == 0) index = 1;
        for (int i = 0; i < propertyUIs.Count; i++)
        {
            var item = propertyUIs[i];
            item.SetData(_data, i + index);
        }
    }
}
