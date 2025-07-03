using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SatelliteRotate : GameMonoBehaviour
{

    private Transform center;
    private float speedRotate => UnityEngine.Random.Range(80, 100f);
    private float amplititude => (Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_range) / 10) - 0.6f;
    private float frequency = 0.3f;
    private float distance = 0;
    private float elapseTime;
    private Vector3 temp;
    private Coroutine rotateRoutine;

    private void OnEnable()
    {
        Move();
    }

    private void Update()
    {
        if (!GameDatas.IsCardEquiped(CardID.RARE_SATELLI_SMART)) return;
        if(FindNearestBoss() != null)
        {
            //DebugCustom.LogColor("FindNearestBoss");
            MoveTarget(FindNearestBoss().transform);
        }
    }

    private void Move()
    {
        center = GPm.Tower.transform;
        transform.position = center.position;

        elapseTime = UnityEngine.Random.Range(0, 10);
        var dir = new Vector3(-1, 1, 0);
        var target = center.position + dir.normalized * (distance + amplititude + 0.6f);

        transform.DOMove(target, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (gameObject.activeSelf) rotateRoutine = StartCoroutine(Rotate());
        });
    }

    private IEnumerator Rotate()
    {
        float elapseTime = 0f;
        float angle = 0f;

        while (true)
        {
            elapseTime += Time.deltaTime;

            float minRadius = 0.5f;
            float maxRadius = amplititude + 1.0f;
            float radius = Mathf.Lerp(minRadius, maxRadius, 0.5f + 0.5f * Mathf.Sin(elapseTime * 2f));

            angle += speedRotate * Time.deltaTime;
            if (angle > 360f) angle -= 360f;

            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            transform.position = center.position + offset;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, offset.normalized);

            yield return null;
        }
    }

    public void MoveTarget(Transform target)
    {
        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
            rotateRoutine = null;
        }

        Vector3 enemyPos = target.position;
        transform.DOMove(enemyPos, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Reached Enemy Position");
        });
    }

    [SerializeField] LayerMask enemiesLayer;
    private Collider2D FindNearestBoss()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, enemiesLayer);
        if (colliders.Length == 0) return null;

        Collider2D nearestBoss = null;
        float minDistance = float.MaxValue;
        Vector2 origin = transform.position;

        foreach (var collider in colliders)
        {
            EnemyTakeDamage enemy = collider.GetComponent<EnemyTakeDamage>();
            if (enemy != null && enemy.isBoss)
            {
                float distance = Vector2.SqrMagnitude((Vector2)collider.transform.position - origin);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBoss = collider;
                }
            }
        }

        return nearestBoss;
    }

}
