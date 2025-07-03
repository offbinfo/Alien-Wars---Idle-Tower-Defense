using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBeam : GameMonoBehaviour
{

    Damager damager
    {
        get
        {
            var dmg = new Damager();
            dmg.damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage) *
                (Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.kill_beam_damage) / 100);
            dmg.objAttack = gameObject;
            dmg.type = DamageType.NORMAL;
            return dmg;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            collision.GetComponent<EnemyTakeDamage>().TakeDamage(damager);
        }
    }

}
