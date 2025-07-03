using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircrafTower : MonoBehaviour
{
    [SerializeField]
    [SerializedDictionary("type bot", "Bot")]
    private SerializedDictionary<TypeBot, GameObject> bots = new();
    TypeBot[] botTypes = {
            TypeBot.FireAircaft,
            TypeBot.ThunderAircaft,
            TypeBot.GoldenAircaft,
            TypeBot.SupportAircaft
    };

    private void Awake()
    {
        ActiveBot(null);
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnBotChallengeUnlock, ActiveBot);
    }

    private void ActiveBot(object o)
    {
        foreach (var botType in botTypes)
        {
            if (GameDatas.IsBotChallengeUnlock(botType))
            {
                Instantiate(bots[botType]);
            }
        }
    }
}
