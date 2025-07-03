using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class TowerAttack : Object_Attack
{

    public float dmg => damager.damage;
    protected override float attackSpeed
    {
        get
        {
            var cardStat = Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_ATK_SPD);
            var multiply = 1 + cardStat / 100f;
            return Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_speed) * multiply;
        }
    }
    private float stack_dmg = 0;
    protected override Damager damager
    {
        get
        {
            var damager = new Damager();

            var statcard = Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_DAMAGE);
            var multiply = 1 + (statcard + stack_dmg) / 100f;
            var damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage) * multiply;
            if (criticalChance.Chance())
            {
                damager.damage = damage * criticalPower / 100;
                damager.type = DamageType.CRIT;
                damager.objAttack = gameObject;
                return damager;
            }

            //relic
            damage = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, damage);

            damager.damage = damage;
            damager.type = DamageType.NORMAL;
            damager.objAttack = gameObject;
            return damager;
        }
    }
    float criticalChance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.critical_shot_chance) +
        Cfg.cardCtrl.GetCurrentStat(CardID.DIVINE_CRITICAL_CHANCE);
    float criticalPower
    {
        get
        {
            var cardStat = Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_CRITICAL_DAMAGE);
            var multiply = 1 + cardStat / 100f;
            float criticalShot = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.critical_shot_damage) * multiply;
            return Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.CritFactor, criticalShot);
        }
    }

    float slowPower
    {
        get
        {
            float slowChance = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.slow_chance);
            if (slowChance.Chance())
                return Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.slow_power);
            else return 0;
        }
    }

    float areaDamage => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.range_damage_bonus);
    float doubleShotChance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.double_shot_chance);
    float doubleShotPower => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.double_shot_damage_percent);
    float multiTargetChance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.multi_shot_chance);
    float numberOfTarget => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.multi_shot_target_number);
    float bounceChance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bounce_shot_chance);
    float timeStun => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.stun_time);
    float chanceStun => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.stun_chance);
    float chanceKnockBack => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.knockback_chance);
    //float chanceDeadHit => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.dead_hit_chance);
    float slowTime => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.slow_time);

    [SerializeField] LayerMask enemiesLayer;
    private NativeArray<Vector2> enemyPositions;
    private NativeArray<float> distances;
    private TowerFXParticles towerFXParticles;
    private TouchHandCtrl touchHandCtrl;

    public override void Awake()
    {
        base.Awake();
        towerFXParticles = GetComponent<TowerFXParticles>();
    }

    private void Start()
    {
        touchHandCtrl = TouchHandCtrl.instance;
    }

    public override void Attack()
    {
        if (Gm.GameState == GameState.EndGame) return;

        var target = FindNearestTarget();
        if (target == null) return;
        var limitStack = Cfg.cardCtrl.GetCurrentStat(CardID.DIVINE_STACK_DMG);
        if (stack_dmg < limitStack) stack_dmg += 1;
        //fire
        Fire(target);

        //multi target
        if (multiTargetChance.Chance())
        {
            List<Collider2D> additionalTargets = FindAllTarget(target);
            var targetAvailable = Mathf.Min(numberOfTarget, additionalTargets.Count);
            for (int i = 0; i < targetAvailable; i++)
            {
                Fire(additionalTargets[i], true);
            }
        }
    }

    private void Fire(Collider2D target, bool isMultiTarget = false)
    {
        towerFXParticles.PlayFxAttack();
        Fire_A_Bullet(target, damager, 100
       , !isMultiTarget && bounceChance.Chance());

        if (!isMultiTarget && doubleShotChance.Chance())
            StartCoroutine(I_DelayDoubleShot(target));
    }

    IEnumerator I_DelayDoubleShot(Collider2D target)
    {
        yield return Yielders.Get(0.1f);
        Fire_A_Bullet(target, damager, doubleShotPower, false);
    }

    private void Fire_A_Bullet(Collider2D target, Damager damager, float multiPower, bool isBounce)
    {
        if(touchHandCtrl.isTouchAttack)
        {
            var directionFire = touchHandCtrl.shootDirection;
            SetUpBullet(directionFire, damager, multiPower, isBounce);
        }
        else
        {
            var directionFire = (target.transform.position - transform.position).normalized;
            SetUpBullet(directionFire, damager, multiPower, isBounce);
        }
    }

    private void SetUpBullet(Vector3 directionFire, Damager damager, float multiPower, bool isBounce)
    {
        var bullet = PoolCtrl.instance.Get(PoolTag.BULLET0, transform.position + directionFire * 0.5f, Quaternion.identity);

        float chanceDeadHit = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.dead_hit_chance);
        chanceDeadHit = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, chanceDeadHit);

        bullet.GetComponent<I_Bullet>().SetUp(damager, slowPower, slowTime, areaDamage, multiPower, isBounce, chanceStun.Chance() ? timeStun : 0
        , chanceKnockBack.Chance()
        , chanceDeadHit.Chance());
        bullet.GetComponent<Object_Move>().SetDirectionMove(directionFire);


        var fx = PoolCtrl.instance.Get(PoolTag.BULLET_HIT, transform.position + directionFire * 0.5f, Quaternion.identity);
        fx.transform.up = directionFire;
    }

    private Collider2D FindNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, objData.attackRange, enemiesLayer);
        if (colliders.Length == 0) return null;

        enemyPositions = new NativeArray<Vector2>(colliders.Length, Allocator.TempJob);
        distances = new NativeArray<float>(colliders.Length, Allocator.TempJob);

        for (int i = 0; i < colliders.Length; i++)
        {
            enemyPositions[i] = colliders[i].transform.position;
        }

        FindNearestTargetJob job = new FindNearestTargetJob
        {
            enemyPositions = enemyPositions,
            origin = transform.position,
            distances = distances
        };

        JobHandle handle = job.Schedule(colliders.Length, 1);
        handle.Complete();

        int minIndex = 0;
        float minDistance = distances[0];
        for (int i = 1; i < distances.Length; i++)
        {
            if (distances[i] < minDistance)
            {
                minDistance = distances[i];
                minIndex = i;
            }
        }

        enemyPositions.Dispose();
        distances.Dispose();

        return colliders[minIndex];
    }

    private List<Collider2D> FindAllTarget(Collider2D excludeTarget)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, objData.attackRange, enemiesLayer);
        List<Collider2D> targets = new List<Collider2D>();
        foreach (var col in colliders)
        {
            if (col != excludeTarget)
            {
                targets.Add(col);
            }
        }
        return targets;
    }

    /*public override void Attack()
{
    if (Gm.GameState == GameState.EndGame) return;

    var target = FindNeareastTarget();
    if (target == null) return;
    var limitStack = Cfg.cardCtrl.GetCurrentStat(CardID.DIVINE_STACK_DMG);
    if (stack_dmg < limitStack) stack_dmg += 1;
    //fire
    Fire(target);

    //multi target
    if (multiTargetChance.Chance())
    {
        var listCol = FindAllTarget(target);
        var targetAvailable = Mathf.Min(numberOfTarget, listCol.Count);
        for (int i = 0; i < targetAvailable; i++)
        {
            Fire(listCol[i], true);
        }
    }
}
private void Fire(Collider2D target, bool isMultiTarget = false)
{

    Fire_A_Bullet(target, damager, 100
    , !isMultiTarget && bounceChance.Chance());

    if (!isMultiTarget && doubleShotChance.Chance())
        StartCoroutine(I_DelayDoubleShot(target));
}

IEnumerator I_DelayDoubleShot(Collider2D target)
{
    yield return Yielders.Get(0.1f);
    Fire_A_Bullet(target, damager, doubleShotPower, false);
}

private void Fire_A_Bullet(Collider2D target, Damager damager, float multiPower, bool isBounce)
{
    var directionFire = (target.transform.position - transform.position).normalized;
    var bullet = PoolCtrl.instance.Get(PoolTag.BULLET0, transform.position + directionFire * 0.5f, Quaternion.identity);

    bullet.GetComponent<I_Bullet>().SetUp(damager, slowPower, 1, areaDamage, multiPower, isBounce, chanceStun.Chance() ? timeStun : 0
    , chanceKnockBack.Chance()
    , chanceDeadHit.Chance());
    bullet.GetComponent<Object_Move>().SetDirectionMove(directionFire);


    var fx = PoolCtrl.instance.Get(PoolTag.BULLET_HIT, transform.position + directionFire * 0.5f, Quaternion.identity);
    fx.transform.up = directionFire;
}


protected Collider2D FindNeareastTarget()
{
    //hàm tìm target gần nhất trong 1 khoảng range có sẵn của Object_Data
    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, objData.attackRange, enemiesLayer);

    if (collider2Ds.Length <= 0)
        return null;
    if (collider2Ds.Length <= 1)
        return collider2Ds[0];

    float distanceMin = Vector3.Distance(collider2Ds[0].transform.position, transform.position);
    int minDistanceIndex = 0;
    for (int i = 1; i < collider2Ds.Length; i++)
    {
        if (Vector3.Distance(collider2Ds[i].transform.position, transform.position) <= distanceMin)
        {
            distanceMin = Vector3.Distance(collider2Ds[i].transform.position, transform.position);
            minDistanceIndex = i;
        }
    }
    return collider2Ds[minDistanceIndex];
}

private List<Collider2D> FindAllTarget(Collider2D col2dExcept)
{
    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, objData.attackRange, enemiesLayer);
    if (collider2Ds.Length == 0) return new List<Collider2D>();
    List<Collider2D> col2ds = new List<Collider2D>(collider2Ds.Length);
    foreach (var col in collider2Ds)
    {
        if (col != col2dExcept)
        {
            col2ds.Add(col);
        }
    }
    return col2ds;
}*/
}

[BurstCompile]
public struct FindNearestTargetJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<Vector2> enemyPositions;
    public Vector2 origin;
    public NativeArray<float> distances;

    public void Execute(int index)
    {
        distances[index] = Vector2.Distance(enemyPositions[index], origin);
    }
}

