using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TabInformationGamePlay : GameMonoBehaviour
{

    [SerializeField]
    private TMP_Text txtDamage;
    [SerializeField]
    private TMP_Text txtAtkSpeed;
    [SerializeField]
    private TMP_Text txtHp;
    private TowerData towerData;

    [SerializeField]
    private Animator animTxtDmg;
    [SerializeField]
    private Animator animTxtHp;
    [SerializeField]
    private Animator animTxtSpeed;
    private float maxHP;
    private float oldValueCur;
    [SerializeField]
    private TMP_Text txtBuyX;
    [SerializeField]
    private Animator animator;
    private bool isSetUp;

    private const string nameOpenAnim = "Open";
    private const string nameCloseAnim = "Close";

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.UpgraderTowerInGame, UpdateStatInGame);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, UpdateStatInGame);
        EventDispatcher.AddEvent(EventID.OnSetChanged, UpdateStatInGame);
        EventDispatcher.AddEvent(EventID.OnChangedTabCard, UpdateStatInGame);
        EventDispatcher.AddEvent(EventID.ShockwaveBonusHealth, UpdateStatInGame);
        EventDispatcher.AddEvent(EventID.OnChangeCountBuyX, OnChangeCountBuyX);
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, UpdateStatInGame);

        OnChangeCountBuyX(null);
        animator.Play("Close");
    }

    private void OnChangeCountBuyX(object obj)
    {
        txtBuyX.text = GameDatas.CountBuyXUpgrader == 4 ? "Max" : $"x{GameDatas.CountBuyXUpgrader}";
    }

    private void OnEnable()
    {
        towerData = GPm.Tower.TowerData;
        UpdateStatInGame(null);
    }

    private void Update()
    {
        SetUpUIHealth();
    }

    private void SetUpUIHealth()
    {
        if (!isSetUp)
        {
            float newMaxHP = towerData.maxHP/* + towerData.HPPerEnemyKillByShockWave*/;
            if (maxHP != newMaxHP)
            {
                maxHP = newMaxHP;
                AnimateText(txtHp, maxHP, 0, TypeInformation.HP);
            }
            isSetUp = true;
        }
    }

    public void UpdateStatInGame(object o)
    {
        float damage = Cfg.upgraderCtrl.GetStatBase(UpgraderID.attack_damage) * (1 + Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_DAMAGE) / 100f);
        float dmgBonus = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.TowerDamage, damage);

        AnimateText(txtDamage, dmgBonus, 1, TypeInformation.DMG);
        AnimateText(txtAtkSpeed, Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_speed) * 
            (1 + Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_ATK_SPD) / 100f), 2, TypeInformation.SPEED);

        float newMaxHP = towerData.maxHP/* + towerData.HPPerEnemyKillByShockWave*/;
        if (maxHP != newMaxHP)
        {
            maxHP = newMaxHP;

            AnimateText(txtHp, maxHP, 0, TypeInformation.HP);
        }
    }

    private void AnimateText(TMP_Text textUI, float newValue, int decimalPlaces, TypeInformation typeInformation)
    {
        if (textUI == null) return;

        float oldValue = float.TryParse(textUI.text, out float parsedValue) ? parsedValue : 0;
        string format = decimalPlaces > 0 ? $"F{decimalPlaces}" : "F0";

        textUI.text = Extensions.FormatCompactNumber(newValue).ToString();

        /*if (Mathf.Abs(oldValue - newValue) > 0.01f)
        {
            switch (typeInformation)
            {
                case TypeInformation.DMG: animTxtDmg.Play(nameOpenAnim); break;
                case TypeInformation.SPEED: animTxtSpeed.Play(nameOpenAnim); break;
                case TypeInformation.HP: animTxtHp.Play(nameOpenAnim); break;
            }
            DOTween.To(() => oldValue, x => textUI.text = x.ToString(format), newValue, 0.5f)
                .OnComplete(() => CompleteTween(typeInformation, textUI, newValue));
        }*/
    }

/*    private void CompleteTween(TypeInformation typeInformation, TMP_Text textUI, float newValue)
    {
        switch (typeInformation)
        {
            case TypeInformation.DMG:
                animTxtDmg.Play(nameCloseAnim);
                break;
            case TypeInformation.SPEED:
                animTxtSpeed.Play(nameCloseAnim);
                break;
            case TypeInformation.HP:
                animTxtHp.Play(nameCloseAnim);
                break;
        }
        //float roundedCurrentValue = Mathf.Round(newValue * 100f) / 100f;
        textUI.text = Extensions.FormatCompactNumber(newValue).ToString();
    }*/

    private void OnDisable()
    {
        if (Cfg != null)
            Cfg.upgraderCtrl.ResetDictInGame();
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.UpgraderTowerInGame, UpdateStatInGame);
        EventDispatcher.RemoveEvent(EventID.OnUpgradeSubjectSuccessLab, UpdateStatInGame);
    }
}

public enum TypeInformation
{
    DMG,
    SPEED,
    HP
}
