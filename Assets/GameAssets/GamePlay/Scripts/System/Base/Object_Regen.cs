using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Regen : GameMonoBehaviour
{
    Object_DataInformation objData;
    private void Awake()
    {
        objData = GetComponent<Object_DataInformation>();
    }

    private void Start()
    {
        StartCoroutine(IE_Regen());
        EventDispatcher.AddEvent(EventID.OnReviveTower, OnRevive);
    }

    private void OnRevive(object o)
    {
        StartCoroutine(IE_Regen());
    }

    IEnumerator IE_Regen()
    {
        while (true)
        {
            yield return new WaitUntil(() => objData.hpCurrent < objData.maxHP);
            yield return new WaitForSeconds(1f);

            if (!GPm.isNoRegenHPTower)
            {
                objData.hpCurrent = Mathf.Min(objData.hpCurrent + objData.regen_speed, objData.maxHP);
            }
        }
    }
}
