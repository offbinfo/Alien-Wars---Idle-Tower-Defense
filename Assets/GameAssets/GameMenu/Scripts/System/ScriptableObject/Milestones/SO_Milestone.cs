using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MilestonesData",menuName = "Milestones")]
public class SO_Milestone : ScriptableObject
{
    public WorldType typeWorld;
    public List<RwdAll> milestonesReward;
    private Dictionary<int, RwdAll> dic_milestones = new Dictionary<int, RwdAll>();

    public RwdAll GetDataByWave(int wave)
    {
        if (dic_milestones.Count == 0)
        {
            // load dic
            foreach (var m in milestonesReward)
            {
                dic_milestones.Add(m.wave,m);
            }
            Debug.Log("NULL DIC");
        }

        return dic_milestones[wave];
    }
}
[System.Serializable]
public class RwdAll
{
    public int wave;
    public MilestoneReward standard;
    public MilestoneReward premium;
}

[System.Serializable]
public class MilestoneReward
{
    public int amount;
    public TypeMilestone typeMilestone;
    [ShowIf(nameof(IsUpgraderCategory))]
    public UpgraderCategory upgraderCategory;
    [ShowIf(nameof(IsLabCategory))]
    public LabCategory labCategory;
    [ShowIf(nameof(IsUltimateWeapon))]
    public UW_ID uW_ID;

    private bool IsUpgraderCategory() => typeMilestone == TypeMilestone.UNLOCK_UPGRADER_INFOR;
    private bool IsUltimateWeapon() => typeMilestone == TypeMilestone.UNLOCK_ULTIMATE_WEAPON;
    private bool IsLabCategory() => typeMilestone == TypeMilestone.UNLOCK_LAB_INFOR;
}
