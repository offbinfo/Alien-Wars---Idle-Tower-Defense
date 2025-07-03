using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFlyDrop : GameMonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private List<string> nameAnimDrops = new();

    float lifebox_change => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.lifebox_change) +
    (ConfigManager.instance.labCtrl.LapManager.
                GetSingleSubjectById(IdSubjectType.LIFEBOX_CHANGE).GetCurrentProperty() / 100);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.FinishWave, ActiveAnim);
        EventDispatcher.AddEvent(EventID.OnLifeBoxAfterBoss, ActiveAnim);
    }

    public void ActiveAnim(object o)
    {
        if (!GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_LIFE_BOX)) return;
        if (lifebox_change >= 30 || lifebox_change.Chance())
        {
            animator.enabled = true;
            int rand = Random.Range(0, nameAnimDrops.Count - 1);

            animator.Play(nameAnimDrops[rand]);
        }
    }

    public void ClaimItemDrop()
    {
        DebugCustom.LogColor("ClaimItemDrop");
        animator.enabled = false;
        EventDispatcher.PostEvent(EventID.OnClaimItemLifeBoxDrop, 0);
    }
 }
