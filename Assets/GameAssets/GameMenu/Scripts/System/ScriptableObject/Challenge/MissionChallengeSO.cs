using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionChallengeSO", menuName = "Data/Challenge/MissionChallengeSO", order = 0)]
public class MissionChallengeSO : SerializedScriptableObject
{

    public Dictionary<MissionChallengeType, MissionChallengeData> missionChallengeDatas = new();
    public Dictionary<TypeRankChallenge, int> badgesBonus = new();

    public List<MissionChallengeData> missionStatic = new();
    public List<MissionChallengeData> missionDynamic = new();

/*    [Button("Add Data")]
    public void SplitMissionData()
    {
        missionStatic.Clear();
        missionDynamic.Clear();

        List<MissionChallengeData> dynamicCandidates = new();

        foreach (var data in missionChallengeDatas.Values)
        {
            if (data.missionCategory == MissionCategory.Static)
            {
                missionStatic.Add(data);
            }
            else
            {
                dynamicCandidates.Add(data);
            }
        }

        var shuffled = dynamicCandidates.OrderBy(x => UnityEngine.Random.value).ToList();

        missionDynamic = shuffled.Take(15).ToList();
    }*/

    //[Button("Shuffled Data")]
    public void ShuffledMissionData()
    {
        List<MissionChallengeData> dynamicCandidates = missionDynamic;
        var shuffled = dynamicCandidates.OrderBy(x => UnityEngine.Random.value).ToList();

        missionDynamic = shuffled.ToList();
    }


}

public class MissionChallengeData
{
    public MissionChallengeType type;
    public List<int> missions;
    public MissionCategory missionCategory;

    public MissionChallengeData(List<int> missions, MissionCategory category)
    {
        this.missions = missions;
        this.missionCategory = category;
    }
}