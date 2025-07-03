using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectManager : Singleton<SubjectManager>
{
    public GameObject obj_AttackResearch => subjectContainers[0].SubjectElementUIs[0].gameObject;
    [SerializeField]
    private List<SubjectContainer> subjectContainers;
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField]
    private ScrollRect scrollRect;

    private void Start()
    {
        txtTitle.text = LanguageManager.GetText("Lab") + " " + 1;
    }

    public ScrollRect ScrollRect { get => scrollRect;}

    public void OpenTabSubject(int indexTab)
    {
        Cfg.labCtrl.indexSlotLab = indexTab;
        gameObject.SetActive(true);    
        txtTitle.text = LanguageManager.GetText("Lab")+" "+ (indexTab + 1);
    }
}
