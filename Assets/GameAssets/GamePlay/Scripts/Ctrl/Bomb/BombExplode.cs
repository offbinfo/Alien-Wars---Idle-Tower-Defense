using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplode : GameMonoBehaviour
{

    [SerializeField] LayerMask layerEnemies;
    private Object_Pool objPool;
    float damageBomb => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage) *
            (1 + Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_DAMAGE) / 100f) * (1 + Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bomb_damage) / 100);
    float rangeExplode
    {
        get
        {
            var statCard = Cfg.cardCtrl.GetCurrentStat(CardID.RARE_BOMB_RANGE);
            var multiply = 1 + statCard / 100f;
            return Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bomb_area) / 9 * multiply;
        }
    }
    float slowPower
    {
        get
        {
            return Cfg.cardCtrl.GetCurrentStat(CardID.DIVINE_BOMB_SLOW);
        }
    }
    private bool isExplode = false;

    private void Awake()
    {
        objPool = GetComponent<Object_Pool>();
    }

    private void OnEnable()
    {
        isExplode = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(GameTags.TAG_ENEMIES) && !isExplode)
        {
            DebugCustom.Log("Explode");
            isExplode = true;
            Explode();
        }
    }

    private void Explode()
    {
        var fx = PoolCtrl.instance.Get(PoolTag.EXPLOSIVE1, transform.position, Quaternion.identity);
        fx.transform.localScale = Vector3.one * rangeExplode;

        objPool.ReturnPool();

        EventDispatcher.PostEvent(EventID.OnBombExplode, null);
        TakeDmgRangedArea();
    }

    private void TakeDmgRangedArea()
    {
        Damager dmg = new Damager();
        dmg.damage = damageBomb;
        dmg.type = DamageType.NORMAL;
        Collider2D[] cols2d = Physics2D.OverlapCircleAll(transform.position, rangeExplode, layerEnemies); //range
        for (int i = 0; i < cols2d.Length; i++)
        {
            if (!cols2d[i].gameObject.activeSelf) return;
            cols2d[i].GetComponent<Object_TakeDamage>()?.TakeDamage(dmg);
            if(slowPower != 0)
            {
                cols2d[i].GetComponent<EnemyTakeEffect>()?.Slow(slowPower, 1f);
            }
        }
    }
}
