using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteCtrl : GameMonoBehaviour
{

    private int number_satellite => (int)Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.satellite_number);
    private int current_satellite = 0;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnCallSatellite, InitCountSpawnSatellite);
        StartCoroutine(IE_QueueCallSatellite());
    }

    private bool isSpawn = false;
    public void CallSatellite()
    {
        if(!GPm.Tower.gameObject.activeSelf) return;
        Object_Pool obj = PoolCtrl.instance.Get(PoolTag.SATELLITE, GPm.Tower.transform.position, Quaternion.identity);
        current_satellite += 1;
    }

    private void InitCountSpawnSatellite(object o)
    {
        current_satellite -= 1;
    }

    IEnumerator IE_QueueCallSatellite()
    {
        while (true)
        {
            yield return Yielders.Get(0.75f);
            yield return new WaitUntil(() => current_satellite < number_satellite);
            //CallSatellite();
            if (GPm.Tower.gameObject.activeSelf)
            {
                Object_Pool obj = PoolCtrl.instance.Get(PoolTag.SATELLITE, GPm.Tower.transform.position, Quaternion.identity);
                yield return new WaitForEndOfFrame();
                current_satellite += 1;
            }
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnCallSatellite, InitCountSpawnSatellite);
    }
}
