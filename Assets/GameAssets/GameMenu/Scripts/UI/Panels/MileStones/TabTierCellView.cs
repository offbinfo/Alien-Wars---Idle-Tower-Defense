using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabTierCellView : GameMonoBehaviour
{

    [SerializeField]
    private ItemMilestoneElement milestoneElement;
    [SerializeField]
    private Transform content;
    public WorldType typeWorld;

    [SerializedDictionary("TypeMilestoneReward", "Image")]
    public SerializedDictionary<TypeMilestone, Sprite> dictImageMileStones;
    [SerializeField]
    private ScrollRect scrollRect;
    public List<MilestoneCellView> milestoneCells;
    private bool isInit;
    public List<int> PointWaveMileStones = new();


    private void Start()
    {
        scrollRect.verticalNormalizedPosition = 0f;
        EventDispatcher.AddEvent(EventID.OnUsingPremiumMileStones, OnRefreshUI);
    }

    private void OnRefreshUI(object obj)
    {
        int wavePlaying = GameDatas.GetHighestWaveInWorld((int)this.typeWorld);
        foreach (var mileStone in milestoneCells)
        {
            mileStone.CheckUnLockItemReward(wavePlaying);
        }
    }

    private void OnEnable()
    {
        if (!isInit) return;
        CheckItemRewardMileStoneReceived();
    }

    public void SetData(List<RwdAll> rwdAlls, WorldType typeWorld)
    {
        this.typeWorld = typeWorld;
        BuildData(rwdAlls);
    }

    private void BuildData(List<RwdAll> rwdAlls)
    {
        GetAllPointWaveMileStone(rwdAlls);
        for (int i = rwdAlls.Count - 1; i >= 0; i--)
        {
            ItemMilestoneElement cellView = Instantiate(milestoneElement, content);
            cellView.SetData(rwdAlls[i], this);
        }
        isInit = true;
        CheckItemRewardMileStoneReceived();
    }

    private void GetAllPointWaveMileStone(List<RwdAll> rwdAlls)
    {
        PointWaveMileStones.Add(0);
        for (int i = 0; i < rwdAlls.Count; i++)
        {
            PointWaveMileStones.Add(rwdAlls[i].wave);
        }
    }

    private void CheckItemRewardMileStoneReceived()
    {
        AllItemMileStoneData data = GameDatas.LoadAllItems();

        if (data == null) return;
        if (data != null)
        {
            foreach (ItemData item in data.items)
            {
                foreach (var mileStone in milestoneCells)
                {
                    mileStone.CheckItemRewardReceived(item);
                }
            }
        }
    }
}
