using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LabInforCtrl : GameMonoBehaviour
{
    [SerializeField]
    private SO_LabManager sO_LabManager;
    public int indexSlotLab;
    public SlotUpgrade slotUpgrade;

    public SO_LabManager LapManager => sO_LabManager;

    private Dictionary<int, SlotUpgrade> activeUpgrades = new Dictionary<int, SlotUpgrade>();
    private Dictionary<int, Action> onUpgradeEvents = new Dictionary<int, Action>();

    private void Start()
    {
        Invoke(nameof(UpgradeContinue), 2f);
    }

    private void UpgradeContinue()
    {
        for (int slotIndex = 0; slotIndex < GameDatas.CountSlotLabUnlock; slotIndex++)
        {
            int index = slotIndex;
            var id = GameDatas.SlotSave(index);

            DebugCustom.LogColor("subject " + id);

            if (id != -1)
            {
                Subject_SO subject = LapManager.GetSingleSubjectById((IdSubjectType)id);
                if (subject != null)
                {
                    SlotUpgrade slotUpgrade = new SlotUpgrade(slotIndex, subject);
                    activeUpgrades[slotIndex] = slotUpgrade;
                    // Khởi động lại quá trình nâng cấp
                    StartCoroutine(I_CheckFinishUpgrade(index, subject));
                }
            }
        }
    }


    public void UpgradeInforLab(Subject_SO subject)
    {
        /*if (indexSlotLab >= slots.Count)
            return;
        var price = (int)subject.GetCurrentPrice();
        if (GameDatas.Gold < price)
            return;
        GameDatas.Gold -= price;*/
        StartUpgrade(indexSlotLab, subject);
    }

    public void RushUpgrade(int slotIndex)
    {
        if (activeUpgrades.ContainsKey(slotIndex))
        {
            StopCoroutine(I_CheckFinishUpgrade(slotIndex, 
                activeUpgrades[slotIndex].Subject, isRushCompleted: true));
            CompleteUpgrade(slotIndex);
        }
    }

    public bool IsInUpgrade(int slotIndex)
    {
        return activeUpgrades.ContainsKey(slotIndex);
    }

    public Subject_SO GetSubjectBySlot(int slotIndex)
    {
        return activeUpgrades.ContainsKey(slotIndex) ? activeUpgrades[slotIndex].Subject : null;
    }

    public int GetGemRush(int slotIndex)
    {
        if (!activeUpgrades.ContainsKey(slotIndex)) return 0;

        Subject_SO subject = activeUpgrades[slotIndex].Subject;
        if (subject == null || subject.timeTargetFinish <= DateTime.Now)
            return 0;

        float value = 0.003f * (int)(subject.timeTargetFinish - DateTime.Now).TotalSeconds;
        return Mathf.CeilToInt(value);
    }

    public void StartUpgrade(int slotIndex, Subject_SO subject)
    {
        if (activeUpgrades.ContainsKey(slotIndex))
            return;

        SlotUpgrade slotUpgrade = new SlotUpgrade(slotIndex, subject);
        activeUpgrades[slotIndex] = slotUpgrade;

        GameDatas.SetTimeTargetSubject(subject.id, DateTime.Now.AddSeconds(subject.Time(subject.currentLevel)));
        GameDatas.SlotSave(slotIndex, subject.id);
        StartCoroutine(I_CheckFinishUpgrade(slotIndex, subject));
        EventDispatcher.PostEvent(EventID.OnStartUpgradeSubject, slotIndex);
    }

    private IEnumerator I_CheckFinishUpgrade(int slotIndex, Subject_SO subject, bool isRushCompleted = false)
    {
        while (true)
        {
            if (onUpgradeEvents.ContainsKey(slotIndex))
            {
                onUpgradeEvents[slotIndex]?.Invoke(); // Gọi sự kiện mỗi giây
            }
            yield return new WaitForSecondsRealtime(1f);

            if (DateTime.Now >= subject.timeTargetFinish)
                break;
        }

        if(!isRushCompleted)
        {
            CompleteUpgrade(slotIndex);
        }
    }

    private void CompleteUpgrade(int slotIndex)
    {
        if (!activeUpgrades.ContainsKey(slotIndex))
            return;

        Subject_SO subject = activeUpgrades[slotIndex].Subject;
        GameDatas.SetTimeTargetSubject(subject.id, default);
        GameDatas.SetLevelSubjectLab(subject.id, subject.currentLevel + 1);
        GameDatas.SetNullSlotSave(slotIndex);

        activeUpgrades.Remove(slotIndex);
        EventDispatcher.PostEvent(EventID.OnUpgradeSubjectSuccessLab, slotIndex);
        EventDispatcher.PostEvent(EventID.OnUpgradeSubjectSuccess_SubjectId, subject.id);

        float totalTime = subject.GetCurrentTime() + GameDatas.TotalTimeUpgradeLab;
        GameDatas.TotalTimeUpgradeLab = totalTime;
        EventChallengeListenerManager.UpgradeLabTotalDays(0);
    }

    private void SaveTimeUpgrade(Subject_SO subject)
    {
        GameDatas.SetTimeTargetSubject(subject.id, GameDatas.GetTimeTargetSubject(subject.id));
    }

    public void AddUpgradeListener(int slotIndex, Action callback)
    {
        if (!onUpgradeEvents.ContainsKey(slotIndex))
        {
            onUpgradeEvents[slotIndex] = callback;
        }
        else
        {
            onUpgradeEvents[slotIndex] += callback;
        }
    }

    public void RemoveUpgradeListener(int slotIndex, Action callback)
    {
        if (onUpgradeEvents.ContainsKey(slotIndex))
            onUpgradeEvents[slotIndex] -= callback;
    }

}
