using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotUpgrade : GameMonoBehaviour
{
    public int SlotIndex { get; private set; }
    public Subject_SO Subject { get; private set; }
    public DateTime FinishTime { get; private set; }
    public Action OnUpgrade;

    public SlotUpgrade(int slotIndex, Subject_SO subject)
    {
        SlotIndex = slotIndex;
        Subject = subject;
        FinishTime = DateTime.Now.AddSeconds(subject.Time(subject.currentLevel));
        //GameDatas.SetTimeTargetSubject(subject.id, FinishTime);
        GameDatas.SlotSave(slotIndex, subject.id);
    }
}
