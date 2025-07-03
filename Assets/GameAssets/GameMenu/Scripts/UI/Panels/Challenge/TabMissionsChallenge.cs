using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TabMissionsChallenge : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private ItemMissionChallenge itemMissionChallenge;
    [SerializeField]
    private MissionChallengeSO MissionChallengeSO;
    private int maxMission = 16;
    [SerializeField]
    private GameObject panelWealComeEvent;
    [SerializeField]
    private RelicManagerSO relicManagerSO;

    private void CheckEventActive()
    {
        DateTime now = DateTime.Now;
        DateTime lastChecked = GameDatas.LastCheckTimeChallenge;

        if (GameDatas.IsNewEventCycle(lastChecked, now))
        {
            StartCoroutine(DelayShowPanelWelcomeEvent());
            GameDatas.ResetEventChallenge();
            //GameDatas.X2BadgesEvent(false);
            ResetDataMissionQuest();
            GameDatas.RandomRelicReward(false);
            DebugCustom.LogColor("Đã sang sự kiện mới");
        }
        if (!GameDatas.IsFirstStartEventChallenge())
        {
            GameDatas.FirstStartEventChallenge(true);
            GameDatas.LastCheckTimeChallenge = now;

            MissionChallengeSO.ShuffledMissionData();
        }

        if (GameDatas.IsDuringEvent(now))
        {
            DebugCustom.LogColor("Đang trong thời gian sự kiện");
        }
        else
        {
            DebugCustom.LogColor("Sự kiện đã kết thúc");
        }
        InitRelicReward();
    }

    private void InitRelicReward()
    {
        DebugCustom.LogColor("GameDatas.IsRandomRelicReward() " + GameDatas.IsRandomRelicReward());
        if (GameDatas.IsRandomRelicReward()) return;
        GameDatas.RandomRelicReward(true);

        var lockedRelics = relicManagerSO.relicDicts
            .Where(kv => !GameDatas.IsRelicUnlock(kv.Key))
            .OrderBy(_ => Random.value) // Shuffle
            .Take(4)
            .Select(kv => kv.Value)
            .ToList();

        GameDatas.SaveAllListRelicItems(lockedRelics);
    }

    private void ResetDataMissionQuest()
    {
        foreach (MissionChallengeType questType in Enum.GetValues(typeof(MissionChallengeType)))
        {
            GameDatas.SetProgressMissionChallenge(questType, 0);
            GameDatas.SetIndexRankMissionChallenge(questType, 1);
        }
    }

    private void Awake()
    {
        CheckEventActive();
    }

    private void Start()
    {
        //CheckEventActive();
        InitData();
    }

    private IEnumerator DelayShowPanelWelcomeEvent()
    {
        yield return null;
        panelWealComeEvent.SetActive(true);
        yield return Yielders.Get(2.5f);
        panelWealComeEvent.SetActive(false);
    }

    private void InitData()
    {
        if (GameDatas.CheckNewDayEventChallenge() && GameDatas.TotalMissionChallenge < maxMission)
        {
            GameDatas.TotalMissionChallenge = Mathf.Min(GameDatas.TotalMissionChallenge + 2, maxMission);
        }

        foreach (MissionChallengeData data in MissionChallengeSO.missionStatic)
        {
            InitMissionView(data);
        }

        for (int i = 0; i < GameDatas.TotalMissionChallenge; i++)
        {
            InitMissionView(MissionChallengeSO.missionDynamic[i]);
        }

    }

    private void InitMissionView(MissionChallengeData data)
    {
        ItemMissionChallenge cellView = Instantiate(itemMissionChallenge, content);
        cellView.SetData(data);
    }
}
