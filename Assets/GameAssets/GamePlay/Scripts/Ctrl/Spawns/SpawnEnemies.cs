using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemies : GameMonoBehaviour
{

    private Object_Pool temp;
    private Vector3 posRandom;
    private int wave = 0;
    [SerializeField]
    private int countEnemies;
    private int countEnemiesKilled;
    public int CountEnemies { get => countEnemies; set => countEnemies = value; }

    private Transform _tower;
    [SerializeField]
    private float rangeSpawn = 8f;

    [SerializeField]
    [Title("Boss")]
    [SerializedDictionary("type world", "PoolTag")]
    private SerializedDictionary<WorldType, PoolTag> bossDict = new();

    [Space]
    [Title("Enemies Spawner")]
    [SerializeField]
    private SerializedDictionary<WorldType, SO_SpawnEnemyManager> enemySpawnRulesNew = new();

    [Space]
    [Title("Gold and Sliver EnemyDrop")]
    [SerializeField]
    [SerializedDictionary("TypeMonster", "EnemyDropGoldOrSliver")]
    private SerializedDictionary<TypeMonster, EnemyDropGoldOrSliver> goldOrSliverDrop = new();

    private float firstTimeSpawn = 1.5f;
    [SerializeField]
    private SO_BaseDataEnemySpawn sO_BaseDataEnemySpawn;
    private BaseDataInforEnemy baseDataInforEnemy;

    private bool isSpawnBoss = false;

    private WorldType worldType;

    private Dictionary<TypeMonster, EnemyDmgAndHP> dmgAndHpBaseEnemies = new();

    private Dictionary<TypeMonster, List<PoolTag>> enemyPools = new();
    public float totalEnemyTest = 0;

    private void Awake()
    {
        InitDataSpawner();
    }

    void Start()
    {
        EventDispatcher.AddEvent(EventID.OnEnemyKilled, OnEnemyKilled);
        _tower = GPm.Tower.transform;

        /*if (GameDatas.IsTut_PlayDemo)
        {
            StartCoroutine(SpawnEnemiesTutorialInGame());
            //tutorial
            StartCoroutine(IEActiveTutorialFirstInGame());
        }
        else
        {
            StartCoroutine(SpawnEnemiesInGameNew());
            //tutorial
            StartCoroutine(IE_TriggerFirstKilled());
        }*/
        StartCoroutine(SpawnEnemiesInGameNew());
        //tutorial
        StartCoroutine(IE_TriggerFirstKilled());
    }

    #region Tutorial

    private IEnumerator IE_TriggerFirstKilled()
    {
        if (GameDatas.IsTut_Play)
        {
            TutorialManager.instance.StartTutorial_buildBase();
            GameDatas.IsTut_Play = false;
        } else
        {
            GPm.time_play_seconds = Time.unscaledTime;
        }

        yield return new WaitUntil(() => countEnemiesKilled >= 1);
        if (!GameDatas.IsTut_0_Upgrade)
        {
            GameDatas.IsTut_0_Upgrade = true;
            TutorialManager.instance.StartTutorial_buildBase1();
        }
    }

    private IEnumerator IEActiveTutorialFirstInGame()
    {
        yield return Yielders.Get(2f);
        TutorialManager.instance.StartTutorialPlayDemo();
    }
    #endregion

    #region Init

    private void InitDataSpawner()
    {
        worldType = (WorldType)GameDatas.CurrentWorld;
        GPm.worldPlaying = GameDatas.CurrentWorld;
        baseDataInforEnemy = sO_BaseDataEnemySpawn.GetBaseDataInforEnemy(worldType);

        //enemy spawner
        enemyPools[TypeMonster.Normal] = enemySpawnRulesNew[worldType].NormalEnemys;
        enemyPools[TypeMonster.Fast] = enemySpawnRulesNew[worldType].FastEnemys;
        enemyPools[TypeMonster.Tank] = enemySpawnRulesNew[worldType].TankEnemys;
        enemyPools[TypeMonster.Ranged] = enemySpawnRulesNew[worldType].RangedEnemys;
        enemyPools[TypeMonster.Guarden] = enemySpawnRulesNew[worldType].GuardenEnemys;
    }

    #endregion

    #region Spawn Monster

    private IEnumerator SpawnEnemiesTutorialInGame()
    {
        int level_hard = 27;
        wave = 0;

        yield return Yielders.Get(firstTimeSpawn);

        while (true)
        {
            wave++;
            level_hard++;

            bool hasBoss = (level_hard % 10 == 0);
            int bigWave = (level_hard / 10) * 10;
            bool hasFastEnemies = (level_hard == (6 + bigWave) || level_hard == (9 + bigWave));

            int maxEnemies = 7 + Mathf.Min(level_hard, 20) * 6;
            CountEnemies = maxEnemies + (hasBoss ? 1 : 0) + (hasFastEnemies ? 5 : 0);

            GPm.wavePlaying = wave;
            EventDispatcher.PostEvent(EventID.OnRefreshUIWave, CountEnemies);

            float spawnDelay = 1f / (1 + (level_hard - 1) * 0.2f);

            if (hasFastEnemies)
            {
                float fastEnemySpawnDelay = 1.2f / (1 + level_hard * 0.1f) / 2;
                for (int i = 0; i < 5; i++)
                {
                    CreateEnemyByWorld(TypeMonster.Tank);
                    yield return Yielders.Get(fastEnemySpawnDelay);
                }
            }

            for (int i = 0; i < maxEnemies; i++)
            {
                CreateEnemyByWorld(TypeMonster.Normal);

                if (hasBoss && i == (int)(maxEnemies * 2f / 3))
                {
                    //tutorial phase 2
                    if (GameDatas.IsTut_PlayDemo)
                    {
                        GameDatas.IsEndTutorial = true;
                        GameDatas.ResetUpgraderTowerDefault();
                        TutorialManager.instance.StartTutorialPlayDemo2();
                    }
                    //var boss = CreateBoss();
                   // InitInformationTutorialBoss(boss, bonusMaxHp: 1000000f, bonusDmg: 300f);
                }

                yield return Yielders.Get(spawnDelay);
            }

            yield return new WaitUntil(() => CountEnemies <= 0);
            DebugCustom.LogColor("End wave");

            if (wave == 1)
                TutorialManager.instance.StartTutorialPlayDemo1();
        }
    }

    private static readonly Dictionary<int, (int fastRate, int tankRate, int rangedRate, int guardianRate, int baseSpawnRate)> WaveData =
        new Dictionary<int, (int, int, int, int, int)>
        {
        { 2, (5, 0, 0, 0, 10) },
        { 5, (5, 2, 0, 0, 11) },
        { 19, (6, 4, 1, 0, 13) },
        { 39, (7, 6, 2, 0, 15) },
        { 59, (8, 7, 3, 0, 17) },
        { 79, (9, 8, 4, 0, 19) },
        { 99, (10, 8, 5, 1, 20) },
        { 149, (10, 9, 6, 1, 22) },
        { 159, (11, 10, 6, 1, 24) },
        { 199, (11, 10, 6, 2, 24) },
        { 249, (11, 11, 7, 2, 26) },
        { 299, (12, 12, 7, 2, 28) },
        { 319, (12, 13, 8, 2, 30) },
        { 399, (12, 13, 8, 3, 30) },
        { 599, (13, 13, 9, 3, 32) },
        { 749, (13, 14, 10, 3, 34) },
        { 799, (13, 14, 10, 4, 34) },
        { 999, (13, 14, 11, 4, 36) },
        { 1249, (14, 15, 11, 4, 37) },
        { 1499, (15, 16, 11, 4, 38) },
        { 1999, (15, 16, 14, 4, 39) },
        { 2499, (17, 17, 14, 4, 40) },
        { 2999, (18, 18, 15, 4, 42) },
        { 3499, (19, 19, 16, 4, 44) },
        { 3999, (20, 19, 17, 4, 46) },
        { 4499, (21, 20, 18, 4, 48) },
        { 4999, (21, 20, 19, 4, 49) },
        { 5499, (22, 20, 19, 4, 50) },
        { 5999, (23, 21, 19, 4, 52) },
        { 6499, (24, 21, 20, 4, 54) },
        { int.MaxValue, (24, 22, 21, 4, 56) } // Wave >= 6500
        };


    private static readonly Dictionary<int, float> additiveThresholdsAttack = new Dictionary<int, float>
    {
        { 5, 0.02f }, { 10, 0.025f }, { 25, 0.012f }, { 50, 0.02f },
        { 100, 0.02f }, { 200, 0.025f }, { 900, 0.02f }
    };

    private static readonly Dictionary<int, float> multiplicativeThresholdsAttack = new Dictionary<int, float>
    {
        { 30, 1.005f }, { 72, 1.01f }, { 83, 1.01f },  { 94, 1.01f }, { 107, 1.01f },
        { 200, 1.02f }, { 400, 1.02f },  { 900, 1.035f }, { 1024, 1.05f },
    };

    private static readonly Dictionary<int, float> enemyHpAdditiveThresholdsHP = new Dictionary<int, float>
    {
        { 5, 0.04f }, { 10, 0.05f }, { 25, 0.06f }, { 50, 0.08f },
        { 60, 0.1f }, { 72, 0.18f }, { 83, 0.2f }, { 94, 0.21f },
        { 100, 0.12f }, { 107, 0.1f }, { 200, 0.15f }, { 900, 0.35f }
    };

    private static readonly Dictionary<int, float> enemyHpMultiplicativeThresholdsHP = new Dictionary<int, float>
    {
        { 30, 1.035f }, { 60, 1.02f }, { 72, 1.025f }, { 83, 1.03f },
        { 94, 1.03f }, { 100, 1.02f }, { 107, 1.02f }, { 139, 1.11f },
        { 182, 1.11f }, { 200, 1.03f }, { 241, 1.13f }, { 332, 1.13f },
        { 400, 1.06f }, { 900, 1.15f }, { 1024, 1.15f }
    };

    private static readonly Dictionary<float, (float waveLength, float cooldown, float spawnFactor)> speedSettings =
    new Dictionary<float, (float, float, float)>
    {
            { 1.0f, (26.00f, 9.00f, 1.00f) },
            { 1.5f, (17.33f, 6.00f, 1.10f) },
            { 2.0f, (13.00f, 4.50f, 1.20f) },
            { 2.5f, (10.40f, 3.60f, 1.30f) },
            { 3.0f, (8.67f, 3.00f, 1.40f) },
            { 3.5f, (7.43f, 2.57f, 1.50f) },
            { 4.0f, (6.50f, 2.25f, 1.60f) },
            { 4.5f, (5.78f, 2.00f, 1.70f) },
            { 5.0f, (5.20f, 1.80f, 1.80f) }
    };



    #region Time Spawn

    private float baseSpawnTime = 1.0f; 

    public float GetTimeSpawn()
    {
        float gameSpeed = TimeGame.TimeGameplay;
        float spawnTime = GetSpawnTime(wave, gameSpeed);
        float waveDuration = GetWaveLength(gameSpeed);
        float cooldown = GetCooldown(gameSpeed);

        return spawnTime;
    }

    private float GetSpawnTime(int wave, float speed)
    {
        float waveFactor = 1.0f / (1.0f + Mathf.Pow(wave - 1, 1.2f) * 0.1f);
        return baseSpawnTime * waveFactor * GetSpeedModifier(speed);
    }

    private float GetSpeedModifier(float speed)
    {
        return speedSettings.ContainsKey(speed) ? speedSettings[speed].spawnFactor : 1.0f;
    }

    private float GetWaveLength(float speed)
    {
        return speedSettings.ContainsKey(speed) ? speedSettings[speed].waveLength : 26.0f;
    }

    private float GetCooldown(float speed)
    {
        return speedSettings.ContainsKey(speed) ? speedSettings[speed].cooldown : 9.0f;
    }
    #endregion

    public int testWave = 9;
    private IEnumerator SpawnEnemiesInGameNew()
    {
        wave = 0;

        if(Gm.isResumeWave)
        {
            wave = GameDatas.GetWaveInResume(GameDatas.CurrentWorld) == 0 ? 0 :
                GameDatas.GetWaveInResume(GameDatas.CurrentWorld) - 1;
            EventDispatcher.PostEvent(EventID.OnRefreshTextWaveUI, 0);
        }

        yield return Yielders.Get(firstTimeSpawn);

        while (true)
        {
            wave++;
            GPm.isTowerTakeDmg = false;
            if (wave == 4)
            {
                if (!GameDatas.isTutBuildPhase2)
                {
                    TutorialManager.instance.StartTutorial_buildBase2();
                    GameDatas.isTutBuildPhase2 = true;
                }
            }

            countEnemies = 0;
            QuestEventManager.WavesCleared(1);
            QuestEventManager.TotalWavesCleared(1);

            if (wave >= 40)
            {
                GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.PASS_WAVE_40, GameDatas.GetBeginnerQuestProgress(BeginnerQuestID.PASS_WAVE_40) + 1);
                GameDatas.IsFirstDoneWave40 = true;
            }

            bool hasBoss = (wave % 10 == 0);

            GetWaveRates(wave, out int fastRate, out int tankRate, out int rangedRate, out int guariandRate, out int baseSpawnRate);

            float spawnRate = GameDatas.IsFirstDoneWave40 ? baseSpawnRate * 3.6f : baseSpawnRate * 2f;

            int normalCount = Mathf.FloorToInt(((100 - (fastRate + tankRate + rangedRate)) * spawnRate / 100));
            int fastCount = Mathf.FloorToInt((fastRate * spawnRate) / 100);
            int tankCount = Mathf.FloorToInt((tankRate * spawnRate) / 100);
            int rangeCount = Mathf.FloorToInt((rangedRate * spawnRate) / 100);

            var statCardSpawnFaster = Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_SPAWN_FASTER);
            var statCardSpawnSlower = Cfg.cardCtrl.GetCurrentStat(CardID.COMMON_SPAWN_SLOWER);
            var spawnMultiplier = (1 - statCardSpawnFaster / 100f) * (1 + statCardSpawnSlower / 100f);

            int totalEnemies = normalCount + fastCount + tankCount + rangeCount;
            countEnemies = totalEnemies + (hasBoss ? 1 : 0);

            totalEnemyTest += countEnemies;
            DebugCustom.LogColor("totalEnemyTest " + totalEnemyTest);

            //DebugCustom.LogColor($"Wave: {wave}, Normal: {normalCount}, Fast: {fastCount}, Tank: {tankCount}, Ranged: {rangeCount}, Boss: {hasBoss}");
            //DebugCustom.LogColor($"countEnemies {countEnemies}");

            GPm.wavePlaying = wave;
            EventDispatcher.PostEvent(EventID.OnRefreshUIWave, CountEnemies);

            // === Danh sách quái cần spawn ===
            List<TypeMonster> enemyList = new List<TypeMonster>();
            enemyList.AddRange(Enumerable.Repeat(TypeMonster.Normal, normalCount));
            enemyList.AddRange(Enumerable.Repeat(TypeMonster.Fast, fastCount));
            enemyList.AddRange(Enumerable.Repeat(TypeMonster.Tank, tankCount));
            enemyList.AddRange(Enumerable.Repeat(TypeMonster.Ranged, rangeCount));

            System.Random rng = new System.Random();
            enemyList = enemyList.OrderBy(x => rng.Next()).ToList();

            Queue<TypeMonster> enemyQueue = new Queue<TypeMonster>(enemyList);
            float baseTime = GetTimeSpawn() * spawnMultiplier;

            if (hasBoss)
            {
                CreateBoss();
            }

            while (enemyQueue.Count > 0)
            {
                TypeMonster type = enemyQueue.Dequeue();
                CreateEnemy(type);
                yield return Yielders.Get(baseTime);
            }

            yield return new WaitUntil(() => countEnemies <= 0);

            GameAnalytics.LogEventPlay(GameDatas.CurrentWorld, wave);
            GameDatas.SetHighestWaveInWorld(GameDatas.CurrentWorld, wave);
            DebugCustom.Log("End Wave");
            GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.PASS_5_WAVE, GameDatas.GetBeginnerQuestProgress(BeginnerQuestID.PASS_5_WAVE) + 1);
            EventChallengeListenerManager.TotalWavesCleared(1);
            EventDispatcher.PostEvent(EventID.FinishWave, null);

            if(!GPm.isTowerTakeDmg)
            {
                EventChallengeListenerManager.NoDamageAfterWave(1);
            }
            if(Cfg.cardCtrl.currentIndexSet == 0)
            {
                EventChallengeListenerManager.PlayWaveWithoutCard(1);
            }
        }
    }

    private void GetWaveRates(int wave, out int fastRate, out int tankRate, out int rangedRate, out int guardianRate, out int baseSpawnRate)
    {
        foreach (var entry in WaveData)
        {
            if (wave <= entry.Key)
            {
                (fastRate, tankRate, rangedRate, guardianRate, baseSpawnRate) = entry.Value;
                return;
            }
        }

        fastRate = 0; tankRate = 0; rangedRate = 0; guardianRate = 0; baseSpawnRate = 10;
    }

    void OnEnemyKilled(object o)
    {
        CountEnemies -= 1;
        countEnemiesKilled += 1;
    }

    #endregion

    #region Init Boss

    private Object_Pool CreateBoss()
    {
        return GetBoss(bossDict[worldType]);
    }

    [Button("Test")]
    public void TranslationInforBoss()
    {
        GetBoss(PoolTag.BOSS0);
    }

    private Object_Pool GetBoss(PoolTag poolTag)
    {
        posRandom = _tower.transform.position + Extensions.GetRandomPosition(rangeSpawn);
        temp = PoolCtrl.instance.Get(poolTag, posRandom, Quaternion.identity);

        InitInformationBoss(temp, baseDataInforEnemy);
        return temp;
    }

    private void InitInformationBoss(Object_Pool boss, BaseDataInforEnemy baseData)
    {
        var data_boss = boss.GetComponent<EnemyData>();

        if (!isSpawnBoss)
        {
            isSpawnBoss = true;
            data_boss.spdMove = baseData.speedMove;
            data_boss.spdMove *= 0.3f; // Speed = 30%
            data_boss.maxHP = baseData.maxHp;
            data_boss.BonusMaxHP = 1;
            data_boss.maxHP *= 10f;
        }

        //data_boss.maxHP += 2f;

        //data_boss.maxHP *= 20f; // HP x20
/*        DebugCustom.LogColor("baseData.maxHp "+ baseData.maxHp);
        DebugCustom.LogColor("data_boss.maxHP " + data_boss.maxHP);*/

        data_boss.goldDrop = goldOrSliverDrop[TypeMonster.Boss].gold * baseData.goldWorldBase;
        data_boss.sliverDrop = goldOrSliverDrop[TypeMonster.Boss].sliver;

/*        DebugCustom.LogColor("maxHP base " + baseData.maxHp);
        DebugCustom.LogColor("maxHP " + data_boss.maxHP);
        DebugCustom.LogColor("Speed " + data_boss.spdMove);*/

        SetDmdAndHpEnemyBase(TypeMonster.Boss, data_boss.damage, data_boss.maxHP);

        DmgOrHPBossTranslation(TypeMonster.Boss, GameDatas.CurrentWorld, data_boss, baseData);
        GoldAndSliverTranslation(data_boss, TypeMonster.Boss);
    }

    private void DmgOrHPBossTranslation(TypeMonster typeMonster, int worldType, EnemyData data_enemy, BaseDataInforEnemy baseData)
    {
        int world = GameDatas.CurrentRank + 1;

        float worldDifficultyMultiplier = CalculateWorldDifficultyMultiplier(world);
        float attackMultiplier = GetEnemyStatMultiplier(GPm.wavePlaying, additiveThresholdsAttack, multiplicativeThresholdsAttack);
        float hpMultiplier = GetEnemyStatMultiplier(GPm.wavePlaying, enemyHpAdditiveThresholdsHP, enemyHpMultiplicativeThresholdsHP);

        /*        float baseAttack = dmgAndHpBaseEnemies[typeMonster].dmg;
                float baseHP = dmgAndHpBaseEnemies[typeMonster].maxHP;*/
        float baseAttack = CalculateBaseAttack(world, GPm.wavePlaying);
        float baseHP = CalculateBaseHp(world, GPm.wavePlaying);

        float finalAttack = baseAttack * worldDifficultyMultiplier * 0.35f/*attackMultiplier*/;
        float finalHP = baseHP * worldDifficultyMultiplier * 0.35f/*hpMultiplier*/;

        data_enemy.maxHP += finalHP;
        data_enemy.damage += finalAttack;

        SpeedMoveTranslation(data_enemy);
    }

    private void InitInformationTutorialBoss(Object_Pool boss, float bonusMaxHp, float bonusDmg)
    {
        var data_boss = boss.GetComponent<EnemyData>();
        data_boss.BonusMaxHP = bonusMaxHp;
        data_boss.BonusDmg = bonusDmg;
        var bossTakeDmg = boss.GetComponent<EnemyTakeDamage>();
        bossTakeDmg.isImmortal = true;
    }

    #endregion

    #region Init Enemy

    [Button("Test Enemy")]
    public void TranslationInforEnemy()
    {
        CreateEnemyByWorld(TypeMonster.Normal);
    }

    private void CreateEnemyByWorld(TypeMonster typeMonster)
    {
        CreateEnemy(typeMonster);
    }

    private void CreateEnemy(TypeMonster typeMonster)
    {
        if (enemyPools.TryGetValue(typeMonster, out var enemyList) && enemyList.Count > 0)
        {
            PoolTag poolTag = enemyList[Random.Range(0, enemyList.Count)];
            GetEnemy(poolTag, typeMonster);
        }
    }

    private Object_Pool GetEnemy(PoolTag poolTag, TypeMonster typeMonster = TypeMonster.Normal)
    {
        posRandom = _tower.transform.position + Extensions.GetRandomPosition(rangeSpawn);
        temp = PoolCtrl.instance.Get(poolTag, posRandom, Quaternion.identity);

        InitInformationEnemy(temp, baseDataInforEnemy, typeMonster);
        return temp;
    }

    // base gold
    private double GoldEnemyBase(TypeMonster typeMonster)
    {
        return goldOrSliverDrop[typeMonster].gold * baseDataInforEnemy.goldWorldBase;
    }

    // base sliver
    private double SliverEnemyBase(TypeMonster typeMonster)
    {
        return goldOrSliverDrop[typeMonster].sliver;
    }

    private void InitInformationEnemy(Object_Pool pool, BaseDataInforEnemy baseData, TypeMonster typeMonster)
    {
        var data_enemy = pool.GetComponent<EnemyData>();

        data_enemy.maxHP = baseData.maxHp;
        data_enemy.damage = baseData.damage;
        data_enemy.attackRange = baseData.attackRange;
        data_enemy.attackSpeed = baseData.attackSpeed;
        data_enemy.spdMove = baseData.speedMove;
        data_enemy.goldDrop = goldOrSliverDrop[typeMonster].gold * baseData.goldWorldBase;
        data_enemy.sliverDrop = goldOrSliverDrop[typeMonster].sliver;
        data_enemy.typeMonster = typeMonster;

        switch (typeMonster)
        {
            case TypeMonster.Normal:
                break; // Không thay đổi

            case TypeMonster.Fast:
                if (dmgAndHpBaseEnemies.ContainsKey(typeMonster)) return;
                data_enemy.spdMove *= 2f;   // Tốc độ x2
                data_enemy.maxHP *= 0.75f; // HP còn 75%
                break;

            case TypeMonster.Tank:
                if (dmgAndHpBaseEnemies.ContainsKey(typeMonster)) return;
                data_enemy.spdMove *= 0.5f;  // Giảm 50% tốc độ
                data_enemy.maxHP *= 5f;  // HP x5

                break;

            case TypeMonster.Ranged:
                if (dmgAndHpBaseEnemies.ContainsKey(typeMonster)) return;
                data_enemy.attackRange = TowerCtrl.instance.TowerData.attackRange / 2; // Attack Range bằng 1/2 trụ
                data_enemy.maxHP *= 0.6f; // HP = 60%
                break;
            default:
                break;
        }

        SetDmdAndHpEnemyBase(typeMonster, data_enemy.damage, data_enemy.maxHP);

        DmgOrHPTranslation(typeMonster, GameDatas.CurrentWorld, data_enemy, baseData);
        GoldAndSliverTranslation(data_enemy, typeMonster);
    }

    private void SetDmdAndHpEnemyBase(TypeMonster typeMonster, float dmg, float maxHp)
    {
        if (dmgAndHpBaseEnemies.ContainsKey(typeMonster)) return;
        EnemyDmgAndHP data = new(dmg, maxHp);
        dmgAndHpBaseEnemies[typeMonster] = data;
    }

    #endregion

    //public int currentWorld = 1;
    //public int wave1 = 1;
    /*    #region tinh tien suc manh enemy
        [Button("Test DmgOrHPTranslation")]
        private void TestDmgOrHPTranslation()
        {
            float worldDifficultyMultiplier = CalculateWorldDifficultyMultiplier(currentWorld);
            float attackMultiplier = GetEnemyStatMultiplier(wave1, attackThresholds);
            float hpMultiplier = GetEnemyStatMultiplier(wave1, hpThresholds);

            float baseAttack = 1;
            float baseHP = 20;

            float finalAttack = baseAttack * worldDifficultyMultiplier * attackMultiplier;
            float finalHP = baseHP * worldDifficultyMultiplier * hpMultiplier;

            DebugCustom.LogColor($"Attack = {finalAttack}, HP = {finalHP}");
        }*/
    #region tinh tien suc manh enemy
    private static readonly Dictionary<int, float> worldDifficultyMultiplierDict = new Dictionary<int, float>
    {
        { 1, 1.00f }, { 2, 1.00f }, { 3, 1.00f }, { 4, 1.00f },
        { 5, 1.05f }, { 6, 1.11f }, { 7, 1.20f }, { 8, 1.38f },
        { 9, 1.75f }, { 10, 4.30f }, { 11, 41.00f }, { 12, 410.00f },
        { 13, 9000.00f }, { 14, 5000.00f }, { 15, 1000.00f }
    };

    private void DmgOrHPTranslation(TypeMonster typeMonster, int worldType, EnemyData data_enemy, BaseDataInforEnemy baseData)
    {
        int world = GameDatas.CurrentWorld + 1;
        float worldDifficultyMultiplier = (CalculateWorldDifficultyMultiplier(world) 
            * worldDifficultyMultiplierDict[world]);
/*        float attackMultiplier = GetEnemyStatMultiplier(GPm.wavePlaying, additiveThresholdsAttack, multiplicativeThresholdsAttack);
        float hpMultiplier = GetEnemyStatMultiplier(GPm.wavePlaying, enemyHpAdditiveThresholdsHP, enemyHpMultiplicativeThresholdsHP);*/

/*        float baseAttack = dmgAndHpBaseEnemies[typeMonster].dmg;
        float baseHP = dmgAndHpBaseEnemies[typeMonster].maxHP;*/
        float baseAttack = CalculateBaseAttack(world, GPm.wavePlaying);
        float baseHP = CalculateBaseHp(world, GPm.wavePlaying);

        float finalAttack = baseAttack * worldDifficultyMultiplier * 0.35f/*attackMultiplier*/;
        float finalHP = baseHP * worldDifficultyMultiplier * 0.35f/*hpMultiplier*/;

        data_enemy.maxHP = finalHP;
        data_enemy.damage = finalAttack;

/*        DebugCustom.LogColor("=======================================");
        DebugCustom.LogColor("data_enemy.maxHP " + data_enemy.maxHP);
        DebugCustom.LogColor("data_enemy.damage " + data_enemy.damage);
        DebugCustom.LogColor("=======================================");*/
        SpeedMoveTranslation(data_enemy);
    }

    //setup BaseHP
    #region SetUpBase Hp Or Attack

    //HP
    private readonly Dictionary<int, float> healthExponentialFactors = new Dictionary<int, float>
    {
        { 1, 2.13f }, { 2, 2.13f }, { 3, 2.13f }, { 4, 2.13f }, { 5, 2.13f },
        { 6, 2.13f }, { 7, 2.13f }, { 8, 2.13f }, { 9, 2.13f }, { 10, 2.14f },
        { 11, 2.15f }, { 12, 2.23f }, { 13, 2.35f }, { 14, 2.49f }, { 15, 2.64f }
    };

    public float CalculateBaseHp(int world, int currentWave, float healthPreExponentialFactor = 0.05f, float healthMultFactor = 0.8f)
    {
        if (!healthExponentialFactors.TryGetValue(world, out float healthExponentialFactor))
        {
            healthExponentialFactor = 2.13f; // Mặc định nếu World không hợp lệ
        }

        float baseHp = healthPreExponentialFactor * Mathf.Pow(currentWave, healthExponentialFactor)
                     + healthMultFactor * currentWave
                     + 1.5f;

        return baseHp;
    }

    //Attack
    private static readonly Dictionary<int, float> damageExponentialFactors = new Dictionary<int, float>
    {
        { 1, 2.007f }, { 2, 2.007f }, { 3, 2.007f }, { 4, 2.007f }, { 5, 2.007f },
        { 6, 2.007f }, { 7, 2.007f }, { 8, 2.007f }, { 9, 2.007f }, { 10, 2.009f },
        { 11, 2.011f }, { 12, 2.027f }, { 13, 2.052f }, { 14, 2.080f }, { 15, 2.109f }
    };

    public static float CalculateBaseAttack(int world, int currentWave, float damagePreExponentialFactor = 0.021f, float damageMultFactor = 0.16f)
    {
        if (!damageExponentialFactors.TryGetValue(world, out float damageExponentialFactor))
        {
            damageExponentialFactor = 2.109f;
        }

        float baseAttack = (damagePreExponentialFactor * Mathf.Pow(currentWave, damageExponentialFactor))
                         + (damageMultFactor * currentWave)
                         + 1.07f;

        return baseAttack;
    }

    #endregion

    private void SpeedMoveTranslation(EnemyData data_enemy)
    {
        data_enemy.spdMove = CalculateEnemySpeed(GPm.wavePlaying, data_enemy.spdMove);
    }

    float CalculateEnemySpeed(int wave, float speedBase)
    {
        float baseSpeed = speedBase;
        float speedIncrease = 0f;

        if (wave >= 100) speedIncrease += 0.1f;
        else if (wave >= 50) speedIncrease += 0.03f;
        else if (wave >= 20) speedIncrease += 0.01f;

        float adjustedSpeed = baseSpeed + speedIncrease;

        if (wave >= 64) adjustedSpeed *= 1.1f;

        return adjustedSpeed;
    }


    private float CalculateWorldDifficultyMultiplier(int world)
    {
        return world == 1 ? 1f : (1 + (world - 1) * 15.5f) * (Mathf.Pow(1.43f, world - 2) + 0.2f * (world - 1));
    }

    private float GetEnemyStatMultiplier(int wave, Dictionary<int, float> additiveThresholds, Dictionary<int, float> multiplicativeThresholds)
    {
        float additiveBonus = 0f; 
        float multiplier = 1f; 

        foreach (var threshold in additiveThresholds)
        {
            if (wave >= threshold.Key) 
                additiveBonus += threshold.Value;
        }

        foreach (var threshold in multiplicativeThresholds)
        {
            if (wave >= threshold.Key) 
                multiplier *= Mathf.Pow(1 + threshold.Value, wave / threshold.Key);
        }

        return (1 + additiveBonus) * multiplier; 
    }


    /*private float GetEnemyStatMultiplier(int wave, Dictionary<int, float> thresholds)
    {
        float multiplier = 1f;
        foreach (var threshold in thresholds)
        {
            multiplier *= Mathf.Pow(1 + threshold.Value, wave / threshold.Key);
        }
        return multiplier;
    }*/
    #endregion

    #region tinh tien gold or sliver drop enemy
    private double CalculateGold(double A, double K, int wave)
    {
        return A * Math.Pow(wave, K / 2);
    }

    private double CalculateSliver(double A, double K, int wave)
    {
        return A * Math.Pow(wave, K);
    }

    private double GetGrowthFactor(int wave)
    {
        double K = 0.2;

        if (wave >= 20) K += 0.1 * (wave / 20);
        if (wave >= 100) K += 0.02 * ((wave - 100) / 200);
        if (wave >= 1100) K += 0.003 * ((wave - 1100) / 200);
        if (wave >= 2100) K += 0.0004 * ((wave - 2100) / 200);

        return K;
    }

    private void GoldAndSliverTranslation(EnemyData data_enemy, TypeMonster typeMonster)
    {
        //double K = GetGrowthFactor(wave);
        //double gold = CalculateGold(GoldEnemyBase(typeMonster), K, wave);
        //double sliver = CalculateSliver(SliverEnemyBase(typeMonster), K, wave);

        //data_enemy.goldDrop = Extensions.CustomRound(gold);
        //data_enemy.sliverDrop = Extensions.CustomRound(sliver);
    }

    #endregion

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnEnemyKilled, OnEnemyKilled);
    }
}

[Serializable]
public class EnemyDropGoldOrSliver
{
    public int gold;
    public int sliver;
}

[Serializable]
public class EnemyDmgAndHP
{
    public float dmg;
    public float maxHP;

    public EnemyDmgAndHP(float dmg, float maxHP)
    {
        this.dmg = dmg;
        this.maxHP = maxHP;
    }
}

