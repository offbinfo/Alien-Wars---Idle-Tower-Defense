using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTakeDamage : Object_TakeDamage
{

    private float damage;
    private float damage_reduce => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_reduce);

    private Damager damager_reflect
    {
        get
        {
            var dmr = new Damager();
            dmr.damage = damage * (data.reflect_damage / 100);
            dmr.type = DamageType.NORMAL;
            dmr.objAttack = gameObject;
            return dmr;
        }
    }

    private TowerShield towerShield;
    private TowerLifeBox towerLifeBox;
    [SerializeField]
    private AntiForceTower antiForceTower;

    public override void Awake()
    {
        base.Awake();
        towerLifeBox = GetComponent<TowerLifeBox>();
        towerShield = GetComponent<TowerShield>();
    }

    public override void TakeDamage(Damager damager)
    {
        if (DodgeChangeDmg())
        {
            return;
        } 
        damager.damage = damager.damage * (1 - (data.damage_resistant / 100));
        damager.damage -= damage_reduce;

        //force reduction
        damage = antiForceTower.DecreaseDmgByAntiForce(damager.damage)/*damager.damage*/;

        if (towerLifeBox.CurHpLifeBox > 0)
        {
            towerLifeBox.CurHpLifeBox -= damage;
        }
        else
        {
            GPm.isTowerTakeDmg = true;
            //damage resistant
            if (towerShield.shieldHP > 0)
                towerShield.shieldHP -= damage;
            else
                base.TakeDamage(damager);
        }

        ReflectDmg(damager);
    }

    private bool DodgeChangeDmg()
    {
        //dodge chance
        float dodgeChange = data.dodge_chance;

        //relic
        dodgeChange = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, dodgeChange);

        if (dodgeChange.Chance())
        {
            return true;
        }
        return false;
    }

    private void ReflectDmg(Damager damager)
    {
        //reflect damage
        float reflectDamage = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DefenseResistance, data.refect_damage_chance);

        if (reflectDamage.Chance())
        {
            //phản dmg theo % 
            damager.objAttack.GetComponent<Object_TakeDamage>()?.TakeDamage(damager_reflect);
        }
    }
}
