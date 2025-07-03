using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BeginnerQuest_SO", menuName = "Data/BeginnerQuests/BeginnerQuest_SO", order = 0)]
public class BeginnerQuest_SO : ScriptableObject
{

    public BeginnerQuestID id;
    public float currentProgress
    {
        get
        {
            var value = GameDatas.GetBeginnerQuestProgress(id);
            if (id == BeginnerQuestID.UPGRADE_DAMAGE_20)
                value = ConfigManager.instance.upgraderCtrl.GetStatBase(UpgraderID.attack_damage);
            if (id == BeginnerQuestID.UNLOCK_SATELLITES)
                value = GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_SATELLITES) == true ? 1 : 0;
            if (id == BeginnerQuestID.UNLOCK_GOLD_BONUS)
                value = GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_GOLD_PER) == true ? 1 : 0;
            if (id == BeginnerQuestID.UPGRADE_ATK_SPD_1_5)
                value = ConfigManager.instance.upgraderCtrl.GetStatBase(UpgraderID.attack_speed);
            if (id == BeginnerQuestID.UPGRADE_HEALTH_TO_20)
                value = ConfigManager.instance.upgraderCtrl.GetStatBase(UpgraderID.health);
            if (id == BeginnerQuestID.LOGIN_2_DAYS)
                value = GameDatas.CountDaysLogin;
            if (id == BeginnerQuestID.REMAIN_30M_LOGIN)
                value = (float)(DateTime.Now - GameDatas.timeStart_30minRemain).TotalMinutes;
            value = Mathf.Round(value);

            if (value >= maxTarget)
                value = maxTarget;
            return value;
        }
    }
    public float maxTarget;
    public bool isDone => currentProgress >= maxTarget;
    public bool isClaim
    {
        get
        {
            return GameDatas.IsClaimBeginnerQuest(id);
        }
        set
        {
            GameDatas.ClaimBeginnerQuest(id, value);
        }
    }
    public int rwdGold;
    public int rwdGem; 
    public int Powerstone;
    public string describe => LanguageManager.GetText(id.ToString());
    public void ClaimReward()
    {
        isClaim = true;
    }
#if UNITY_EDITOR
    [ContextMenu("Change Name")]
    public void ChangeName()
    {
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), id.ToString());
    }
#endif
}
