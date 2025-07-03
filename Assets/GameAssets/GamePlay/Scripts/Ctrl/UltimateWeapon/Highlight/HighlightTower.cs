using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTower : GameMonoBehaviour
{

    [SerializeField] private List<LightUW> lightUWs = new();
    [SerializeField] private UW_ID ID;
    [SerializeField] private float rotateSpd;
    [SerializeField] private int countLight = 0;

    private bool isLightActive = false;
    private float cachedRotateSpeed;

    private void Start()
    {
        CheckLightActive();
    }

    private void CheckLightActive()
    {
        if (!GameDatas.isUnlockUltimateWeapon(UW_ID.HIGHLIGHT))
            return;

        var data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Passive>(ID);
        countLight = Mathf.Min(data.GetCurrentQuantity(), 4);

        if (countLight <= 0)
        {
            isLightActive = false;
            return;
        }

        isLightActive = true;
        float anglePerLight = 360f / countLight;

        for (int i = 0; i < lightUWs.Count; i++)
        {
            bool isActive = i < countLight;
            if (lightUWs[i].gameObject.activeSelf != isActive)
                lightUWs[i].gameObject.SetActive(isActive);

            if (isActive)
                lightUWs[i].transform.rotation = Quaternion.Euler(0, 0, i * anglePerLight);
        }
    }

    private void Update()
    {
        if (!isLightActive)
            return;

        cachedRotateSpeed = rotateSpd * Time.deltaTime;
        transform.Rotate(Vector3.forward, cachedRotateSpeed);
    }
}
