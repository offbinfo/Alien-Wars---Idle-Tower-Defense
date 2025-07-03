using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerQuestCellData : BaseUICellData
{
    public DailyQuestType dailyQuestType;
    public DailyQuestSpecialType specialType;
    public RewardDailyQuest amountReward;
    public int countQuest;
    public SO_DailyQuestManager sO_DailyQuest;

    public BeginnerQuestCellData(DailyQuestType dailyQuestType, DailyQuestSpecialType specialType = DailyQuestSpecialType.DestroyEnemiesHighlight,
        RewardDailyQuest amountReward = null, int countQuest = 0, SO_DailyQuestManager sO_DailyQuest = null)
    {
        this.dailyQuestType = dailyQuestType;
        this.specialType = specialType;
        this.amountReward = amountReward;
        this.countQuest = countQuest;
        this.sO_DailyQuest = sO_DailyQuest;
    }
}
