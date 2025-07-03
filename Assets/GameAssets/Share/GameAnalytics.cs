using Firebase.Analytics;
using System;
using System.Diagnostics;

public class GameAnalytics
{
    public static void LogEventPlay(int world, int wave)
    {
        FirebaseAnalytics.LogEvent("play_world_" + world, "wave", wave.ToString());
    }

    public static void LogEventCLaimOfflineReward(bool isX2)
    {
        FirebaseAnalytics.LogEvent("offline_reward_", "bonusx2", isX2.ToString());
    }

    public static void LogTotalUserPlayArena(int total)
    {
        FirebaseAnalytics.LogEvent("arena_player_count", "total", total.ToString());
    }

    public static void LogTotalUserRePlayArena(int total)
    {
        FirebaseAnalytics.LogEvent("total_arena_replays", "total", total.ToString());
    }

    public static void LogTotalUpRankArena(int total)
    {
        FirebaseAnalytics.LogEvent("rank_up_attempts", "total" ,total.ToString());
    }

    public static void LogEventEndPlay(int world, int wave)
    {
        FirebaseAnalytics.LogEvent("play_world_" + world, "wave", wave.ToString());
    }

    public static void LogEventLongToUnlockNewWorld(TimeSpan elapsedTime, int world)
    {
        FirebaseAnalytics.LogEvent("time_unlock_new_world",
                new Parameter("time_elapsed_days", (float)elapsedTime.TotalDays),
                new Parameter("time_elapsed_hours", (float)elapsedTime.TotalHours),
                new Parameter("world", world));
    }

    public static void LogEventLongToUnlockUltimateWeapon(TimeSpan elapsedTime, string nameUW)
    {
        FirebaseAnalytics.LogEvent("time_unlock_ultimate_weapon",
            new Parameter("time_elapsed_days", (float)elapsedTime.TotalDays),
            new Parameter("time_elapsed_hours", (float)elapsedTime.TotalHours),
            new Parameter("nameUW", nameUW));
    }

    public static void LogEvent_PlayTutorial(int step)
    {
        FirebaseAnalytics.LogEvent("play_tutorial", "tutorial_step", "step_" + step);
    }
    public static void LogEvent_EarnGold(string source, float value)
    {
        FirebaseAnalytics.LogEvent("earn_gold", "source_value", string.Format("{0}_{1}", source, value));
    }
    public static void LogEvent_UpgraderByGold(UpgraderID id)
    {
        FirebaseAnalytics.LogEvent("upgrade_by_gold", "upgrade_id", id.ToString());
    }
    public static void LogEvent_UpgradeByCoin(UpgraderID id)
    {
        FirebaseAnalytics.LogEvent("upgrade_by_coin", "upgrade_id", id.ToString());
    }
    public static void LogEvent_TimePlay(int time)
    {
        FirebaseAnalytics.LogEvent("time_play", "time", time.ToString());
    }
    public static void LogEvent_LoseGameWorld_x(int world, int waveLose)
    {
        FirebaseAnalytics.LogEvent("lose_game_world_" + world, "wave", waveLose);
    }
    public static void LogEvent_rewardAds(string place)
    {
        Parameter parameter = new Parameter("place", place);
        FirebaseAnalytics.LogEvent("RewardAds",parameter);
    }
}
