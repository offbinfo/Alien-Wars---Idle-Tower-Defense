using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Object_Attack
{
    private TowerCtrl _tower;
    private Object_DataInformation _dataTower;
    private EnemyData enemyData;
    private Object_TakeDamage _takeDamage;
    private Object_Move _object_Move;
    public EnemyAttackType enemyAttackType;
    public TypeMonster typeMonster;
    float timeAttack => 1f / attackSpeed;
    private float photonCharge = 0f;
    private float photonChargeDuration = 20f;

    private float spacingFactorTower = 0.6f;

    public override void Awake()
    {
        base.Awake();
        enemyData = GetComponent<EnemyData>();
        _object_Move = GetComponent<Object_Move>();
    }

    private void Start()
    {
        _tower = GPm.Tower;
        enemyAttackType = enemyData.EnemyAttackType;
        typeMonster = enemyData.typeMonster;
    }

    protected override bool canAttack
    {
        get
        {
            if(_dataTower == null)
            {
                _dataTower = _tower.GetComponent<Object_DataInformation>();
            }
            if (enemyAttackType == EnemyAttackType.Ranged)
            {
                return base.canAttack && _dataTower.hpCurrent > 0
                        && Vector3.Distance(_tower.transform.position, transform.position) <= _tower.TowerData.attackRange;
            } else
            {
                return base.canAttack && _dataTower.hpCurrent > 0
                        && Vector3.Distance(_tower.transform.position, transform.position) 
                        <= objData.attackRange + spacingFactorTower;
            }
        }
    }

    public override void Attack()
    {
        if (_tower == null) return;
        switch(enemyAttackType)
        {
            case EnemyAttackType.Melee:
                AttackMelee();
                break;
            case EnemyAttackType.Ranged:
                AttackRanged();
                break;
            default:
                break;
        }
    }

    private void AttackMelee()
    {
        _tower.GetComponent<Object_TakeDamage>()?.TakeDamage(damager);
    }

    private void AttackRanged()
    {
        _object_Move.isNoMove = true;
        if (typeMonster == TypeMonster.Ranged)
        {
            var directionFire = (_tower.transform.position - transform.position).normalized;
            var bullet = PoolCtrl.instance.Get(PoolTag.BULLET_ENEMY0, transform.position + directionFire * 0.5f, Quaternion.identity);

            bullet.GetComponent<I_BulletEnemy>().SetUp(damager);
            bullet.GetComponent<Object_Move>().SetDirectionMove(directionFire);
        }
    }
}
