using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAircaft : BaseAircaft
{
    [SerializeField]
    private GameObject fx;
    public override float Ranged => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Bonus>(typeBot).GetCurrentRange() / 10;
    public override float Duration => 10f;
    public override float Cooldown => 10f;

    public float bonusDmg => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Bonus>(typeBot).GetCurrentBonus();
    public float coolDownLab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.SUPPORT_AIRCRAFT_COOLDOWN).GetCurrentProperty();
    public float durationLab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.SUPPORT_AIRCRAFT_DURATION).GetCurrentProperty();

    public override void ActiveSkill(bool Active)
    {
        base.ActiveSkill(Active);
        fx.SetActive(Active);
    }

    public override void SetUp()
    {
        var data = Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Bonus>(typeBot);
        Duration = data.GetCurrentDuration() + coolDownLab;
        Cooldown = data.GetCurrentCoolDown() + durationLab;

        float scale = Mathf.Clamp(Ranged / 3.75f, minScale, maxScale);
        fx.transform.localScale = new Vector3(scale, scale, scale);
        circleCollider.radius = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.TAG_ENEMIES))
        {
            other.GetComponent<EnemyTakeDamage>().IncreareDmgBonus(bonusDmg);
        }
    }
}
