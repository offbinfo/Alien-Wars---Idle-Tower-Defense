using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag
{
    ENEMY0,
    ENEMY1,
    ENEMY2,
    ENEMY3,
    ENEMY4,
    ENEMY5,
    ENEMY6,
    ENEMY7,
    ENEMY8,
    ENEMY9,
    BOSS0,
    BOSS1,
    BOSS2,
    BOSS3,
    BOSS4,
    TEXT_POP,
    BULLET0,
    BULLET_HIT,
    EXPLOSIVE1, 
    EXPLOSIVE2,
    EXPLOSIVE3,
    BOMB,
    SATELLITE,
    CURRENCY_GOLD,
    CURRENCY_SLIVER,
    SHOCKWAVE,
    MISSILE,
    TEXT_POP_HAPPY,
    TEXT_POP_WAVE_DONE,
    ENEMY10,
    ENEMY11,
    ENEMY12,
    ENEMY13,
    ENEMY14,
    ENEMY15,
    BULLET_ENEMY0,
    NOVA_FIRE,
    EXPLOSIVE_SHIELD,
    VOID_NEXUS,
    BULLET_THUNDER,
    ENEMY_ARENA_0,
    ENEMY_ARENA_1,
    ENEMY_ARENA_2,
    BOSS_ARENA_0,
    ENEMY_ARENA_3,
    BLOOD_ENEMY,
    THUNDER_LIGHT,
    BEEN_STRUCK,
    AIRCAFT,
    ENEMY_GUARDEN_0,
    ENEMY_CRIMSON_0,
    ENEMY_PHOTON_0,
    ENEMY_RIPPER_0,
}

public enum BeginnerQuestID
{
    PASS_5_WAVE,
    UPGRADE_DAMAGE_20,
    KILL_3_BOSS,
    KILL_100_CREEP,
    REMAIN_30M_LOGIN,
    UNLOCK_SATELLITES,
    UNLOCK_GOLD_BONUS,
    LOGIN_2_DAYS,
    UPGRADE_ATK_SPD_1_5,
    UPGRADE_HEALTH_TO_20,
    UNLOCK_1_MORE_SLOT_CARD,
    FIRST_DEPOSIT,
    PASS_WAVE_40,
}

public enum BonusTypeRelic
{
    Coins,
    TowerDamage,
    TowerHealth,
    DefenseResistance,
    CritFactor,
    LabSpeed,
    DamagePerMeter,
    Health,
    Damage
}

public enum MissionCategory
{
    Static, // Nhiệm vụ tĩnh
    Dynamic // Nhiệm vụ động
}

public enum MissionChallengeType
{
    LoginDays,                     
    PlayArenaTimes,                
    WatchVideoTimes,             
    UpgradeLabTotalDays,          
    ClaimFreeGemTimes,             
    NoDamageAfterWave,            
    BuyCards,                      
    PlayWaveWithoutCard,           
    CompleteDailyMissions,         
    UpgradeWorkshop,               
    DealCriticalDamageEnemies,     
    ReachWaveAnyWorld,             
    KillFastEnemies,             
    KillBosses,                  
    KillBossesOutOfRange,        
    KillNormalEnemies,           
    KillTankEnemies,           
    KillRangeEnemies,              
    KillWithTomahawk,             
    KillWithShockwave,            
    KillInHighlight,          
    EnemiesInVoidNexus,        
    KillDuringGoldenSanctuary,     
    KillWithMine,                 
    KillWithSatellite,           
    SkipWaves,                 
    PlayForHours                   
}

public enum DamageType
{
    NORMAL,
    CRIT,
}

public enum TypeMileStoneCateGory
{
    STANDARD,
    PREMIUM,
}

public enum BotDifficulty
{
    Easy,
    Mid,
    Hard
}

