using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ZicZac : GameMonoBehaviour
{

    [SerializeField] float speedMove = 10f;
    [SerializeField] float speedRotate = 5f;
    [SerializeField] float range = 0.1f;

    Action OnReachTarget;
    float t;
    Vector3 target;
    Vector3 direction
    {
        get
        {
            return (target - transform.position).normalized;
        }
    }
    Vector3 directionNoNormalized => target - transform.position;
    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
    public void AddEventOnReachTarget(Action action)
    {
        OnReachTarget += action;
    }
    public void RemoveEventOnReachTarget(Action action)
    {
        OnReachTarget -= action;
    }
    private void Update()
    {
        if (target == null)
            return;
        if (directionNoNormalized.sqrMagnitude <= 0.1f * 0.1f)
            OnReachTarget?.Invoke();

        var per = new Vector3(-direction.y, direction.x) * Mathf.Sin(t) * range; // vuông góc

        transform.position += direction * Time.deltaTime * speedMove + per;

        transform.up = direction + per;
        t += Time.deltaTime * speedRotate;
    }
}
