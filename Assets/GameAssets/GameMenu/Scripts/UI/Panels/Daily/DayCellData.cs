using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCellData : BaseUICellData
{
    public DailyData data;
    public Sprite icon;
    public int index;
    private DailyRewardSO dailyRewardSO;

    public DailyData Data => data;
    public DailyRewardSO DailyRewardSO => dailyRewardSO;
    public DayCellData(DailyData dailyData, Sprite icon, int index, DailyRewardSO dailyRewardSO)
    {
        this.data = dailyData;
        this.icon = icon;
        this.index = index;
        this.dailyRewardSO = dailyRewardSO;
    }

}