public enum NameUserType
{
    Yasuooo, Zedd, LeeSin, Jinx, Viego, Thresh, Ezreal, Darius,
    ShadowLiam, FrostNoah, NightEmma, BlazeSofia, PhantomLuca, ThunderOliver, SniperMarta, GhostElena, IceJohan, StormMateo,
    RyuHiroshi, SakuraBlade, JisooX, MinhoDrift, AnanyaStrike, ArjunSlayer, WeiToxic, MeiCyber, AhmadRogue, SitiFury,
    KwameVortex, AminaVenom, ZuberiReaper, ThandiDagger, OmarSphinx, FatimaNova, AbebeHavoc, TendaiSniper, BakariTitan, NiaStealth,
    EthanRampage, IsabellaXtreme, MateoNuke, CamilaBullet, DiegoInferno, ValentinaChaos, LucasGlitch, JoséOblivion, EmiliaLegend, JuanRift,
    JackStorm, IslaPhantom, TaneRogue, MoanaSavage, AriSpectre, LaniFlash,
    MohammedRage, LaylaNemesis, YousefDagger, ZahraStrike,
    AlphaHunter, CyberPhoenix, ShadowReaper, MysticBlaze, CrimsonFury, NeonSpecter, SteelVortex, ArcaneWraith, TitanStorm, InfernoShade,
    PhantomKnight, OmegaRider, GhostSniper, ChaosStriker, DarkNova, FrostFang, ThunderDrake, LunarSamurai, VenomShroud, SolarBane,
    StormWarrior, EchoSpecter, AzureDagger, RavenFang, ObsidianFlash, MidnightWraith, TempestRogue, EmberShadow, DraconisRift, VoidAssassin,
    BlitzFury, RadiantVortex, OnyxPhantom, SavageHawk, NightShade, CyberStriker, ArcaneBerserker, TitanSlayer, InfernoKnight, CelestialBane,
    EmberKnight, FrostGladiator, SolarPhantom, MysticHavoc, ShadowInferno, NeonRampage, SpectralBlade, CrimsonReaver, ThunderPhantom, DuskSlayer, Nghia
}

public enum CurrencyType
{
    GOLD,
    GEM,
    POWER_STONE,
    BADGES,
    ARMOR_SPHERE,
    POWER_SPHERE,
    ENGINE_SPHERE,
    CRYOGENIC_SPHERE,
}

public enum WorldType
{
    WORLD1 = 0,
    WORLD2 = 1,
    WORLD3 = 2,
    WORLD4 = 3,
    WORLD5 = 4,
    WORLD6 = 5,
    WORLD7 = 6,
    WORLD8 = 7,
    WORLD9 = 8,
    WORLD10 = 9,
    WORLD11 = 10,
    WORLD12 = 11,
    WORLD13 = 12,
}

public enum EnemyAttackType
{
    Melee,   // Đánh gần
    Ranged,  // Đánh xa
    Hybrid   // Có thể vừa đánh xa vừa đánh gần
}

public enum TypeMonster
{
    Normal,
    Fast,
    Tank,
    Ranged,
    Boss,
    Guarden,
    Crimson,
    Photon,
    Ripple,
}

public enum TypeXBonusGold
{
    x3 = 3,
    x4 = 4,
    x5 = 5,
    x7 = 7,
    x10 = 10,
    x15 = 15,
    x20 = 20,
}

public enum EnemyType
{
    ENEMY,
    BOSS
}

public enum ResourceType
{
    GOLD,
    GEM,
    POWER_STONE
}


public enum TypeSkinTower
{
    GOLD,
    GEM,
    POWER_STONE
}

public enum UpgraderGroupID
{
    ATTACK,
    DEFENSE,
    RESOURCE,
    SATELLITE,
}

