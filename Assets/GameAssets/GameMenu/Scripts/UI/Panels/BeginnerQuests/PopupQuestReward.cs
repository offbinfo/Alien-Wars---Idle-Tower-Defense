using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupQuestReward : UIPanel, IBoard
{

    [SerializeField] BQElement prefabElement;
    [SerializeField] List<BeginnerQuest_SO> list_data;
    [SerializeField] Transform contentParent;
    [SerializeField] TMP_Text txt_time;
    private List<BQElement> elements = new List<BQElement>();
    private bool isSetUp;

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupQuestReward;
    }
    private void OnEnable()
    {
        OnAppear();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private void Awake()
    {
        SetUp();
    }

    private void Init()
    {
        StartCoroutine(UpdateTask30Remain());
        StartCoroutine(ICountTimeEnd());

        if (isSetUp)
        {
            OnRefreshQuests();
        }
    }

    private void OnRefreshQuests()
    {
        for (int i = 0; i < list_data.Count; i++)
        {
            elements[i].SetUp(list_data[i]);
        }
    }

    private void SetUp()
    {
        foreach (var data in list_data)
        {
            var element = Instantiate(prefabElement, contentParent);
            elements.Add(element);
        }
        isSetUp = true;
        OnRefreshQuests();
    }

    private IEnumerator UpdateTask30Remain()
    {
        var element = new BQElement();
        foreach (var d in elements)
        {
            if (d.id == BeginnerQuestID.REMAIN_30M_LOGIN) element = d;
        }

        while (element.currentProgress <= element.maxTarget)
        {
            yield return new WaitForSeconds(60);
            element.SetUp(element.d);
        }

    }
    public void OnClickBtnClose()
    {
        Close();
    }

    private IEnumerator ICountTimeEnd()
    {
        var timetarget = GameDatas.timeTargetBeginnerQuest;
        while (DateTime.Now <= timetarget)
        {
            var offsetTime = timetarget - DateTime.Now;
            txt_time.text = offsetTime.Display();
            yield return new WaitForSeconds(1f);
        }
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
