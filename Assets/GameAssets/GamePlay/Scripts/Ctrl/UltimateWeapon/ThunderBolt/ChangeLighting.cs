using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLighting : GameMonoBehaviour
{

    private CircleCollider2D _collider2D;
    public LayerMask layerMask;

    public GameObject chainLightEffect;
    public GameObject beenStruck;

    //public int amountToChain = 5;
    private GameObject startObj;
    public GameObject endObj;

    private Animator animator;
    public ParticleSystem fx;
    private int singleSpawns;
    private Object_Pool object_Pool;
    private Tween returnTween;

    private bool stockStun => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.THUNDER_STUN).GetCurrentProperty() > 0 ? true : false;
    private float stunChange => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.THUNDER_STUN_CHANCE).GetCurrentProperty();
    private float stunMultipliter => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.THUNDER_STUN_MULTIPLIER).GetCurrentProperty() / 100;
    private int quantityThunder => Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Active_Change>(UW_ID.THUNDER_BOLT).GetCurrentQuantity();

    private void Awake()
    {
        object_Pool = GetComponent<Object_Pool>();
        _collider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        fx = GetComponent<ParticleSystem>();
    }

    Damager damager
    {
        get
        {
            var dmg = new Damager();
            dmg.damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage)
                * Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Active_Change>(UW_ID.THUNDER_BOLT).GetCurrentDmg();
            dmg.objAttack = gameObject;
            dmg.type = DamageType.NORMAL;
            return dmg;
        }
    }

    /*    private void Start()
        {
            if (amountToChain == 0) object_Pool.ReturnPool();
            singleSpawns = 1;

            startObj = gameObject;
        }*/

    private void OnEnable()
    {
        _collider2D.enabled = true;
        //if (amountToChain == 0) object_Pool.ReturnPool();
        singleSpawns = quantityThunder;

        startObj = gameObject;

        returnTween = DOVirtual.DelayedCall(1f, () =>
        {
            object_Pool.ReturnPool();
        });
    }

    private void OnDisable()
    {
        if (returnTween != null && returnTween.IsActive())
            returnTween.Kill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int otherLayer = collision.gameObject.layer;

        if (((1 << otherLayer) & layerMask) != 0 && !collision.GetComponentInChildren<EnemyStruck>())
        {
            if (singleSpawns <= 0)
            {
                _collider2D.enabled = false;
                return;
            }

            Transform targetTransform = collision.transform;

            startObj = gameObject;
            endObj = collision.gameObject;

            Instantiate(beenStruck, targetTransform);
            PoolCtrl.instance.Get(PoolTag.THUNDER_LIGHT, targetTransform.position, Quaternion.identity);

            //stock stun
            StockStun(collision);

            collision.GetComponent<EnemyTakeDamage>().TakeDamage(damager);

            animator.StopPlayback();
            _collider2D.enabled = false;

            singleSpawns--;

            fx.Play();

            Vector3 posStart = startObj.transform.position;
            Vector3 posEnd = endObj.transform.position;
            Vector3 mid = (posStart + posEnd) * 0.5f;

            var emitParams = new ParticleSystem.EmitParams();

            emitParams.position = posStart;
            fx.Emit(emitParams, 5);

            emitParams.position = posEnd;
            fx.Emit(emitParams, 5);

            emitParams.position = mid;
            fx.Emit(emitParams, 5);
        }
    }

    private float timeStun = 0.25f;
    private void StockStun(Collider2D other)
    {
        if(!stockStun) return;
        float stunThunder = stunChange + 2.5f;
        if (stunThunder.Chance())
        {
            timeStun *= stunMultipliter;
            var stunEffect = other.GetComponent<I_Stun>();
            stunEffect?.Stun(timeStun);
        }
    }


    /*    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (layerMask == (layerMask | (1 << collision.gameObject.layer)) && !collision.GetComponentInChildren<EnemyStruck>())
            {
                if(singleSpawns != 0)
                {
                    startObj = gameObject;
                    endObj = collision.gameObject;

                    Instantiate(beenStruck, collision.gameObject.transform);
                    PoolCtrl.instance.Get(PoolTag.THUNDER_LIGHT, collision.gameObject.transform.position, Quaternion.identity);

                    Damager dmg = new Damager();
                    dmg.damage = damageThunder;
                    dmg.type = DamageType.NORMAL;
                    collision.gameObject.GetComponent<EnemyTakeDamage>().TakeDamage(dmg);

                    animator.StopPlayback();
                    _collider2D.enabled = false;

                    singleSpawns--;
                    fx.Play();

                    var emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = startObj.transform.position;

                    fx.Emit(emitParams, 5);

                    emitParams.position = endObj.transform.position;

                    fx.Emit(emitParams, 5);

                    emitParams.position = (startObj.transform.position + endObj.transform.position) / 2;

                    fx.Emit(emitParams, 5);
                }
            }
        }*/
}
