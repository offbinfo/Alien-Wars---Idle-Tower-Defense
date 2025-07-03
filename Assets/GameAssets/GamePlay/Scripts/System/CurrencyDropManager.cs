using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDropManager : GameMonoBehaviour
{
/*    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnEnemyKilled, DropCoin);
        //EventDispatcher.AddEvent(EventID.EnemyDestroyedByShockwave, DropGold);
    }*/

    /*public void DropCoinOrSliver(object o, double gold, double sliver)
    {
        var pos = (Vector3)o;
        //silver
        var coinPerKill = Cfg.labCtrl.LapManager.GetSingleSubjectById
            (IdSubjectType.COIN_PER_KILL).GetCurrentProperty();

        var value = 1 + (int)coinPerKill;
        GPm.SliverInGame += value;
        var posSilver = pos + Extensions.GetRandomPosition(0.5f);
        var objPool = PoolCtrl.instance.Get(PoolTag.CURRENCY_SLIVER, posSilver, Quaternion.identity, "+" + value);

        //GameDatas.SetDataProfile(IDInfo.TotalCoinsEarned, GameDatas.GetDataProfile(IDInfo.TotalCoinsEarned) + 1);
        //gold
        if (70f.Chance())
        {
            var posGold = pos + Extensions.GetRandomPosition(0.5f);
            var amount = (1 + GameDatas.CurrentWorld) * GameDatas.x_sum;
            PoolCtrl.instance.Get(PoolTag.CURRENCY_GOLD, posGold, Quaternion.identity, "+" + amount);

            GPm.GoldInGame += (int)amount;
            GameDatas.Gold += (int)amount;
        }
    }

    public void DropGold(object o)
    {
        var pos = (Vector3)o;
        var posGold = pos + Extensions.GetRandomPosition(0.5f);
        var amount = 1;
        PoolCtrl.instance.Get(PoolTag.CURRENCY_GOLD, posGold, Quaternion.identity, "+" + amount);

        GPm.GoldInGame += (int)amount;
        GameDatas.Gold += (int)amount;
    }*/
}
