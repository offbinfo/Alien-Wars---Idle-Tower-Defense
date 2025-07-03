using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidNexusTower : Singleton<VoidNexusTower>
{
    [SerializeField] UW_ID ID;
    [SerializeField]
    private Object_DataInformation _dataInformation;
    private Vector3 lastBombDirection = Vector3.zero;
    private float totalVoid = 1;
    private int curSpawnVoid = 0;

    private float countdownTime;
    private float currentTime;
    private float durationTime;
    private float curDurationTime;
    public bool isActiveVoidNexus;

    private float sizeVoidNexus;

    private List<Object_Pool> nexusPrefabs = new();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        var data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_PassiveDuration>(ID);
        currentTime = data.GetCurrentCooldown();
        countdownTime = data.GetCurrentCooldown();

        durationTime = data.GetCurrentDuration();
        curDurationTime = data.GetCurrentDuration();

        sizeVoidNexus = data.GetCurrentSize() / 10;

        float bonusVoid = Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.EXTRA_VOID_NEXUS).GetCurrentProperty();
        totalVoid += bonusVoid;

        DebugCustom.LogColor("Total Void: " + bonusVoid);
    }

    void Update()
    {
        if (!GameDatas.isUnlockUltimateWeapon(ID))
            return;
        ThrowVoidNexus();
    }

    private void ThrowVoidNexus()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if (curSpawnVoid < totalVoid)
            {
                SpawnVoid();
            }

            if (curDurationTime > 0)
            {
                isActiveVoidNexus = true;
                curDurationTime -= Time.deltaTime;
            }
            else
            {
                isActiveVoidNexus = false;
                curSpawnVoid = 0;
                currentTime = countdownTime; //reset
                curDurationTime = durationTime; //reset
                foreach(Object_Pool pool in nexusPrefabs)
                {
                    pool.ReturnPool();
                }
            }
        }
    }

    public void SpawnVoid()
    {
        float range = Random.Range(_dataInformation.attackRange, _dataInformation.attackRange);

        Vector3 posVoid;

        if (lastBombDirection == Vector3.zero)
        {
            posVoid = Extensions.GetRandomPosition(range);
            lastBombDirection = posVoid;
        }
        else
        {
            posVoid = -lastBombDirection;
            lastBombDirection = Vector3.zero;
        }

        curSpawnVoid++;
        var voidNexus = PoolCtrl.instance.Get(PoolTag.VOID_NEXUS, transform.position, Quaternion.identity);
        voidNexus.GetComponent<I_Nexus_Fly>()?.Fly(transform.position + posVoid, sizeVoidNexus);
        nexusPrefabs.Add(voidNexus);
    }

}
