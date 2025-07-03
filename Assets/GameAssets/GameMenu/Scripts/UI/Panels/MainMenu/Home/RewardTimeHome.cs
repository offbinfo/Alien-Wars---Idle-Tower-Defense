using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardTimeHome : Singleton<RewardTimeHome>
{

    public TMP_Text txt_time;
    public TMP_Text txt_gold;
    public TMP_Text txt_gem;
    public TMP_Text txt_powerStone;
    public Slider slider_process;
    public Vector3 formula_bonus_gold;
    private Coroutine c_CountDown;
    Dictionary<int, int> waveBonusInfo = new Dictionary<int, int>
    {
        {10, 50}, {30, 100}, {40, 150}, {70, 200},
        {90, 250}, {120, 300}, {140, 350}
    };

    private void OnEnable()
    {
        c_CountDown = StartCoroutine(I_CountDownGetReward());
    }
    private void OnDisable()
    {
        if (c_CountDown != null)
        {
            StopCoroutine(c_CountDown);
        }
    }
    IEnumerator I_CountDownGetReward()
    {
        yield return new WaitForEndOfFrame();
        DateTime dtSave = GameDatas.LoadObject<DateTime>("TimeSave");
        if (dtSave != DateTime.MinValue)
        {
            //!default
            TimeSpan ts = DateTime.Now - dtSave;
            GameDatas.SecondsAccumulate += (int)ts.TotalSeconds;
            if (GameDatas.SecondsAccumulate > GameDatas.secondToGetReward) GameDatas.SecondsAccumulate = GameDatas.secondToGetReward;
        }

        if (GameDatas.SecondsAccumulate < 0)
        {
            GameDatas.SecondsAccumulate = 1;
        }

        var timeLeft = GameDatas.secondToGetReward - GameDatas.SecondsAccumulate;
        txt_time.text = LanguageManager.GetText("full_in") + " " + Extensions.SecondsToTime(timeLeft);
        txt_gold.text = GetRewardGold(GameDatas.SecondsAccumulate).ToString();
        txt_gem.text = GetRewardGem(GameDatas.SecondsAccumulate).ToString();
        txt_powerStone.text = GetRewardPowerStone(GameDatas.SecondsAccumulate).ToString();
        slider_process.maxValue = 60 * 60 * 4;
        slider_process.value = GameDatas.SecondsAccumulate;

        while (GameDatas.SecondsAccumulate < GameDatas.secondToGetReward)
        {
            yield return new WaitForSeconds(1f);
            GameDatas.SecondsAccumulate += 1;
            GameDatas.SaveObject("TimeSave", DateTime.Now);
            timeLeft = GameDatas.secondToGetReward - GameDatas.SecondsAccumulate;
            txt_time.text = LanguageManager.GetText("full_in") + " " + Extensions.SecondsToTime(timeLeft);
            txt_gold.text = GetRewardGold(GameDatas.SecondsAccumulate).ToString();
            txt_gem.text = GetRewardGem(GameDatas.SecondsAccumulate).ToString();
            txt_powerStone.text = GetRewardPowerStone(GameDatas.SecondsAccumulate).ToString();
            slider_process.value = GameDatas.SecondsAccumulate;
        }
        //tích lũy full time
        txt_time.text = LanguageManager.GetText("claim");

    }

    public int GetRewardGold(int seconds)
    {
        if (!GameDatas.IsFirstClaimOfflineReward)
        {
            var userPower = GameDatas.userPower;
            var basePower = GameDatas.basePower;
            var baseReward = 400;
            var sumReward4h = baseReward * (userPower / basePower);
            var goldPerSecond = sumReward4h / (4 * 60 * 60);
            return (int)(goldPerSecond * seconds);
        } else
        {

            int world = GameDatas.GetHighestWorld();
            float worldMultiplier = 1.0f + 0.2f * (GameDatas.GetHighestWorld() + 1);
            int currentWave = GameDatas.GetHighestWaveInWorld(world) + 1;
            int baseGold = 600;
            float idleEfficiency = 0.35f;

            float totalIdleGold = 0f;

            foreach (var kvp in waveBonusInfo)
            {
                int wave = kvp.Key;
                int bonus = kvp.Value;

                if (currentWave >= wave)
                {
                    float idleGoldPerWorld = (baseGold + wave + bonus) * worldMultiplier;
                    totalIdleGold += idleGoldPerWorld;
                }
            }

            if (totalIdleGold == 0f)
            {
                totalIdleGold = baseGold * worldMultiplier;
            }

            totalIdleGold *= idleEfficiency;

            float goldPerSecond = totalIdleGold / (4 * 60 * 60);

            return (int)(goldPerSecond * seconds);
        }
    }

    public int GetRewardGem(int secondsAccumulate)
    {
        var sum = 0;
        if (secondsAccumulate < 30 * 60)
            sum = 0;
        else if (secondsAccumulate < 60 * 60)
            sum = 1;
        else if (secondsAccumulate < 90 * 60)
            sum = 2;
        else if (secondsAccumulate < 120 * 60)
            sum = 2;
        else if (secondsAccumulate < 180 * 60)
            sum = 2;
        else if (secondsAccumulate < 240 * 60)
            sum = 3;
        else
            sum = 3;
        return sum;
    }
    public int GetRewardPowerStone(int seconds)
    {
        if (seconds <= 0)
            return 0;
        var sum = seconds / 3600;
        return sum;
    }
    #region BUTTON
    public void OnClickBtnClaim()
    {
        if(GameDatas.IsFirstClaimOfflineReward)
        {
            Gui.OpenBoard(UiPanelType.PopupClaimReward);
        }
        else
        {
            OnClaimReward();
        }
    }

    public void OnClaimReward(bool bonusX2 = false)
    {
        if (GameDatas.SecondsAccumulate <= 0) return;

        float amountGold = GetRewardGold(GameDatas.SecondsAccumulate) * (bonusX2 == true ? 2 : 1);
        float amountGem = GetRewardGem(GameDatas.SecondsAccumulate) * (bonusX2 == true ? 2 : 1);
        float amountPowerStone = GetRewardPowerStone(GameDatas.SecondsAccumulate) * (bonusX2 == true ? 2 : 1);

        var rect = txt_gold.GetComponent<RectTransform>();

        if (amountGold > 0)
        {
            GameAnalytics.LogEvent_EarnGold("reward_time", amountGold);
            GameDatas.Gold += amountGold;
            GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + amountGold);

            ObjectUI_Fly_Manager.instance.Get(20, rect.transform.position, TopUI_Currency.instance.goldIcon.transform.position, CurrencyType.GOLD);
        }


        if (amountGem > 0)
        {
            QuestEventManager.FreeGemClaimed(1);
            GameDatas.Gem += amountGem;
            GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amountGem);
            ObjectUI_Fly_Manager.instance.Get(20, txt_gem.transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.GEM);
        }

        if (amountPowerStone > 0)
        {
            GameDatas.PowerStone += amountPowerStone;
            if (amountPowerStone != 0) ObjectUI_Fly_Manager.instance.Get(20, txt_powerStone.transform.position,
                TopUI_Currency.instance.powerStoneIcon.transform.position, CurrencyType.POWER_STONE);
        }

        ResetReward();
        GameAnalytics.LogEventCLaimOfflineReward(bonusX2);
    }

    public void ResetReward()
    {
        //reset
        GameDatas.SecondsAccumulate = 0;
        GameDatas.SaveObject("TimeSave", DateTime.Now);

        if (c_CountDown != null) StopCoroutine(c_CountDown);
        c_CountDown = StartCoroutine(I_CountDownGetReward());
        GameDatas.IsFirstClaimOfflineReward = true;
    }
    #endregion


}
