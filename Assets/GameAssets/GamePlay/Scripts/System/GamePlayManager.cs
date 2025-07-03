using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [SerializeField]
    private TowerCtrl tower;
    public int wavePlaying;
    public int worldPlaying;

    private float gemInGame;
    private float goldInGame;
    private float sliverInGame;
    public float time_play_seconds;
    public bool isNoRegenHPTower;
    public bool isTowerTakeDmg;

    float statCardGold => Cfg.cardCtrl.GetCurrentStat(CardID.RARE_GOLD_RWD);
    float statCardSilver => Cfg.cardCtrl.GetCurrentStat(CardID.RARE_SILVER_RWD);
    float goldPerWave_Lab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.GOLD_PER_WAVE).GetCurrentProperty();
    float coinPerWave_Lab => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.COIN_PER_WAVE).GetCurrentProperty();

    float coinPerWave => GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_COIN_PER) == true ? 
        Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.coin_per_wave) : 0;
    float coinInterest => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.coin_interest);

    float sliver => GPm.sliverInGame % Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.INTEREST).GetCurrentProperty();
    float maxInterest => Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.MAX_INTEREST).GetCurrentProperty();

    public TowerCtrl Tower => tower;
    public float GemInGame
    {
        get { return gemInGame; }
        set
        {
            gemInGame = value;
            EventDispatcher.PostEvent(EventID.OnGemChanged_ingame, 0);
        }
    }

    public float GoldInGame
    {
        get { return goldInGame; }
        set
        {
            goldInGame = value;
            EventDispatcher.PostEvent(EventID.OnGoldChanged_ingame, 0);
        }
    }
    public float SliverInGame
    {
        get { return sliverInGame; }
        set
        {
            sliverInGame = value;
            EventDispatcher.PostEvent(EventID.OnSilverChanged_ingame, 0);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitTowerInGame();
    }

    private void Start()
    {
        Gui.UiParent.gameObject.SetActive(true);
        EventDispatcher.AddEvent(EventID.FinishWave, OnFinishWave);
    }

    private void InitTowerInGame()
    {
        tower = TowerCtrl.instance;
    }

    public void RevivalTower()
    {
        var towerPos = tower.transform.position;
        var objects = FindObjectsOfType<EnemyDestroy>();

        foreach (var item in objects)
        {
            item.Destroy(null);
            if(item.EnemyType == EnemyType.BOSS)
            {
                var distance = Vector3.Distance(item.transform.position, towerPos);
                if (distance <= 3f)
                {
                    var direction = (item.transform.position - towerPos).normalized;
                    item.transform.position += direction * (3f - distance);
                }
            }
        }
        tower.TowerRevival.Revival();
        //PoolCtrl.instance.ReturnAll();
    }

    public void EndRoundWave(bool isResume)
    {
        if (GameDatas.IsTut_PlayDemo) return;
        GameDatas.ResumeWave(GameDatas.CurrentWorld, isResume);
        GameDatas.SetWaveInResume(GameDatas.CurrentWorld, GPm.wavePlaying);
        GameDatas.SetWorldInResume(GPm.worldPlaying);
    }

    public void SpawnTextUpgrader(string infor, string id_Text)
    {
        float radius = Random.Range(3, 6);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        Vector3 spawnPos = tower.transform.position + new Vector3(Mathf.Cos(angle), 0, 0) * radius;
        PoolCtrl.instance.Get(PoolTag.TEXT_POP_WAVE_DONE, spawnPos, Quaternion.identity, "Up +" +infor);
    }

    private void OnDestroy()
    {
        var timePlay = (int)(Time.unscaledTime - time_play_seconds);
        GameAnalytics.LogEvent_TimePlay(timePlay);
        GameDatas.SetDataProfile(IDInfo.BattleTime, GameDatas.GetDataProfile(IDInfo.BattleTime) + timePlay);
    }

    private void OnFinishWave(object o)
    {
        float coinInterestBonus = 0;
        if (GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_COIN_INTEREST))
        {
            coinInterestBonus = Mathf.FloorToInt(((int)statCardSilver + (int)coinPerWave_Lab + coinPerWave) * coinInterest);
        }

        SliverInGame += ((int)statCardSilver + (int)coinPerWave_Lab + coinPerWave) + coinInterestBonus;
        GameDatas.SetDataProfile(IDInfo.TotalCoinsEarned, GameDatas.GetDataProfile(IDInfo.TotalCoinsEarned) + (int)statCardSilver);

        float goldPerWave = Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.gold_bonus_each_wave);

        var reward = goldPerWave + statCardGold + goldPerWave_Lab;
        GoldInGame += (int)reward;
        GameDatas.Gold += (int)reward;
        GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + (int)reward);

        SetSliverInterest();
    }

    private void SetSliverInterest()
    {
        //lab
        if (!GameDatas.IsClusterUnlockLabInfor(LabCategory.WORKSHOP_INTEREST)) return;
        int sliverCeil = Mathf.CeilToInt(sliver);
        if (sliverCeil <= maxInterest)
        {
            GPm.sliverInGame += sliverCeil;
        }
    }

    private void OnApplicationQuit()
    {
        EndRoundWave(true);
        GameDatas.ActiveWelcomeBack(true);
    }
}
