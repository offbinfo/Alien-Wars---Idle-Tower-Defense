using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiForceTower : GameMonoBehaviour
{
    [SerializeField] UW_ID ID;
    [SerializeField]
    private Object_DataInformation dataInformation;
    private float countdownTime;
    private float currentTime;
    private float durationTime;
    private float curDurationTime;
    [SerializeField]
    private GameObject slowForce;
    private float changeSlow;
    private float slowPower = 50f;
    public float reduction;
    private bool isActiveAntiForce;
    public bool forceDuration => Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.FORCE_REDUCTION).GetCurrentProperty() > 0 ? true : false;
    private float rangeAnti;
    [SerializeField]
    private TowerCtrl towerCtrl;

    private void Start()
    {
        Init();
    }

    public float DecreaseDmgByAntiForce(float damage)
    {
        if(isActiveAntiForce && forceDuration)
        {
            damage *= reduction;
        }
        return damage;
    }

    private void Init()
    {
        var data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_PassiveSlow>(ID);

        currentTime = data.GetCurrentCooldown();
        countdownTime = data.GetCurrentCooldown();

        float durationBonus = Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.ANTI_FORCE_DURATION).GetCurrentProperty();
        durationTime = data.GetCurrentDuration() + durationBonus;
        curDurationTime = data.GetCurrentDuration() + durationBonus;

        float range = Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.ANTI_FORCE_RANGE).GetCurrentProperty();

        rangeAnti += (towerCtrl.TowerData.attackRange + 10 + range);
        slowForce.transform.localScale = new Vector3(rangeAnti, rangeAnti, 1);

        reduction += Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.ANTI_FORCE_REDUCTION).GetCurrentProperty();

        changeSlow = data.GetCurrentSlowChange();
    }

    void Update()
    {
        if (!GameDatas.isUnlockUltimateWeapon(ID))
            return;
        ActiveAntiForce();
    }

    private void ActiveAntiForce()
    {
        if (currentTime > 0)
        {
            isActiveAntiForce = false;
            slowForce.gameObject.SetActive(false);
            currentTime -= Time.deltaTime;
        }
        else
        {
            isActiveAntiForce = true;
            slowForce.gameObject.SetActive(true);
            if (curDurationTime > 0)
            {
                curDurationTime -= Time.deltaTime;
            }
            else
            {
                isActiveAntiForce = false;
                slowForce.gameObject.SetActive(false);
                currentTime = countdownTime; //reset
                curDurationTime = durationTime; //reset
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!slowForce.gameObject.activeSelf) return;
        if (changeSlow.Chance())
        {
            if (other.CompareTag(GameTags.TAG_ENEMIES))
            {
                other.GetComponent<I_Slow>().Slow(slowPower, 1f);
            }
        }
    }

}
