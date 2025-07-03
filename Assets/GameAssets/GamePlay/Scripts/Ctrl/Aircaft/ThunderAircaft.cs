using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAircaft : BaseAircaft
{
    [SerializeField]
    private GameObject fx;
    public override float Ranged => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Shock>(typeBot).GetCurrentRange() / 10;
    public override float Duration => 10f;
    public override float Cooldown => 10f;
    private float shock = 500f;

    public float shockChange => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Shock>(typeBot).GetCurrentShockChange();
    public float coolDownLab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.THUNDER_AIRCRAFT_COOLDOWN).GetCurrentProperty();
    public float shockTime => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.THUNDER_AIRCRAFT_SHOCK_TIME).GetCurrentProperty();

    public override void ActiveSkill(bool Active)
    {
        base.ActiveSkill(Active);
        fx.SetActive(Active);
    }

    public override void SetUp()
    {
        var data = Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Shock>(typeBot);
        Duration = data.GetCurrentDuration();
        Cooldown = data.GetCurrentCoolDown();

        float scale = Mathf.Clamp(Ranged / 3.75f, minScale, maxScale);
        fx.transform.localScale = new Vector3(scale, scale, scale);
        circleCollider.radius = scale;

        shock += shockTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            if(shockChange.Chance())
            {
                var eTakeDmg = collision.GetComponent<EnemyTakeDamage>();

                collision.GetComponent<I_Stun>()?.Stun(shock);
            }
        }
    }
}
