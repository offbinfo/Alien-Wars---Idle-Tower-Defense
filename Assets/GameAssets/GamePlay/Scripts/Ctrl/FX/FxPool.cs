using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPool : Object_Pool
{
    [SerializeField]
    private float timeReturnPool;
    [SerializeField]
    private GameObject objfx;
    private float countTime;

    void OnEnable()
    {
        countTime = timeReturnPool;
        if (objfx != null) objfx.SetActive(true);
    }
    private void Update()
    {
        EnqueueObject();
    }

    private void EnqueueObject()
    {
        countTime -= Time.deltaTime;
        if (countTime <= 0)
        {
            PoolCtrl.instance.Return(this);
        }
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }*/
}
