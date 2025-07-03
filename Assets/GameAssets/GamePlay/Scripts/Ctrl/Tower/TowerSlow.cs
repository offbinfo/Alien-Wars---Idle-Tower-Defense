using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlow : GameMonoBehaviour
{
    float slowPower;
    [SerializeField] GameObject obj_Graphic_slow;
    [SerializeField] Collider2D colliderSlow;
    private void Awake()
    {
        obj_Graphic_slow.SetActive(false);
        EventDispatcher.AddEvent(EventID.OnEquipCard, (o) => {
            if ((CardID)o == CardID.EPIC_FREEZE_AREA)
                ActiveSLow(true);
        });
        EventDispatcher.AddEvent(EventID.OnUnequipcard, (o) => {
            if ((CardID)o == CardID.EPIC_FREEZE_AREA)
                ActiveSLow(false);
        });
        EventDispatcher.AddEvent(EventID.OnLevelCardChanged, (o) => {
            if ((CardID)o == CardID.EPIC_FREEZE_AREA)
                slowPower = Cfg.cardCtrl.GetCurrentStat(CardID.EPIC_FREEZE_AREA);
        });

        colliderSlow = GetComponent<Collider2D>();
    }

    private void Start()
    {
        //setup
        var cardFreeze = Cfg.cardCtrl.GetCard(CardID.EPIC_FREEZE_AREA);
        slowPower = cardFreeze.GetCurrentStat();

        ActiveSLow(GameDatas.IsCardEquiped(cardFreeze.id));
    }

    private void ActiveSLow(bool isActive)
    {
        obj_Graphic_slow.SetActive(isActive);
        colliderSlow.enabled = isActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.TAG_ENEMIES))
        {
            //DebugCustom.LogColor("SLow AOE");
            other.GetComponent<I_Slow>().Slow(slowPower, 9999f);
        }
    }
}
