using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCtrl : GameMonoBehaviour
{
    [SerializeField]
    private BotChallengeManager datas;

    public BotChallengeManager BotManager => datas;

    public int GetCountUW()
    {
        return datas.bots.Count;
    }
}
