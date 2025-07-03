using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface I_Bullet
{
    void SetUp(Damager damager, float slowPower, 
        float slowTime, float areaDmg, float multiPower, bool isBounce, float timeStun, bool isKnockBack, bool isDeadHit);
}

public class BulletCollided : GameMonoBehaviour, I_Bullet
{
    Damager damager;
    float slowPower;
    float slowTime;
    float timeStun;
    float areaDmg;
    bool isBounce;
    bool isKnockBack;
    bool isDeadHit;
    protected Object_Destroy objDestroy;
    protected Object_Pool objPool;
    protected HashSet<Collider2D> cols2dCollided = new HashSet<Collider2D>();
    protected float shotTargetNumber => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bounce_shot_target_number);

    protected int countBounce = 0;

    private static Collider2D[] overlapResults = new Collider2D[10];
    private ThunderBolt thunderBolt;
    private bool isThunderBolt => GameDatas.isUnlockUltimateWeapon(UW_ID.THUNDER_BOLT);
    private float radiusOverlapCircle = 1.5f;

    private float tomahawkExplotionRadius => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.TOMAHAWK_EXPLOTION_RADIUS).GetCurrentProperty();
    private float tomahawkAmplify => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.TOMAHAWK_AMPLIFY).GetCurrentProperty();
    public bool isTomahawk;


    private void OnEnable()
    {
        countBounce = 0;
    }

    protected virtual void Awake()
    {
        objDestroy = GetComponent<Object_Destroy>();
        objPool = GetComponent<Object_Pool>();
        objPool.AddEventInit(OnReset);
    }
    private void Start()
    {
        thunderBolt = ThunderBolt.instance;
    }

    void OnReset(object o)
    {
        cols2dCollided.Clear();
    }

    public GameObject beenStruck;
    public ChangeLighting changeLighting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(GameTags.TAG_ENEMIES) || !cols2dCollided.Add(other)) return;

        var takeDamage = other.GetComponent<Object_TakeDamage>();
        var slowEffect = other.GetComponent<I_Slow>();
        var stunEffect = other.GetComponent<I_Stun>();
        var knockBackEffect = other.GetComponent<I_KnockBack>();
        var destroyObj = other.GetComponent<Object_Destroy>();

        try
        {
            // Life steal (hút máu)
            damager.objAttack.GetComponent<TowerLifeSteal>()?.StealLife(damager.damage);
        }
        catch (System.Exception ex)
        {
            
        }

        if (isThunderBolt && thunderBolt.changeThunder.Chance())
        {
            //chain thunder
            if (beenStruck != null)
            {
                Instantiate(beenStruck, other.transform);
            }
            PoolCtrl.instance.Get(PoolTag.THUNDER_LIGHT, other.transform.position, Quaternion.identity);
        }

        //tomahawk ampilify
        if (isTomahawk && takeDamage.isFlagTomahawk)
        {
            damager.damage += tomahawkAmplify;
            radiusOverlapCircle += tomahawkExplotionRadius;
            EventChallengeListenerManager.KillWithTomahawk(1);
        }

        if (isDeadHit)
        {
            QuestEventManager.CriticalHits(1);

            EventChallengeListenerManager.CriticalHits(1);
            PoolCtrl.instance.Get(PoolTag.TEXT_POP, transform.position, Quaternion.identity, "HEAD SHOT");
            destroyObj?.Destroy(damager);
        }
        else
        {
            takeDamage?.TakeDamage(damager);
            slowEffect?.Slow(slowPower, slowTime);
            stunEffect?.Stun(timeStun);
            if (isKnockBack) knockBackEffect?.KnockBack();
        }

        // Gây damage theo vùng (AOE)
        areaDmg = Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.DamagePerMeter, areaDmg);
        if (areaDmg > 0)
        {
            damager.damage = damager.damage * (Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.damage_range) / 100) * areaDmg / 100;
            int count = Physics2D.OverlapCircleNonAlloc(other.transform.position, radiusOverlapCircle, overlapResults);
            for (int i = 0; i < count; i++)
            {
                Collider2D col = overlapResults[i];
                if (col != other) col.GetComponent<Object_TakeDamage>()?.TakeDamage(damager);
            }
        }

        // Nếu đạn có hiệu ứng bật nảy
        if (isBounce)
        {
            if (countBounce > shotTargetNumber)
            {
                objDestroy?.Destroy(null);
            }
            else
            {
                Bounce();
                return;
            }
        }
        else
        {
            objDestroy?.Destroy(null);
        }

        if (isTomahawk && takeDamage.curHp > 0)
        {
            takeDamage.isFlagTomahawk = true;
        }
    }

    public void SetUp(Damager damager, float slowPower, float slowTime, float areaDmg, float multiPower = 100, bool isBounce = false, float timeStun = 0, bool isKnockBack = false, bool isDeadHit = false)
    {
        this.damager = damager;
        this.slowPower = slowPower;
        this.slowTime = slowTime;
        this.areaDmg = areaDmg;
        this.isBounce = isBounce;
        this.timeStun = timeStun;
        this.isKnockBack = isKnockBack;
        this.isDeadHit = isDeadHit;
        this.damager.damage = this.damager.damage * (multiPower / 100);
    }

    public virtual void Bounce()
    {
        //giảm damage trong này là xong
        //bỏ slow bỏ stun bỏ knoackback nằm ở trong này 
        //set up lại là xong
    }
}
