using Cinemachine.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpperCtrl : GameMonoBehaviour
{
    [SerializeField]
    private TypeMonster typeMonster;
    Object_DataInformation objData;
    private Object_Move _object_Move;

    private bool isCharging = false;
    private float chargeTime = 20f;
    [SerializeField]
    private LazePhoton lazePhoton;
    public int splitLevel = 0;
    public int maxSplits = 4;

    public GameObject splitPrefab;

    protected virtual Damager damager
    {
        get
        {
            var dm = new Damager();
            dm.damage = objData.damage;
            dm.type = DamageType.NORMAL;
            dm.objAttack = gameObject;
            return dm;
        }
    }

    private void Awake()
    {
        objData = GetComponent<Object_DataInformation>();
        _object_Move = GetComponent<Object_Move>();
    }

    private Coroutine regenCoroutine;

    private void OnEnable()
    {
        ActiveSkill();
    }

    private void OnDisable()
    {
        if (regenCoroutine != null)
        {
            GPm.isNoRegenHPTower = false;
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    public bool IsTrySplit()
    {
        return splitLevel < maxSplits;
    }

    [Button("Test TrySplit")]
    public void TrySplit()
    {
        if (splitLevel >= maxSplits)
            return;

        for (int i = 0; i < 2; i++) 
        {
            GameObject clone = Instantiate(splitPrefab, transform.position + Random.insideUnitSphere, Quaternion.identity);

            EnemyUpperCtrl splitter = clone.GetComponent<EnemyUpperCtrl>();
            splitter.splitLevel = this.splitLevel + 1;
            splitter.maxSplits = this.maxSplits;
            objData.maxHP = objData.hpCurrent / 2f;
            objData.hpCurrent = objData.hpCurrent / 2f;
            clone.transform.localScale = transform.localScale * 0.9f;
        }

        //Destroy(gameObject); // Hủy bản gốc sau khi tách
    }

    private void ActiveSkill()
    {
        switch (typeMonster)
        {
            case TypeMonster.Crimson:
                if (regenCoroutine == null)
                {
                    GPm.isNoRegenHPTower = true;
                    regenCoroutine = StartCoroutine(IE_Regen());
                }
                break;
            case TypeMonster.Photon:
                if (!isCharging)
                {
                    damager.damage *= 3f;
                    lazePhoton.GetComponent<I_BulletEnemy>().SetUp(damager);
                    StartCoroutine(ChargeAndFire());
                }
                break;
            case TypeMonster.Ripple:
                break;
        }
    }

    private IEnumerator ChargeAndFire()
    {
        isCharging = true;

        yield return new WaitForSeconds(chargeTime);

        lazePhoton.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        lazePhoton.gameObject.SetActive(false);

        isCharging = false;
    }

    private void FirePhotonBeam()
    {

    }

    IEnumerator IE_Regen()
    {
        while (true)
        {
            yield return new WaitUntil(() => objData.hpCurrent < objData.maxHP);
            yield return new WaitForSeconds(1f);

            float regenAmount = objData.maxHP * 0.02f;
            objData.hpCurrent = Mathf.Min(objData.hpCurrent + regenAmount, objData.maxHP);
        }
    }

}
