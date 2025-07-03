using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BotChallengeManager", menuName = "Data/Bots/BotChallengeManager", order = 0)]
public class BotChallengeManager : SerializedScriptableObject
{
    public Dictionary<TypeBot, SO_Bot_Base> bots = new();

    public T GetDataById<T>(TypeBot id) where T : SO_Bot_Base
    {
        if (bots.ContainsKey(id))
            return bots[id] as T;
        else
            return null;
    }

}
