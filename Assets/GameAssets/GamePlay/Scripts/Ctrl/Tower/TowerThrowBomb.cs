using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerThrowBomb : GameMonoBehaviour
{
    int bomb_number => (int)Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bomb_number);
    private int bomb_count_ingame = 0;
    private Object_DataInformation data;

    private void Awake()
    {
        data = GetComponent<Object_DataInformation>();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnBombExplode, OnBombExplode);
        StartCoroutine(IE_ThrowBomb());
    }

    private void OnBombExplode(object o)
    {
        bomb_count_ingame -= 1;
    }

    private IEnumerator IE_ThrowBomb()
    {
        while (true)
        {
            yield return new WaitUntil(() => bomb_count_ingame < bomb_number);
            yield return Yielders.Get(Random.Range(1.5f, 3f));
            // ThrowBomb();
            bomb_count_ingame += 1;
            var posBomb = Extensions.GetRandomPosition(Random.Range(0.6f, data.attackRange));
            var bomb = PoolCtrl.instance.Get(PoolTag.BOMB, transform.position, Quaternion.identity);
            bomb.GetComponent<I_Bomb_Fly>()?.Fly(transform.position + posBomb);
            yield return new WaitForEndOfFrame();
        }
    }

    private void ThrowBomb()
    {
        bomb_count_ingame += 1;
        //throw bomb trong bán kính của rangeATK
        var posBomb = Extensions.GetRandomPosition(Random.Range(0.6f, data.attackRange));
        //call bomb từ pool lên và cho bay ra 
        var bomb = PoolCtrl.instance.Get(PoolTag.BOMB, transform.position, Quaternion.identity);
        bomb.GetComponent<I_Bomb_Fly>()?.Fly(transform.position + posBomb);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnBombExplode, OnBombExplode);
    }
}
