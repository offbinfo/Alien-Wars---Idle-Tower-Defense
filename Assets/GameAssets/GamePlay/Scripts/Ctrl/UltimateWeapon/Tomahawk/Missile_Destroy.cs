using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_Destroy : Object_Destroy
{

    private Object_Pool obj_pool;
    private void Awake()
    {
        obj_pool = GetComponent<Object_Pool>();
    }

    public override void Destroy(Damager d)
    {
        PoolCtrl.instance.Get(PoolTag.EXPLOSIVE1, transform.position, Quaternion.identity);
        obj_pool.ReturnPool();
        EventDispatcher.PostEvent(EventID.OnMissileDestroy, null);
    }
}