public enum LabCategory
{
    WORKSHOP_BASE,
    WORKSHOP_CRETICAL,
    WORKSHOP_SLOW,
    WORKSHOP_AREA,
    WORKSHOP_DOUBLE_BULLET,
    WORKSHOP_MULTI_TARGET,
    WORKSHOP_BOUNCE_BULLET,
    WORKSHOP_STUN,
    WORKSHOP_KNOCKBACK,
    WORKSHOP_DEADHIT,
    WORKSHOP_CORPRE_EXPLOSION,
    WORKSHOP_REFLECT,
    WORKSHOP_SHIELD,
    WORKSHOP_LIFE_STEAL,
    WORKSHOP_IMPLUSE_WAVE,
    WORKSHOP_GOLDBONUS_WAVE,
    WORKSHOP_GOLDBONUS_KILL,
    WORKSHOP_GOLD_DISCOUNT,
    WORKSHOP_SATELLITES,
    WORKSHOP_GROUND_MINES,
    WORKSHOP_TOMAHAWK_AMPLIFY,
    WORKSHOP_HIGHLIGHT,
    WORKSHOP_SHOCKWAVE_HEALTH,

    WORKSHOP_ATTACK_DISCOUNT,
    WORKSHOP_DEFENSE_DISCOUNT,

    WORKSHOP_VOID_NEXUS_DAMAGE,
    WORKSHOP_THUNDER_STUN_CHANGE,
    WORKSHOP_GOLDEN_SANCTUARY_BONUS,
    WORKSHOP_FORCE_REDUCTION,
    WORKSHOP_BUYX,
    WORKSHOP_GAME_SPEED,
    WORKSHOP_CARD_PRESET,
    WORKSHOP_TECHNOLOGY_DISCOUNT,
    WORKSHOP_TECHNOLOGY_SPEED,
    WORKSHOP_DAMAGE_RANGE,
    WORKSHOP_THORNY_ARMOR,
    WORKSHOP_DAMAGE_REDUCE,
    WORKSHOP_DAMAGE_RESISTANCE,

    WORKSHOP_INTEREST,
    WORKSHOP_MAX_INTEREST,

    WORKSHOP_DOUBLE_KILL_BEAM,
    WORKSHOP_LIFEBOX_AFTER_BOSS,
    WORKSHOP_LIFEBOX_HP_AMOUNT,
    WORKSHOP_LIFEBOX_MAX_HP,
    WORKSHOP_LIFEBOX_CHANGE,
    WORKSHOP_LIFE_BOX,
    WORKSHOP_COIN_INTEREST,
    WORKSHOP_RANGE_ADD,
    WORKSHOP_SHOCKWAVE_GOLD_BONUS,
    WORKSHOP_BOMB_DAMAGE,
    WORKSHOP_SATALLITE_DAMAGE,
    WORKSHOP_GOLDEN_SANCTUARY_DURATION,
    WORKSHOP_VOID_NEXUS_GOLD_BONUS,
    WORKSHOP_TOMAHAWK_EXPLOTION_RADIUS,
    WORKSHOP_THUNDER_STOCK_STUN,
    WORKSHOP_EXTRA_VOID_NEXUS,
    WORKSHOP_THUNDER_STUN_MULTIPLIER,
    WORKSHOP_ANTI_FORCE_DURATION,
    WORKSHOP_ANTI_FORCE_RANGE,
    WORKSHOP_ANTI_FORCE_REDUCTION,

    WORKSHOP_FIRE_AIRCRAFT,
    WORKSHOP_GOLDEN_AIRCRAFT,
    WORKSHOP_SUPPORT_AIRCRAFT,
    WORKSHOP_THUNDER_AIRCRAFT,
}

