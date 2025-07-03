using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Object_TakeDamage : GameMonoBehaviour
{
    protected Object_DataInformation data;
    private Object_Destroy objDestroy;
    public bool isFlagTomahawk;
    public float curHp;

    public virtual void Awake()
    {
        data = GetComponent<Object_DataInformation>();
        objDestroy = GetComponent<Object_Destroy>();
    }

    private void OnEnable()
    {
        isFlagTomahawk = false;
        curHp = data.hpCurrent; 
    }

    public virtual void TakeDamage(Damager damager)
    {
        if(damager.damage > 0)
        {
            PoolCtrl.instance.Get(PoolTag.TEXT_POP, transform.position, Quaternion.identity, damager.damage);
        }
        data.hpCurrent -= damager.damage;
        curHp = data.hpCurrent;

        if (data.hpCurrent <= 0)
        {
            objDestroy.Destroy(damager);
        }
    }
}
