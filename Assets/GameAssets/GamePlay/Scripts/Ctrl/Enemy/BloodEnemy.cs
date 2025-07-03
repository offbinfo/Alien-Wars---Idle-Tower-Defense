using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEnemy : MonoBehaviour
{
    [SerializeField]
    private Object_Pool pool;
    private Tween returnTween;

    private void Awake()
    {
        pool = GetComponent<Object_Pool>();
    }

    private void OnEnable()
    {
        returnTween = DOVirtual.DelayedCall(1.5f, () =>
        {
            pool.ReturnPool();
        });
    }

    private void OnDisable()
    {
        if (returnTween != null && returnTween.IsActive())
            returnTween.Kill();
    }
}
