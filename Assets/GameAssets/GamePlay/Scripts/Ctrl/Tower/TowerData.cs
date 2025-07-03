using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : Object_DataInformation
{

    public override float attackRange => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_range) / 10;
    public override float maxHP
    {
        get
        {
            var statCard = Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_HEALTH);
            var multiply = 1 + statCard / 100f;
            var sum = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.health) * multiply;
            float sumNew = sum + HPPerEnemyKillByShockWave;
            return Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, sumNew);
        }
    }

    public override float regen_speed
    {
        get
        {
            var statCard = Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_HP_REGEN);
            var multiply = 1 + statCard / 100f;
            return Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.generation) * multiply;
        }
    }

    public override float damage_resistant => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_resistance);
    public override float dodge_chance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.dodge_chance);
    public override float resistance_chance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_resistance);
    public override float refect_damage_chance => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_return_chance);
    public override float reflect_damage => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_return_power);

    private int countEnemiesKillByShockwave = 0;
    public float HPPerEnemyKillByShockWave;
    //public float bonusHpShockWave => HPPerEnemyKillByShockWave;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.EnemyDestroyedByShockwave, (obj) =>
        {
            countEnemiesKillByShockwave++;
            hpCurrent = hpCurrent;
        });
        EventDispatcher.AddEvent(EventID.ShockwaveBonusHealth, (obj) =>
        {
            HPPerEnemyKillByShockWave = (maxHP * (1f + Cfg.labCtrl.LapManager.GetSingleSubjectById
            (IdSubjectType.SHOCKWAVE_HEALTH).GetCurrentProperty() / 100)) - maxHP;
            maxHP = maxHP;
            hpCurrent = hpCurrent;
        });
        EventDispatcher.AddEvent(EventID.UpgraderTowerInGame, OnMaxHPChanged);
        EventDispatcher.AddEvent(EventID.OnEquipCard, OnMaxHPChanged_Card);
        EventDispatcher.AddEvent(EventID.OnUnequipcard, OnMaxHPChanged_Card);
    }

    private void OnMaxHPChanged(object o)
    {
        if ((UpgraderID)o == UpgraderID.health)
        {
            hpCurrent = hpCurrent;
        }
    }
    private void OnMaxHPChanged_Card(object o)
    {
        if ((CardID)o == CardID.COMMON_HEALTH)
        {
            hpCurrent = hpCurrent;
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.UpgraderTowerInGame, OnMaxHPChanged);
        EventDispatcher.RemoveEvent(EventID.OnEquipCard, OnMaxHPChanged_Card);
        EventDispatcher.RemoveEvent(EventID.OnUnequipcard, OnMaxHPChanged_Card);
    }
}
