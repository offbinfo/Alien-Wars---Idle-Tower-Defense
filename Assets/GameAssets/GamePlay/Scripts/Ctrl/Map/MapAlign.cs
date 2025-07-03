using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAlign : MonoBehaviour
{
    [SerializeField] RectTransform left;
    [SerializeField] RectTransform right;
    [SerializeField] Transform bgLeft;
    [SerializeField] Transform bgRight;
    private Camera cam;
    private bool isTransitioning;
    public float scale = 12;

    private float startSize, targetSize, duration, elapsed;
    private Vector3 startLeft, leftTarget;
    private Vector3 startRight, rightTarget;
    private float defaultScale = 5f;
    private float defaultAttackRange = 3f;
    private float defaultOrthographicSize = 12;
    private float maxOrthographicSize = 20f;
    private float maxAttackRange = 7.8f;
    private float defaultTowerScale = 0.7f;
    private float defaultTowerColScale = 0.6f;
    private float maxTowerScale = 1f;

    private void Awake()
    {
        cam = CameraController.instance.mainCam;
        bgLeft.position = left.transform.position;
        bgRight.position = right.transform.position;
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnScaleMapByRangedTower, OnScaleMap);
        ScaleDefault();
    }

    private void ScaleDefault()
    {
        OnScaleMap(UpgraderID.attack_range);
        StartCoroutine(SimulateClicks());
    }

    private void OnScaleMap(object obj)
    {
        UpgraderID upgraderID = (UpgraderID)obj;
        if(upgraderID == UpgraderID.attack_range)
        {
            var tower = TowerCtrl.instance;
            float attackRange = tower.TowerData.attackRange;

            float t = Mathf.InverseLerp(defaultAttackRange, maxAttackRange, attackRange);

            float newCameraSize = Mathf.Lerp(defaultOrthographicSize, maxOrthographicSize, t);
            float towerScale = Mathf.Lerp(defaultTowerScale, maxTowerScale, t);
            float scaleCol = Mathf.Lerp(defaultTowerColScale, maxTowerScale, t);

            tower.bodyTower.localScale = Vector3.one * towerScale;
            tower.col.radius = scaleCol;

            scale = newCameraSize;
            DebugCustom.LogColor("scale " + scale);

            StartCoroutine(SimulateClicks());
        }
    }

    private IEnumerator SimulateClicks()
    {
        for (int i = 0; i < 5; i++)
        {
            SmoothTransition(scale, left.transform, right.transform);
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    public void SmoothTransition(float size, Transform left, Transform right, float time = 0.5f)
    {
        startSize = cam.orthographicSize;
        targetSize = size;

        startLeft = bgLeft.position;
        leftTarget = left.position;

        startRight = bgRight.position;
        rightTarget = right.position;

        duration = time;
        elapsed = 0f;
        isTransitioning = true;
    }

    private void Update()
    {
        if (!isTransitioning) return;

        Transition();
    }

    private void Transition()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
        bgLeft.position = Vector3.Lerp(startLeft, leftTarget, t);
        bgRight.position = Vector3.Lerp(startRight, rightTarget, t);

        if (t >= 1f)
        {
            cam.orthographicSize = targetSize;
            bgLeft.position = leftTarget;
            bgRight.position = rightTarget;
            isTransitioning = false;
        }
    }


}
