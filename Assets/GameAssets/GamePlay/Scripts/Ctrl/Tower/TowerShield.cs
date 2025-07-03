using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShield : Object_Shield
{

    public override float shieldHPMax => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.shield_hp) / 100;
    public override float timeRefillShield => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.shield_spawn_time);
    public LayerMask enemies;
    private Object_DataInformation objData;
    private TowerAttack towerAttack;

    private void Awake()
    {
        objData = GetComponent<Object_DataInformation>();
        towerAttack = GetComponent<TowerAttack>();
    }

    protected override void ExplodeShield()
    {
        base.ExplodeShield();
        //explode shield
        if (!GameDatas.IsCardEquiped(CardID.RARE_SHIELD_EXPLODE))
            return;

        var fx = PoolCtrl.instance.Get(PoolTag.EXPLOSIVE_SHIELD, transform.position, Quaternion.identity);
        //fx.transform.localScale = Vector3.one * 2f;
        var dmg = new Damager();
        var statCard = Cfg.cardCtrl.GetCurrentStat(CardID.RARE_SHIELD_EXPLODE);
        var multiply = 1 + statCard / 100f;
        dmg.damage = towerAttack.dmg * multiply;

        var cols2d = Physics2D.OverlapCircleAll(transform.position, objData.attackRange, enemies);
        foreach (var col in cols2d)
        {
            var knockBackEffect = col.GetComponent<I_KnockBack>();
            knockBackEffect?.KnockBack();
            col.GetComponent<Object_TakeDamage>().TakeDamage(dmg);
        }
    }
}
