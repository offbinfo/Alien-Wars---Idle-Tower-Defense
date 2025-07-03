using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : Object_Move
{

    private Object_Pool _objPool;
    private Vector3 _originalPos;
    public override void Awake()
    {
        base.Awake();
        _objPool = GetComponent<Object_Pool>();
        _objPool.AddEventInit((o) => {
            _originalPos = transform.position;
        });
    }

    public void Update()
    {
        Move();
        BulletLimited();
    }
    public virtual void Move()
    {
        transform.position += directionMove.normalized * Time.deltaTime * speedMove;
    }

    private void BulletLimited()
    {
        if (Vector3.Distance(_originalPos, transform.position) >= 40f)
        {
            PoolCtrl.instance.Return(_objPool);
        }
    }
}
