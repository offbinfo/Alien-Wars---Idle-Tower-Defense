using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemMissionChallenge : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtNameMission;
    [SerializeField]
    private TMP_Text txtRankMission;
    [SerializeField]
    private TMP_Text txtBadges;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text txtSlider;
    [SerializeField]
    private GameObject panelCompleted;
    [SerializeField]
    private GameObject panelClaim;

    public int[] badges = {10, 15, 20};
    private MissionChallengeType missionChallengeType;
    private MissionChallengeData missionChallengeData;
    private int indexRank;

    private float progressMission;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefreshClaimEventChallenge, OnRefreshClaimEventChallenge);
        EventDispatcher.AddEvent(EventID.OnX2BadgesEvent, OnX2BadgesEvent);
    }

    private void OnX2BadgesEvent(object obj)
    {
        SetData(missionChallengeData);
    }

    private void OnRefreshClaimEventChallenge(object obj)
    {
        MissionChallengeType type = (MissionChallengeType)obj;

        if (type == missionChallengeType)
        {
            SetProgressMission(missionChallengeData);
        }
    }

    public void SetData(MissionChallengeData data)
    {
        missionChallengeType = data.type;
        int indexRank = GameDatas.GetIndexRankMissionChallenge(missionChallengeType);

        missionChallengeData = data;
        txtNameMission.text = LanguageManager.GetText(missionChallengeType.ToString());
        txtRankMission.text = indexRank +"/3";
        int badgesBonus = badges[indexRank - 1] * (GameDatas.IsX2BadgesEvent() ? 2 : 1);
        txtBadges.text = badgesBonus.ToString();

        SetProgressMission(missionChallengeData);
    }

    private void OnRefreshData(MissionChallengeData data)
    {
        txtRankMission.text = indexRank + "/3";
        txtBadges.text = badges[indexRank - 1].ToString();
        CheckItemMissionCompleted();
    }

    private void SetProgressMission(MissionChallengeData data)
    {
        if(data.type == MissionChallengeType.UpgradeLabTotalDays)
        {
            float totalDays = GameDatas.TotalTimeUpgradeLab / 86400;
            progressMission = Mathf.FloorToInt(totalDays);
        }
        else
        {
            progressMission = GameDatas.GetProgressMissionChallenge(data.type);
        }
        int indexRank = GameDatas.GetIndexRankMissionChallenge(missionChallengeType);
        txtSlider.text = progressMission +
            "/" + data.missions[indexRank - 1];

        slider.maxValue = data.missions[indexRank - 1];
        slider.value = progressMission;

        CheckItemMissionCompleted();
        CheckItemMissionClaim();
    }

    private void OnEnable()
    {
        if (missionChallengeData == null) return;
        SetProgressMission(missionChallengeData);
    }

    public void Claim()
    {
        GameDatas.SetIndexRankMissionChallenge(missionChallengeType, indexRank + 1);
        GameDatas.SetProgressMissionChallenge(missionChallengeType, 0);

        int badgesBonus = badges[indexRank - 1] * (GameDatas.IsX2BadgesEvent() ? 2 : 1);
        GameDatas.Badges += badgesBonus;
        panelClaim.SetActive(false);
        var TargetUIBadges = TopUI_Currency_Horizontal.instance != null ? TopUI_Currency_Horizontal.instance.badges.transform.position : CurrencyContainer.instance._trans_badges.position;
        ObjectUI_Fly_Manager.instance.Get(10, transform.position, TargetUIBadges, CurrencyType.BADGES);

        //GameDatas.SetProgressMissionChallenge(missionChallengeType, 0);
        /*OnRefreshData(missionChallengeData);*/
        SetData(missionChallengeData);
    }

    private void CheckItemMissionCompleted()
    {
        indexRank = GameDatas.GetIndexRankMissionChallenge(missionChallengeType);
        if (indexRank > 3)
        {
            panelCompleted.SetActive(true);
        }
    }

    private void CheckItemMissionClaim()
    {
        if (progressMission >= slider.maxValue)
        {
            panelClaim.SetActive(true);
        }
        else
        {
            panelClaim.SetActive(false);
        }
    }

}
