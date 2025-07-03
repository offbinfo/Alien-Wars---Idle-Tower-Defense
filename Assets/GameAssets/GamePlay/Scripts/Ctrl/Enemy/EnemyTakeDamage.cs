using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : Object_TakeDamage, ICastedLight
{
    public bool isBoss = false;
    public bool isImmortal = false;
    private bool iscastedLight = false;
    private float dmgScale = 1f;
    private EnemyCtrl enemyCtrl;
    private EnemyData enemyData;
    private bool isX2Gold;
    public Object_Pool Pool;
    private bool isGuardianVial;

    private float bonusDamage = 1f;

    public override void Awake()
    {
        base.Awake();
        Pool = GetComponent<Object_Pool>();
        enemyCtrl = GetComponent<EnemyCtrl>();
        enemyData = GetComponent<EnemyData>();  
    }

    private void OnEnable()
    {
        bonusDamage = 1f;
    }

    public void CastedLight(float dmgScale)
    {
        iscastedLight = true;
        this.dmgScale = dmgScale;
    }

    public void OutLight()
    {
        iscastedLight = false;
    }

    public void IncreareGoldBonus(double bonus)
    {
        enemyData.goldBonusVoidNexus += bonus;
    }

    public void IncreareDmgBonus(float bonus)
    {
        if(bonus < 1) return;
        bonusDamage = bonus;
    }

    public override void TakeDamage(Damager damager)
    {
        if (isImmortal && data.hpCurrent <= 0) return;
        if (iscastedLight)
        {
            damager.damage *= dmgScale;
            if(!isX2Gold)
            {
                isX2Gold = true;
                int goldRound = Mathf.FloorToInt((float)(enemyData.goldDrop +
                    Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.HIGHTLIGHT_GOLD_BONUS).GetCurrentProperty()));
                enemyData.goldDrop += goldRound;
            }
        } else
        {
            isX2Gold = false;
            enemyData.goldDrop = enemyData.goldBaseDrop;
        }

        enemyCtrl.hpbar.InActiveHpBar(data.hpCurrent > damager.damage);
        damager.damage *= bonusDamage;

        if(isGuardianVial)
        {
            // giam damage 60% nhan vao khi guardian dc kich hoat
            damager.damage *= 0.4f;
        }

        base.TakeDamage(damager);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_LIGHT_GuardianVeil))
        {
            isGuardianVial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_LIGHT_GuardianVeil))
        {
            isGuardianVial = false;
        }
    }
}
