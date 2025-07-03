using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubjectContainer : GameMonoBehaviour
{

    [SerializeField] SubjectElementUI prefab;
    [SerializeField] Transform parent;
    [SerializeField] SubjectType type;
    [SerializeField] TMP_Text txt_Title;
    private List<SubjectElementUI> subjectElementUIs = new();

    public List<SubjectElementUI> SubjectElementUIs { get => subjectElementUIs; }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefeshUILabs, OnRefeshUILabs);
        EventDispatcher.AddEvent(EventID.OnBuyClusterUpgrader, OnCheckSubjectActive);
        InitData();
    }

    [SerializeField]
    private GameObject iconArrow;
    [SerializeField]
    private GameObject iconScroll;

    public bool isScroll = false;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnCheckSubjectActive(object o)
    {
        UpgraderCategory upgraderCategory = (UpgraderCategory)o;
        if (upgraderCategory == UpgraderCategory.WORKSHOP_KILL_BEAM)
        {
            foreach (var item in subjectElementUIs)
            {
                item.CheckUnlockSubject();
            }
        }
    }

    public void ScrollItemSubject()
    {
        if(isScroll)
        {
            isScroll = false;
            iconArrow.SetActive(true);
            iconScroll.SetActive(false);
            parent.gameObject.SetActive(true);
        } else
        {
            isScroll = true;
            iconArrow.SetActive(false);
            iconScroll.SetActive(true);
            parent.gameObject.SetActive(false);
        }
    }

    private void InitData()
    {
        foreach (var item in Cfg.labCtrl.LapManager.GetAllSubjectByType(type))
        {
            var element = Instantiate(prefab, parent);
            SubjectElementData cellData = new(item);
            element.SetData(cellData);
            subjectElementUIs.Add(element);

        }
        txt_Title.text = LanguageManager.GetText(type.ToString());
    }

    private void UpdateUI()
    {
        Canvas.ForceUpdateCanvases();
    }

    private void OnRefeshUILabs(object obj)
    {
        foreach (var item in subjectElementUIs)
        {
            item.CheckUnlockSubject();
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshUILabs, OnRefeshUILabs);
    }

}
