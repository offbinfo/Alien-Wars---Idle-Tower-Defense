using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GoldenSanctuary : Singleton<GoldenSanctuary>
{
    [SerializeField] UW_ID ID;
    private float countdownTime;
    private float currentTime;
    private float durationTime;
    private float curDurationTime;
    [SerializeField]
    private LightGolden lightGolden;
    [SerializeField]
    private TowerCtrl towerCtrl;
    public bool isCastGolden;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        var data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_PassiveDuration>(ID);
        currentTime = data.GetCurrentCooldown();
        countdownTime = data.GetCurrentCooldown();

        float durationBonus = Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.GOLDEN_SANCTUARY_DURATION).GetCurrentProperty();
        durationTime = data.GetCurrentDuration() + durationBonus;
        curDurationTime = data.GetCurrentDuration() + durationBonus;

        float ranged = towerCtrl.TowerData.attackRange + 5f + data.GetCurrentQuantity();
        lightGolden.SetUp(ranged);
    }

    void Update()
    {
        if (!GameDatas.isUnlockUltimateWeapon(ID))
            return;
        ActiveGoldenSanctuary();
    }

    private void ActiveGoldenSanctuary()
    {
        if (currentTime > 0)
        {
            isCastGolden = false;
            lightGolden.gameObject.SetActive(false);
            currentTime -= Time.deltaTime;
        }
        else
        {
            isCastGolden = true;
            lightGolden.gameObject.SetActive(true);
            if (curDurationTime > 0)
            {
                curDurationTime -= Time.deltaTime;
            }
            else
            {
                isCastGolden = false;
                lightGolden.gameObject.SetActive(false);
                currentTime = countdownTime; //reset
                curDurationTime = durationTime; //reset
            }
        }
    }
}
