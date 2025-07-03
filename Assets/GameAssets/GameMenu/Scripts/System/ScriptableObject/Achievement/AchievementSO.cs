using language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "DataAchievement", menuName = "Data/DataAchievement", order = 0)]
public class AchievementSO : MonoBehaviour
{
    public AchievementID id;
    public int level;
    public string title => LanguageManager.GetText(id.ToString());
    public int gold;
    public int gem;
    public int maxvalue;

/*    public bool claimedAll => GameDatas.IsClaimedAllLevelAchievement(id);
    public bool isUnlock => level == GameDatas.GetLevelAchievement(id);
    public bool isDone => isUnlock && GameDatas.GetValue(id) >= maxvalue; 
    public void Claim()
    {
        GameAnalytics.LogEvent_EarnGold("achievement", gold);
        GameDatas.Gold += gold;
        GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + gold);
    }*/
}