public enum UpgraderCategory
{
    WORKSHOP_BASE,
    WORKSHOP_CRETICAL,
    WORKSHOP_SLOW,
    WORKSHOP_AREA,
    WORKSHOP_DOUBLE_BULLET,
    WORKSHOP_MULTI_TARGET,
    WORKSHOP_BOUNCE_BULLET,
    WORKSHOP_STUN,
    WORKSHOP_KNOCKBACK,
    WORKSHOP_DEADHIT,
    WORKSHOP_CORPRE_EXPLOSION,
    WORKSHOP_REFLECT,
    WORKSHOP_SHIELD,
    WORKSHOP_LIFE_STEAL,
    WORKSHOP_IMPLUSE_WAVE,
    WORKSHOP_GOLDBONUS_WAVE,
    WORKSHOP_GOLDBONUS_KILL,
    WORKSHOP_GOLD_DISCOUNT,
    WORKSHOP_SATELLITES,
    WORKSHOP_GROUND_MINES,
    WORKSHOP_DAMAGE_RANGE,
    WORKSHOP_REDUCE,
    WORKSHOP_DAMAGE_RETURN_POWER,
    WORKSHOP_DODGE,
    WORKSHOP_COIN_PER,
    WORKSHOP_GOLD_PER,
    WORKSHOP_GOLDBONUS_KILL_BOSS,
    WORKSHOP_FREE_UPGRADE,
    WORKSHOP_COIN_INTEREST,
    WORKSHOP_KILL_BEAM,
    WORKSHOP_LIFE_BOX,
}

public enum TypeMilestone
{
    POWER_STONE,
    GOLD,
    GEM,

    DAILY_GIFT,
    ACHIEVEMENT,
    LABS,
    CARDS,
    FLIP_IMAGE,
    LUCKY_DRAW,
    RANKING,
    WORLD_2,
    AVATAR_QUANTUM_CANNON,
    WORLD_3,
    AVATAR_PHOTON_BLASTER,
    WORLD_4,
    AVATAR_CYBERNOVA,
    AREA,
    UNLOCK_UPGRADER_INFOR,
    UNLOCK_LAB_INFOR,
    UNLOCK_ULTIMATE_WEAPON,
    WORLD_5,
    WORLD_6,
    WORLD_7,
    WORLD_8,
    WORLD_9,
    WORLD_10,
    WORLD_11,
    WORLD_12,
}

public enum UltimateWeaponCategory
{
    
}

public enum UpgraderID
{
    //ATTACK
    attack_damage,
    attack_range,
    attack_speed,
    critical_shot_chance,
    critical_shot_damage,
    slow_chance,
    slow_power,
    range_damage_bonus,
    double_shot_chance,
    double_shot_damage_percent,
    multi_shot_chance,
    multi_shot_target_number,
    bounce_shot_chance,
    bounce_shot_damage,
    bounce_shot_target_number,
    stun_chance,
    stun_time,
    knockback_chance,
    dead_hit_chance,
    corpse_explosion_percent,
    corpse_explosion_damage_percent,
    //DEFENSE
    health,
    generation,
    damage_resistance,
    dodge_chance,
    damage_return_chance,
    damage_return_power,
    shield_hp,
    shield_spawn_time,
    life_steal,
    //RESOURCE
    gold_bonus_each_wave,
    gold_bonus_each_kill100,
    gold_bonus_after_boss_killed,
    discount_of_coin_price,
    //SATELLITE
    satellite_number,
    satellite_damage,
    satellite_stun_time,
    //BOMB
    bomb_number,
    bomb_damage,
    bomb_area,

    impulse_wave_size,
    impulse_wave_frequency,
    damage_range,
    bounce_shot_range,
    knockback_force,
    slow_time,
    corpse_explosion_range,
    damage_reduce,
    coin_per_kill,
    coin_per_wave,
    gold_per_kill,
    free_attack_upgrade,
    free_deffense_upgrade,
    free_resource_upgrade,
    coin_interest,

    kill_beam_cooldown,
    kill_beam_damage,
    kill_beam_duration,
    lifebox_hp_amount,
    lifebox_max_hp,
    lifebox_change
}

public enum Format
{
    NUMBER,
    PERCENT,
    SECOND,
}

