using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MileStoneManager", menuName = "Data/MileStoneManager")]
public class SO_MileStoneManager : SerializedScriptableObject
{

    public Dictionary<WorldType ,SO_Milestone> milestones = new();

    public List<SO_Milestone> GetMilestonesValuesAsList()
    {
        return milestones.Values.ToList();
    }

}
