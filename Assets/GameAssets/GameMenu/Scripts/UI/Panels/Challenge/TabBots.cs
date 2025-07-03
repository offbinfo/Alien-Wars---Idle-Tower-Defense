using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabBots : MonoBehaviour
{

    [SerializeField]
    private List<ItemBotChallenge> cellViews = new();
    [SerializeField]
    private BotChallengeManager challengeManager;

    private void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        for (int i = 0; i < challengeManager.bots.Count; i++)
        {
            TypeBot type = (TypeBot)i;
            cellViews[i].SetUp(challengeManager.bots[type]);
        }
    }
}