[System.Flags]
public enum BuyingType
{
    GOLD = 1 << 0,
    BY_LEVEL_UPGRADE = 1 << 1,
    REWARD = 1 << 2,
    NoBuy = 1 << 3,
}

public enum TypeAvatar
{
    AVATAR_NORMAL = 0,
    AVATAR_QUANTUM_CANNON = 1,
    AVATAR_PHOTON_BLASTER = 2,
    AVATAR_CYBERNOVA = 3,
    AVATAR_PLASMA_BARRAGER = 4,
}

public enum TypeBannerPack
{
    StonePack = 0,
    BeginnerPack = 1,
}

public enum TypeRarityTech
{
    Common,
    UnCommon,
    UnCommonPlus,
    Rare,
    RarePlus,
    Legendary,
    LegendaryPlus,
    Artifact,
    ArtifactPlus,
    Heirloom,
}
public enum TypeClassTechSystem
{
    CompositeArmor = 0,
    HybridPower = 1,
    Engine = 2,
    Cryogenic = 3
}

public enum TypeTech
{
    // CompositeArmor
    AegisFortitude,
    TitaniumBastion,
    EarthforgedBulwark,
    PhoenixWard,

    // HybridPower
    OblivionsEdge,
    Stormbreaker,
    FangOfTheVoid,
    BlazetongueReape,

    // Engine
    ProsperitysGrasp,
    VortexExcavator,
    AlchemistsSatchel,
    MidasTouch,

    // Cryogenic
    ArcaneCodex,
    CelestialScepter,
    GrimoireAbyss,
    AstralConduit
}


public enum SubStatsTechSystem
{
    // CompositeArmor
    HpRegen,
    DamageResistancePercent,
    DamageReduce,
    DamageReturnPower,
    Lifesteal,
    ImpulseWaveSize,
    ImpulseWaveFrequency,
    DodgeChance,
    ShieldHp,
    ShieldTime,
    SatelliteDamage,
    BombDamage,
    BombArea,

    // HybridPower
    AttackSpeed,
    CriticalChance,
    CriticalFactor,
    AttackRange,
    DamagePerMeter,
    MultiShotChance,
    MultiShotTargets,
    RapidFireChance,
    BounceShotChance,
    BounceShotTargets,
    BounceShotRange,
    KnockbackChance,
    KnockbackForce,
    StunChance,
    CorpseExplosionDamage,

    // Engine
    CoinPerKill,
    CoinPerWave,
    GoldPerKill,
    GoldPerWave,
    FreeAttackUpgrade,
    FreeDefenseUpgrade,
    FreeResourceUpgrade,
    CoinInterest,
    KillBeamDamage,
    LifeboxHpAmount,
    LifeboxMaxHp,
    LifeboxChance,

    // Cryogenic
    GoldenSanctuaryBonus,
    GoldenSanctuaryDuration,
    GoldenSanctuaryCooldown,

    VoidNexusSize,
    VoidNexusDuration,
    VoidNexusCooldown,

    HighlightBonus,
    HighlightAngle,

    AntiForceDuration,
    AntiForceSpeedReduction,
    AntiForceCooldown,

    ShockwaveDamage,
    ShockwaveQuantity,
    ShockwaveCooldown,

    TomahawkDamage,
    TomahawkQuantity,
    TomahawkCooldown,

    ThunderboltDamage,
    ThunderboltQuantity,
    ThunderboltChance
}

public enum TypeFormat
{
    Percent,
    Flat,
    Seconds,
    Multiplier,
}

public enum TypeRarityRelic
{
    Rare,
    Epic
}

