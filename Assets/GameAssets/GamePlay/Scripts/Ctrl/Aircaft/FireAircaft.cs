using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAircaft : BaseAircaft
{
    [SerializeField]
    private GameObject fx;
    public override float Ranged => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Damage>(typeBot).GetCurrentRange() / 10;
    public override float Duration => 1.5f;
    public override float Cooldown => 10f;
    public float coolDownLab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.FIRE_AIRCRAFT_COOLDOWN).GetCurrentProperty();
    public float burnStack => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.FIRE_AIRCRAFT_BURN_STACK).GetCurrentProperty();

    Damager damager
    {
        get
        {
            var dmg = new Damager();
            dmg.damage = Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Damage>(typeBot).GetCurrentDmgBonus();
            dmg.objAttack = gameObject;
            dmg.type = DamageType.NORMAL;
            return dmg;
        }
    }

    public override void ActiveSkill(bool Active)
    {
        base.ActiveSkill(Active);
        fx.SetActive(Active);
    }

    public override void SetUp()
    {
        var data = Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Damage>(typeBot);
        Duration = data.GetCurrentDuration() == 0 ? (1.5f + burnStack) : data.GetCurrentDuration() + burnStack;
        Cooldown = data.GetCurrentCoolDown() + coolDownLab;

        float scale = Mathf.Clamp(Ranged / 3.75f, minScale, maxScale);
        fx.transform.localScale = new Vector3(scale, scale, scale);
        circleCollider.radius = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            var eTakeDmg = collision.GetComponent<EnemyTakeDamage>();
            eTakeDmg?.TakeDamage(damager);
        }
    }
}
