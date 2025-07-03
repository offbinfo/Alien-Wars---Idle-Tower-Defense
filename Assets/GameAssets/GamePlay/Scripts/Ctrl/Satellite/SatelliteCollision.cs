using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteCollision : GameMonoBehaviour
{
    private Object_Pool objPool;
    float timeStun => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.satellite_stun_time);
    Damager damager
    {
        get
        {
            var dmg = new Damager();
            dmg.damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage) *
            (1 + Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_DAMAGE) / 100f) * (1 + Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.satellite_damage) / 100);
            dmg.objAttack = gameObject;
            dmg.type = DamageType.NORMAL;
            return dmg;
        }
    }

    int count = 0;
    private void Awake()
    {
        objPool = GetComponentInParent<Object_Pool>();
    }

    private void OnEnable()
    {
        count = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.TAG_ENEMIES))
        {
            var eTakeDmg = other.GetComponent<EnemyTakeDamage>();

            if (eTakeDmg.isBoss)
            {
                var multiply = 1 + Cfg.cardCtrl.GetCurrentStat(CardID.RARE_SATELLI_SMART) / 100f;
                damager.damage = damager.damage * multiply;
            }

            eTakeDmg?.TakeDamage(damager);
            float time = timeStun / 100;
            other.GetComponent<I_Stun>()?.Stun(time);

            //destroy
            objPool.ReturnPool();
            PoolCtrl.instance.Get(PoolTag.EXPLOSIVE1, transform.position, Quaternion.identity);
            if (count == 0)
            {
                EventDispatcher.PostEvent(EventID.OnCallSatellite, null);
                count = 1;
            }
        }
    }
}