public enum TypeRelic
{
    DemonsPact,
    PhoenixFeather,
    ChronoCore,
    HoneyDrop,
    BloodfangAmulet,
    DragonsBreath,
    CelestialBlade,
    VoidFang,
    StoneGuardian,
    ShadowCloak,
    SanctuaryOrb,
    AlchemistsStone,
    MerchantsFortune,
    TreasureCompass,
    GoldenTouch,
    KingsTribute,
    StormcallerTotem,
    SoulHarvester,
    EclipseEye,
    ChaosRune,
    FrostboundSigil,
    OrbOfOmniscience,
    NecromancersGrasp,
    TwilightCrystal,
    LunarCharm,
    DimensionalRift,
    WhisperingRelic,
    VeilOfTheUnknown,
    HourglassOfOblivion,
    EldritchSigil,
    None
}

public enum TypeSong
{
    Default = 0,
    DefaultMusic = 1,
    IntheEnd = 2,
    Movement = 3,
    Nothing = 4,
    None,
}

public enum TypeBot
{
    FireAircaft = 0,
    ThunderAircaft = 1,
    GoldenAircaft = 2,
    SupportAircaft = 3,
}

public enum TypeRank
{
    Recruit = 0,
    Private = 1,
    Sergeant = 2,
    Captain = 3,
    Major = 4,
    General = 5,
}

public enum TypeRankChallenge
{
    Rank1 = 0,
    Rank2 = 1,
    Rank3 = 2,
}

public enum CardID
{
    NONE = 0,
    //common
    COMMON_DAMAGE = 1,
    COMMON_HEALTH = 2,
    COMMON_HP_REGEN = 3,
    COMMON_SPAWN_FASTER = 4,
    COMMON_SPAWN_SLOWER = 5,

    //RARE
    RARE_BOMB_RANGE = 6,
    RARE_GOLD_RWD = 7,
    RARE_SATELLI_SMART = 8,
    RARE_SHIELD_EXPLODE = 9,
    RARE_SILVER_RWD = 10,

    //EPIC
    EPIC_ATK_SPD = 11,
    EPIC_CRITICAL_DAMAGE = 12,
    EPIC_FREEZE_AREA = 13,
    EPIC_REDUCE_PRICE = 14,

    //DIVINE
    DIVINE_BOMB_SLOW = 15,
    DIVINE_CORPSE_EXPLODE_RANGE = 16,
    DIVINE_CRITICAL_CHANCE = 17,
    DIVINE_SATELLI_POISON = 18,
    DIVINE_STACK_DMG = 19,
}

public enum TypeCard
{
    COMMON,
    RARE,
    EPIC,
    DIVINE,
}

public enum TypePack
{
    BEGINER,
    PREMIUM,
    BOOSTER,
    GOLDER,
    GEM,
    GOLD,
    EVENT_BOOST,
}

public enum TypeExpressMode
{
    REMOVE_ADS,
    SPEED_GAMEPLAY_X5,
}

public enum TypeXBonusCurrency
{
    x2,
    x3,
    x4,
}

public enum TypeDayElement
{
    DAY1,
    DAY2,
    DAY3,
    DAY4,
    DAY5,
    DAY6,
    DAY7,
    DAY8
}

public enum IdSubjectType
{
    STARTING_COIN = 0,
    UPGRADE_ATTACK_DISCOUNT = 1,
    UPGRADE_DEFENSE_DISCOUNT = 2,
    UPGRADE_GOLD_DISCOUNT = 3,
    LAB_DISCOUNT = 4,
    LAB_SPEED = 5,

    ATK_DAMAGE = 6,
    SPEED_ATTACK_ADDED = 7,
    CRITICAL_DAMAGE = 8,
    ATTACK_RANGE_ADD = 9,

    HEALTH = 10,
    HEALTH_REGEN = 11,
    BOMB_DAMAGE = 12,
    SATELLITE_DAMAGE = 13,

    GOLD_PER_KILL = 14,
    COIN_PER_WAVE = 15,
    COIN_PER_KILL = 16,
    GOLD_PER_WAVE = 17,

