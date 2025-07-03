using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandCtrl : Singleton<TouchHandCtrl>
{
    private float rotationSpeed = 1000f;
    private Transform arrow;
    public bool isTouchAttack = false;
    public bool isTouchAttackTutorial = false;
    public Vector2 shootDirection;
    private TowerCtrl towerCtrl;

    private float holdTime = 0.5f; // Thời gian giữ cần thiết
    private float holdTimer = 0f;
    private bool isHolding = false;

    void Start()
    {
        towerCtrl = TowerCtrl.instance;
        arrow = towerCtrl.arrow.transform;
    }

    void Update()
    {
        if (!GameDatas.isTutBuildPhase2) return;

        // Kiểm tra nếu chuột hoặc cảm ứng đang trên UI
        if (IsPointerOverUI()) return;

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            isTouchAttackTutorial = true;
            if (!isHolding)
            {
                isHolding = true;
                holdTimer = 0f;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                towerCtrl.iconArrow.SetActive(true);
                isTouchAttack = true;

                Vector3 touchPos = Input.mousePosition;
                if (Input.touchCount > 0)
                {
                    touchPos = Input.GetTouch(0).position;
                }

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                worldPos.z = 0;

                Vector3 direction = worldPos - arrow.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

                float newAngle = Mathf.MoveTowardsAngle(arrow.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
                arrow.rotation = Quaternion.Euler(0, 0, newAngle);
                shootDirection = arrow.transform.up;
            }
        }
        else
        {
            isTouchAttack = false;
            isHolding = false;
            holdTimer = 0f;

            if (towerCtrl.iconArrow.activeSelf)
            {
                towerCtrl.iconArrow.SetActive(false);
            }
        }
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return true;

        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return true;
        }

        return false;
    }
}
