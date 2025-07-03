using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class InformationFactory : GameMonoBehaviour
{

    [SerializeField]
    private TMP_Text txtDamage;
    [SerializeField]
    private TMP_Text txtAtkSpeed;
    [SerializeField]
    private TMP_Text txtHp;
    [SerializeField]
    private TMP_Text txtBuyX;
    private bool isUpdateBase = false;
    [SerializeField]
    private Animator animator;  

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.UpgraderTowerBaseGame, UpdateStatBase);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, UpdateStatBase);
        EventDispatcher.AddEvent(EventID.OnSetChanged, UpdateStatBase);
        EventDispatcher.AddEvent(EventID.OnChangedTabCard, UpdateStatBase);
        EventDispatcher.AddEvent(EventID.OnChangeCountBuyX, OnChangeCountBuyX);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, UpdateStatBase);

        OnChangeCountBuyX(null);
        animator.Play("Close");
    }

    private void OnChangeCountBuyX(object obj)
    {
        txtBuyX.text = GameDatas.CountBuyXUpgrader == 4 ? "Max" : $"x{GameDatas.CountBuyXUpgrader}";
    }

    private void OnEnable()
    {
        UpdateStatBase(null);
    }

    private void UpdateStatBase(object o)
    {
        float damage = Cfg.upgraderCtrl.GetStatBase(UpgraderID.attack_damage) * (1 + Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_DAMAGE) / 100f);
        float health = Cfg.upgraderCtrl.GetStatBase(UpgraderID.health) * (1 + Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_HEALTH) / 100f);

        float dmgBonus = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, damage);
        float healthBonus = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerHealth, health);

        AnimateText(txtDamage, dmgBonus, 1);
        AnimateText(txtAtkSpeed, Cfg.upgraderCtrl.GetStatBase(UpgraderID.attack_speed) * (1 + Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_ATK_SPD) / 100f), 2);
        AnimateText(txtHp, healthBonus, 0);
    }


    private void AnimateText(TMP_Text textUI, float newValue, int decimalPlaces)
    {
        if (textUI == null) return;

        float oldValue = float.TryParse(textUI.text, out float parsedValue) ? parsedValue : 0;
        string format = decimalPlaces > 0 ? $"F{decimalPlaces}" : "F0";

        textUI.text = Extensions.FormatCompactNumber(newValue).ToString();
/*
        if (Mathf.Abs(oldValue - newValue) > 0.01f)
        {
            DOTween.To(() => oldValue, x => textUI.text = x.ToString(format), newValue, 0.5f)
                .OnComplete(() => CompleteTween(textUI, newValue));
        }*/
    }

/*    private void CompleteTween(TMP_Text textUI, float newValue)
    {
        //float roundedCurrentValue = Mathf.Round(newValue * 100f) / 100f;
        textUI.text = Extensions.FormatCompactNumber(newValue).ToString();
    }*/

    private void Update()
    {
        if (!isUpdateBase)
        {
            isUpdateBase = true;
            UpdateStatBase(null);
        }
    }
}