    IMPUSLE_WAVE_SIZE = 18,
    HIGHTLIGHT_GOLD_BONUS = 19,
    TOMAHAWK_AMPLIFY = 20,
    TOMAHAWK_EXPLOTION_RADIUS = 21,
    SHOCKWAVE_HEALTH = 22,
    SHOWCKWAVE_GOLD_BONUS = 23,

    VOID_NEXUS_DAMAGE = 24,
    THUNDER_STUN = 25,
    GOLDEN_SANCTUARY_BONUS = 26,
    ANTI_FORCE_REDUCTION = 27,
    VOID_NEXUS_GOLD_BONUS = 28,
    EXTRA_VOID_NEXUS = 29,
    GOLDEN_SANCTUARY_DURATION = 30,
    THUNDER_STUN_CHANCE = 31,
    THUNDER_STUN_MULTIPLIER = 32,
    FORCE_REDUCTION = 33,
    ANTI_FORCE_DURATION = 34,
    ANTI_FORCE_RANGE = 35,
    BUYX = 36,
    GAME_SPEED = 37,
    TECHNOLOGY_DISCOUNT = 38,
    TECHNOLOGY_SPEED = 39,
    CARD_PRESET = 40,
    DAMAGE_RANGE = 41,
    THORNY_ARMOR = 42,
    DAMAGE_REDUCE = 43,
    DAMAGE_RESISTANCE = 44,

    INTEREST = 45,
    MAX_INTEREST = 46,

    DOUBLE_KILL_BEAM = 47,
    LIFEBOX_AFTER_BOSS = 48,
    LIFEBOX_HP_AMOUNT = 49,
    LIFEBOX_MAX_HP = 50,
    LIFEBOX_CHANGE = 51,

    FIRE_AIRCRAFT_BURN_STACK = 52,
    FIRE_AIRCRAFT_COOLDOWN = 53,
    GOLDEN_AIRCRAFT_COOLDOWN = 54,
    GOLDEN_AIRCRAFT_DURATION = 55,
    THUNDER_AIRCRAFT_COOLDOWN = 56,
    THUNDER_AIRCRAFT_SHOCK_TIME = 57,
    SUPPORT_AIRCRAFT_COOLDOWN = 58,
    SUPPORT_AIRCRAFT_DURATION = 59,
}
public enum SubjectType
{
    MAIN_RESEARCH,
    ATTACK_RESEARCH,
    DEFENSE_RESEARCH,
    GOLD_RESEARCH,
    ULTIMATE_WEAPON,
    SUPPORT_RESEARCH,
}

public enum SubjectTypeSupport
{
    NONE,
    SUPPORT,
    BONUS,
}

public enum AchievementID
{
    DESTROY_MONSTER,
    DESTROY_BOSSES,
    PASS_WAVES,
    UPGRADE_FORTNESS,
    CARDS_COLLECT,
    LAB_RESEARCH,
    UNLOCK_WORLD,
    EVENT_PLAY_TIMES,
}

public enum UW_ID
{
    TOMAHAWK,
    HIGHLIGHT,
    SHOCKWAVE,
    VOID_NEXUS,
    THUNDER_BOLT,
    GOLDEN_SANCTUARY,
    ANTI_FORCE,
}

public enum DailyQuestSpecialType
{
    DestroyEnemiesHighlight,
    DestroySlowedEnemies,
    DestroyWithTomahawk,
    DestroyGoldenSanctuary,
    BounceShotEnemies,
    DestroyWithShockwaves,
    DestroyWithSatellites,
    DestroyWithBombs,
    KnockbackEnemies,
}

public enum DailyQuestType
{
    StartBattle,
    Upgrade,
    BuyCard,
    DestroyFastEnemies,
    SurviveWaves,
    DestroyBoss,
    WatchAds,
    ClaimFreeGems,
    DestroyShotEnemies,
    DestroyTankEnemies,
    DestroyNormalEnemies,
    CriticalHits,
    UpgradeInBattle,
    ChangeSkin,
    TotalWaves,
/*    ArenaMatches,*/
}

