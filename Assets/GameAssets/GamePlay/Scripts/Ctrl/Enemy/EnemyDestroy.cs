using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;

public class EnemyDestroy : Object_Destroy, ICastedLightGolden
{
    private Object_Pool objPool;
    private Object_DataInformation data;
    private Collider2D _collider;
    [SerializeField]
    private PoolTag typeFx;
    [SerializeField]
    private LayerMask layerEnemies;
    private EnemyCtrl enemyCtrl;
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private EnemyUpperCtrl enemyUpperCtrl;

    private GoldenSanctuary goldenSanctuary;
    private VoidNexusTower voidNexusTower;

    public EnemyType EnemyType => enemyType;
    float corpse_explosion_percent => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.corpse_explosion_percent);
    float corpse_explosion_range => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.corpse_explosion_range);
    float coin_per_kill => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.coin_per_kill);
    float gold_per_kill => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.gold_per_kill);

    float lifebox_after_boss => ConfigManager.instance.labCtrl.LapManager.
                    GetSingleSubjectById(IdSubjectType.LIFEBOX_AFTER_BOSS).GetCurrentProperty();

    float highLight_gold_bonus => ConfigManager.instance.labCtrl.LapManager.
                GetSingleSubjectById(IdSubjectType.HIGHTLIGHT_GOLD_BONUS).GetCurrentProperty();

    float gold_bonus_aircaft => Cfg.botCtrl.BotManager.GetDataById<SO_Bot_Bonus>(TypeBot.GoldenAircaft).GetCurrentBonus();

    public Damager _Damager
    {
        get
        {
            damager = new Damager();
            var percent = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.corpse_explosion_damage_percent);
            var damage = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_damage);
            damager.damage = damage * percent / 100;
            damager.type = DamageType.NORMAL;
            damager.objAttack = null;
            return damager;
        }
    }

    Damager damager;

    private void Awake()
    {
        enemyCtrl = GetComponent<EnemyCtrl>();
        objPool = GetComponent<Object_Pool>();
        data = GetComponent<Object_DataInformation>();
        _collider = GetComponent<Collider2D>();
        enemyData = GetComponent<EnemyData>();
        enemyUpperCtrl = GetComponent<EnemyUpperCtrl>();
    }

    public override void Destroy(Damager d)
    {
        CheckHandleEnemy(d);
    }

    private void CheckHandleEnemy(Damager d)
    {
        switch(enemyType)
        {
            case EnemyType.BOSS:
                HandleBoss();
                break;
            case EnemyType.ENEMY:
                HandleEnemy(d);
                break;
        }

        GameDatas.countEnemyDestroy += 1;


        if (enemyUpperCtrl != null && enemyData.typeMonster == TypeMonster.Ripple)
        {
            if (!enemyUpperCtrl.IsTrySplit())
            {
                DropCoinOrSliver(enemyData.goldDrop, enemyData.sliverDrop);
            }
        } else
        {
            DropCoinOrSliver(enemyData.goldDrop, enemyData.sliverDrop);
        }

        EventDispatcher.PostEvent(EventID.OnEnemyKilled, transform.position);
        PoolCtrl.instance.Return(objPool);
        PoolCtrl.instance.Get(typeFx, transform.position, Quaternion.identity);
        PoolCtrl.instance.Get(PoolTag.BLOOD_ENEMY, transform.position, Quaternion.identity);
    }

    private void HandleBoss()
    {
        if (lifebox_after_boss > 0)
        {
            EventDispatcher.PostEvent(EventID.OnLifeBoxAfterBoss, 0);
        }
        GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.KILL_3_BOSS, GameDatas.GetBeginnerQuestProgress(BeginnerQuestID.KILL_3_BOSS) + 1);
        GameDatas.SetDataProfile(IDInfo.NumberofBossesDefeated, GameDatas.GetDataProfile(IDInfo.NumberofBossesDefeated) + 1);
        QuestEventManager.BossKilled(1);

        //PoolCtrl.instance.Get(PoolTag.EXPLOSIVE1, transform.position, Quaternion.identity);
        AudioManager.PlaySoundStatic("explosive_2");
        EventChallengeListenerManager.KillBossesOutOfRange(1);
        EventChallengeListenerManager.BossKilled(1);
        PoolCtrl.instance.Get(PoolTag.NOVA_FIRE, transform.position, Quaternion.identity);
    }

    private void HandleEnemy(Damager d)
    {
        if (d != null && d.isShockwave)
            DropGoldBySHockWave(enemyData.goldDrop);

        if(enemyUpperCtrl != null && enemyData.typeMonster == TypeMonster.Ripple)
        {
            if(enemyUpperCtrl.IsTrySplit())
            {
                enemyUpperCtrl.TrySplit();
            }
        }

        GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.KILL_100_CREEP, GameDatas.GetBeginnerQuestProgress(BeginnerQuestID.KILL_100_CREEP) + 1);
        enemyCtrl.hpbar.InActiveHpBar(false);
        PoolCtrl.instance.Return(objPool);
        PoolCtrl.instance.Get(typeFx, transform.position, Quaternion.identity);
        AudioManager.PlaySoundStatic("enemies_die_small_" + Random.Range(1, 8));

        CoroseExplosionPersent();
        CheckQuestKillEnemy();
    }

    private void CoroseExplosionPersent() 
    {
        if (corpse_explosion_percent.Chance())
        {
            //explosion corpse
            var multiplyRange = 1 + Cfg.cardCtrl.GetCurrentStat(CardID.DIVINE_CORPSE_EXPLODE_RANGE) / 100f;

            var range = (1f * multiplyRange) + corpse_explosion_range;
            var fx = PoolCtrl.instance.Get(PoolTag.EXPLOSIVE3, transform.position, Quaternion.identity);
            fx.transform.localScale = Vector3.one * 0.5f * range;
            Collider2D[] cols2d = Physics2D.OverlapCircleAll(transform.position, range, layerEnemies);

            foreach (var colE in cols2d)
            {
                if (colE != _collider)
                    colE.GetComponent<Object_TakeDamage>()?.TakeDamage(_Damager);
            }
        }
    }

    private void CheckQuestKillEnemy()
    {
        switch(enemyData.typeMonster) {
            case TypeMonster.Normal:
                EventChallengeListenerManager.NormalEnemiesKilled(1);
                QuestEventManager.NormalEnemiesKilled(1);
                break;
            case TypeMonster.Fast:
                EventChallengeListenerManager.FastEnemiesKilled(1);
                QuestEventManager.FastEnemiesKilled(1);
                break;
            case TypeMonster.Tank:
                EventChallengeListenerManager.TankEnemiesKilled(1);
                QuestEventManager.TankEnemiesKilled(1);
                break;
            case TypeMonster.Ranged:
                EventChallengeListenerManager.KillRangeEnemies(1);
                QuestEventManager.ShotEnemiesKilled(1);
                break;
        }
    }

    private void OnDisable()
    {
        
    }

    #region Drop gold or Sliver
    //Drop gold or Sliver
    public void DropCoinOrSliver( double gold, double sliver)
    {
        IsCheckBonusGold();

        int coinBonus = Mathf.FloorToInt(coin_per_kill / 100);
        int goldBonus = Mathf.FloorToInt(gold_per_kill + 
            (Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.GOLD_PER_KILL).GetCurrentProperty() / 100f));

        var pos = transform.position;
        //silver
        var coinPerKill = Cfg.labCtrl.LapManager.GetSingleSubjectById
            (IdSubjectType.COIN_PER_KILL).GetCurrentProperty();

        var value = (int)sliver + (int)coinPerKill;

        float sliverReward = Mathf.Floor((value + (int)resourceBonus) + coinBonus);

        //relic 
        sliverReward = Mathf.FloorToInt(Cfg.relicsCtrl.PercentRelic(BonusTypeRelic.Coins, sliverReward));

        GPm.SliverInGame += sliverReward;
        var posSilver = pos + Extensions.GetRandomPosition(0.5f);
        var objPool = PoolCtrl.instance.Get(PoolTag.CURRENCY_SLIVER, posSilver, Quaternion.identity, "+" + sliverReward);

        GameDatas.SetDataProfile(IDInfo.TotalCoinsEarned, GameDatas.GetDataProfile(IDInfo.TotalCoinsEarned) + 1);
        //gold

        if(isHightLightBonusGold)
        {
            EventChallengeListenerManager.KillInHighlight(1);
        }
        float goldBonudHighlight = isHightLightBonusGold ? Mathf.Floor(highLight_gold_bonus) : 0;
        float goldBonusAircaftBot = isBonusGoldAircraftBot ? gold_bonus_aircaft : 1;

        var amount = (gold + enemyData.goldBonusVoidNexus + goldBonudHighlight) * goldBonusAircaftBot;
        if (amount == 0) return;
        if (70f.Chance())
        {
            var posGold = pos + Extensions.GetRandomPosition(0.5f);
            if (GameDatas.x_sum > 0)
            {
                amount *= GameDatas.x_sum;
            }

            float floorGold = Mathf.Floor(goldBonus + resourceBonus);
            amount += floorGold;
            PoolCtrl.instance.Get(PoolTag.CURRENCY_GOLD, posGold, Quaternion.identity, "+" + amount);

            GPm.GoldInGame += (int)amount;
            GameDatas.Gold += (int)amount;
        }
    }

    private void IsCheckBonusGold()
    {
        if (!goldenSanctuary)
        {
            goldenSanctuary = GoldenSanctuary.instance;
        }
        if (!goldenSanctuary.isCastGolden)
        {
            resourceBonus = 0;
        }
        if (!voidNexusTower)
        {
            voidNexusTower = VoidNexusTower.instance;
        }
        if (!voidNexusTower.isActiveVoidNexus)
        {
            enemyData.goldBonusVoidNexus = 0;
        }
    }

    private bool isHightLightBonusGold;
    private bool isBonusGoldAircraftBot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameTags.TAG_LIGHT_UW))
        {
            isHightLightBonusGold = true;
        }
        if (collision.CompareTag(GameTags.TAG_LIGHT_GoldenAirCaft))
        {
            isBonusGoldAircraftBot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_LIGHT_UW))
        {
            isHightLightBonusGold = false;
        }
        if(collision.CompareTag(GameTags.TAG_LIGHT_GoldenAirCaft))
        {
            isBonusGoldAircraftBot = false;
        }
    }

    public void DropGoldBySHockWave(double gold)
    {
        EventChallengeListenerManager.KillWithShockwave(1);
        EventDispatcher.PostEvent(EventID.EnemyDestroyedByShockwave, 0);
        //drop gold bonus
        float goldBonusShockwave = Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.SHOWCKWAVE_GOLD_BONUS).GetCurrentProperty();

        float goldRound = Mathf.Floor(goldBonusShockwave);

        var pos = transform.position;
        var posGold = pos + Extensions.GetRandomPosition(0.5f);
        var amount = 1 + goldRound;
        PoolCtrl.instance.Get(PoolTag.CURRENCY_GOLD, posGold, Quaternion.identity, "+" + amount);

        GPm.GoldInGame += (int)amount;
        GameDatas.Gold += (int)amount;
    }

    private float resourceBonus = 0;

    public void CastedLight(float bonus)
    {
        resourceBonus = bonus;
    }

    public void OutLight()
    {
        resourceBonus = 0;
    }

    #endregion
}
