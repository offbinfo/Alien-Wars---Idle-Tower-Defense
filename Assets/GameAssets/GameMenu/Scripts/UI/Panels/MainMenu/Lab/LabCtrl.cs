using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabCtrl : Singleton<LabCtrl>
{

    [SerializeField] private List<SlotUpgradeUI> slots;
    [SerializeField]
    private SubjectManager subjectManager;
    [SerializeField]
    private ScrollRect scrollRect;

    private int[] costUnlockSlot = new int[] {
        0,100,400,1400,3000
    };

    public ScrollRect ScrollRect { get => scrollRect;}
    public List<SlotUpgradeUI> Slots { get => slots; }
    public SubjectManager SubjectManager { get => subjectManager; }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshUnlockSlotLab, OnRefreshUnlockSlotLab);
        InitDataSlotUpgrade();
    }

    private void InitDataSlotUpgrade()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetData(i, costUnlockSlot[i]);
            slots[i].OnOpenLab += OnOpenSubjectLab;
        }
    }

    private void OnOpenSubjectLab(int indexSubject)
    {
        subjectManager.OpenTabSubject(indexSubject);
    }

    private void OnRefreshUnlockSlotLab(object o)
    {
        foreach (var slot in slots)
        {
            slot.CheckSlotUpgradeUnlock();
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefreshUnlockSlotLab, OnRefreshUnlockSlotLab);
    }
}
