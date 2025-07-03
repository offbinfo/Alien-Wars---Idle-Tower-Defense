using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : Object_Destroy
{

    Object_Pool objPool;
    private void Awake()
    {
        objPool = GetComponent<Object_Pool>();
    }
    public override void Destroy(Damager d)
    {
        PoolCtrl.instance.Return(objPool);
    }
}
