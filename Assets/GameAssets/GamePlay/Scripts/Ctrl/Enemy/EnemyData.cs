using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : Object_DataInformation, I_MoveData
{
    public float spdMove
    {
        get
        {
            var enemyData = objectData as SO_EnemyData;
            return enemyData._spdMove;
        }
        set
        {
            var enemyData = objectData as SO_EnemyData;
            enemyData._spdMove = value;
        }
    }
/*    float HPScale
    {
        get
        {


            var wave = GPm.wavePlaying;
            var data = objectData as SO_EnemyData;
            var a = data.hpScale_formula.x;
            var b = data.hpScale_formula.y;
            var c = data.hpScale_formula.z;

            var value = a * Mathf.Pow(wave, 2) + b * wave + c;
            var multiply = GPm.worldPlaying * 15;
            if (multiply <= 0)
                return value;
            return value * multiply;
        }
    }
    float damageScale
    {
        get
        {
            var wave = GPm.wavePlaying;

            var data = objectData as SO_EnemyData;
            var a = data.dmgScale_formula.x;
            var b = data.dmgScale_formula.y;
            var c = data.dmgScale_formula.z;

            var value = a * Mathf.Pow(wave, 2) + b * wave + c;
            var multiply = GPm.worldPlaying * 15;
            if (multiply <= 0)
                return value;
            return value * multiply;
        }
    }*/
    public float HPScale = 1;
    public float damageScale = 1;

    public EnemyAttackType EnemyAttackType
    {
        get
        {
            var data = objectData as SO_EnemyData;
            return data.enemyAttackType;
        }
    }
    public override float maxHP => (base.maxHP/* * HPScale*/) + BonusMaxHP;
    public override float damage => (base.damage /** damageScale*/) + BonusDmg;

    public SO_ObjectData DataInformation => objectData;

    public float BonusMaxHP { get => bonusMaxHP; set => bonusMaxHP = value; }
    public float BonusDmg { get => bonusDmg; set => bonusDmg = value; }

    private float bonusMaxHP;
    private float bonusDmg;

    public float corpse_explosion_percent => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.corpse_explosion_percent);

    public Damager Damager
    {
        get
        {
            damager = new Damager();
            var percent = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.corpse_explosion_damage_percent);
            var damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage);
            damager.damage = damage * percent / 100;
            damager.type = DamageType.NORMAL;
            damager.objAttack = null;
            return damager;
        }
    }
    private Damager damager;

    public double goldDrop;
    public double goldBonusVoidNexus;
    public double sliverDrop;

    public double goldBaseDrop;
    public TypeMonster typeMonster;

    private void Start()
    {
        goldBaseDrop = goldDrop;
    }

}
