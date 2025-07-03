using language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using static ES3;

public class SubjectElementUI : BaseUICellView
{
    [SerializeField] TMP_Text txt_id;
    [SerializeField] TMP_Text txt_info;
    [SerializeField] TMP_Text txt_time;
    [SerializeField] TMP_Text txt_gold;
    [SerializeField] GameObject obj_OnProcess;
    [SerializeField] GameObject obj_NoBuy;
    private long price;

    [SerializeField] GameObject obj_Buy;
    [SerializeField] GameObject obj_MaxUpgrader;
    private Subject_SO subjectData;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnStartUpgradeSubject, OnRefreshUISubject);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccess_SubjectId, (o) => {
            UpdateUI();
        });
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, (o) => {
            UpdateUI();
        });
        EventDispatcher.AddEvent(EventID.OnGoldChanged, OnGoldChanged);
    }

    private void OnGoldChanged(object obj)
    {
        CheckBuy();
    }

    private void OnEnable()
    {
        CheckBuy();
    }

    private void CheckBuy()
    {
        obj_NoBuy.SetActive(price > GameDatas.Gold);
    }

    private void UpdateUI()
    {
        txt_id.text = LanguageManager.GetText(subjectData.id.ToString());

        long priceFloor = (long)Mathf.Floor(subjectData.GetCurrentPrice());
        txt_time.text = TimeSpan.FromSeconds(subjectData.GetCurrentTime()).Display();
        price = priceFloor;

        txt_gold.text = Extensions.FormatNumber(price);
        obj_OnProcess.SetActive(subjectData.timeTargetFinish >= DateTime.Now);
        CheckUnlockSubject();
        CheckBuy();
        CheckUpgradeMaxSubject();
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);

        SubjectElementData cellData = data as SubjectElementData;

        subjectData = cellData.Subject_SO;
        UpdateUI();
    }

    private void OnRefreshUISubject(object o)
    {
        UpdateUI();
    }

    private void CheckUpgradeMaxSubject()
    {
        var format = subjectData.format switch
        {
            Format.NUMBER => "",
            Format.PERCENT => "%",
            Format.SECOND => "s",
        };

        float currentProperty = Mathf.Abs(subjectData.GetCurrentProperty());
        float nextProperty = Mathf.Abs(subjectData.GetNextProperty());

        if (subjectData.currentLevel >= subjectData.levelMax)
        {
            obj_MaxUpgrader.SetActive(true);
            obj_Buy.SetActive(false);
            float property = currentProperty == 0 ? 1 : currentProperty;
            txt_info.text = property + format;
        }
        else
        {
            obj_MaxUpgrader.SetActive(false);
            obj_Buy.SetActive(true);
            txt_info.text = currentProperty + format + "<sprite name=mui ten ngang>" + nextProperty + format;
        }
    }

    public void CheckUnlockSubject()
    {
        if(subjectData.labCategory == LabCategory.WORKSHOP_DOUBLE_KILL_BEAM)
        {
            if (GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_KILL_BEAM))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        } else
        {
            if (subjectData.buyingType != BuyingType.REWARD ||
                GameDatas.IsClusterUnlockLabInfor(subjectData.labCategory))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        } 
    }

    public void OnClickUpgrade()
    {
        if (subjectData.timeTargetFinish >= DateTime.Now) // đang nâng cấp , just in case
            return;
        GameDatas.BuyUsingCurrency(CurrencyType.GOLD, price, OnBuySuccess);
    }

    private void OnBuySuccess(bool isSuceess)
    {
        if (isSuceess)
        {
            Cfg.labCtrl.UpgradeInforLab(subjectData);
            SubjectManager.instance.gameObject.SetActive(false);
            QuestEventManager.Upgrade(1);
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnStartUpgradeSubject, OnRefreshUISubject);
    }
}
