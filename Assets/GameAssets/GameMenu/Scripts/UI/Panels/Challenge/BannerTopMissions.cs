using AYellowpaper.SerializedCollections;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerTopMissions : GameMonoBehaviour
{
    [SerializeField]
    private Slider progress;
    private int maxBadges = 500;
    [SerializeField]
    private TMP_Text txtBadges;
    [SerializeField]
    private RelicManagerSO relicManager;

    [SerializeField]
    private SerializedDictionary<int, RelicRewardElement> relicsReward = new();
    [SerializeField]
    private List<RelicRewardElement> relicRewardElements;

    private void Start()
    {
        progress.maxValue = maxBadges;
        progress.value = GameDatas.Badges;
        EventDispatcher.AddEvent(EventID.OnBadgesChanged, OnBadgesChanged);
        OnBadgesChanged(null);

        BuildData();
    }

    private void ChangeTxtBadges()
    {
        txtBadges.text = Extensions.FormatNumber(GameDatas.Badges);
    }

    private void BuildData()
    {
        AllChallengeData data = GameDatas.LoadAllRelicItems();
        if (data.items == null) return;
        if (data.items.Count > 0)
        {
            gameObject.SetActive(true);
            relicRewardElements[0].Setup(data.items[0], relicManager.GetIconRelicData(data.items[0].typeRelic));
            relicRewardElements[1].Setup(data.items[1], relicManager.GetIconRelicData(data.items[1].typeRelic));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnBadgesChanged(object obj)
    {
        progress.value = GameDatas.Badges;
        ChangeTxtBadges();
        foreach (int key in relicsReward.Keys)
        {
            if (progress.value >= key)
            {
                relicsReward[key].isClaim = true;
            }
            else
            {
                break;
            }
        }
    }
}
