using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBeginerQuest : UIPanel, IBoard
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private BeginnerQuestElement beginnerQuestElement;
    [SerializeField]
    private SO_DailyQuestManager SO_DailyQuestManager;
    [SerializeField]
    private List<WeekendQuestElement> weekendQuestElements;
    [SerializeField]
    private Slider sliderReward;
    [SerializeField]
    private TMP_Text txtPoint;
    private List<GameObject> itemQuests = new();

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupDailyQuest;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    private void Start()
    {
        sliderReward.maxValue = 38;
        BuildDataPointWeekend();
        ChangeQuestDayNew();
        SetSliderReward();

        EventDispatcher.AddEvent(EventID.OnRefreshDailyGift, OnRefreshDailyGift);
    }

    private void OnRefreshDailyGift(object obj)
    {
        SetSliderReward();
    }

    private void SetSliderReward()
    {
        sliderReward.value = GameDatas.GetAccumulateDailyGift();
        txtPoint.text = GameDatas.GetAccumulateDailyGift().ToString();
    }

    private void BuildDataPointWeekend()
    {
        int[] keysArray = SO_DailyQuestManager.rewardDailyQuestsWeekend.Keys.ToArray();

        for (int i = 0; i < keysArray.Length; i++)
        {
            weekendQuestElements[i].SetData(keysArray[i]
                , SO_DailyQuestManager.rewardDailyQuestsWeekend[keysArray[i]]);
        }
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
        ScaleUpItem();
    }

    private void ChangeQuestDayNew()
    {
        RewardDailyQuest amountReward =
    SO_DailyQuestManager.GetRewardDailyQuestByWorld((WorldType)GameDatas.GetHighestWorld());

        AllDailyQuestData itemDailyQuestDatas = GameDatas.LoadAllQuests();
        var sequence = DOTween.Sequence();

        for (int i = 0; i < itemDailyQuestDatas.items.Count; i++)
        {
            BeginnerQuestElement cellView = Instantiate(beginnerQuestElement, content);

            ItemDailyQuestData itemDailyQuestData = itemDailyQuestDatas.items[i];

            BeginnerQuestCellData cellData = new(itemDailyQuestData.dailyQuestType
                , amountReward: amountReward, countQuest: itemDailyQuestData.countQuest, sO_DailyQuest: SO_DailyQuestManager);
            cellView.SetData(cellData);
            itemQuests.Add(cellView.gameObject);

            sequence.Append(cellView.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
        }
    }

    private void ScaleUpItem()
    {
        if (itemQuests.Count > 0)
        {
            var sequence = DOTween.Sequence();

            for (int i = 0; i < itemQuests.Count; i++)
            {
                sequence.Append(itemQuests[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
            }
        }
    }

    private void ScaleDownItem()
    {
        if (itemQuests.Count > 0)
        {
            for (int i = 0; i < itemQuests.Count; i++)
            {
                itemQuests[i].transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutBack);
            }
        }
    }

    private void OnDisable()
    {
        ScaleDownItem();
    }

    public void BtnClaimNow()
    {
        
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
