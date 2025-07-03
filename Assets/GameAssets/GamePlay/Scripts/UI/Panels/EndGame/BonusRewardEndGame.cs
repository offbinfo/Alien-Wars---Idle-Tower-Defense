using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusRewardEndGame : GameMonoBehaviour
{

    [SerializeField] TMP_Text txt_des;
    [SerializeField] TMP_Text txt_gold;
    long goldRwd;
    float multiply;
    bool isClaimed = false;
    public void Open(float x, long gold)
    {
        goldRwd = gold;
        multiply = x;
        gameObject.SetActive(true);
        txt_des.text = string.Format(LanguageManager.GetText("gold_bonus_des"), x);
        txt_gold.text = ((int)(gold * x)).ToString();
        isClaimed = false;
    }

    public void OnClickX1()
    {
        if (isClaimed)
            return;
        ClaimRwd(1);
    }

    public void OnClickYes()
    {
        if (isClaimed)
            return;
        WatchAds.WatchRewardedVideo(() =>
        {
            ClaimRwd(multiply);
        }, "EndGame_X2");
    }

    public void OnClickClose()
    {
        if (isClaimed)
            return;
        ClaimRwd(1);
    }

    private void ClaimRwd(float multiply)
    {
        isClaimed = true;
        float totalGoldEarn = (float)(goldRwd * multiply);
        GameDatas.Gold += totalGoldEarn/* - GPm.GoldInGame*/;
        GameAnalytics.LogEvent_EarnGold("play_game", (totalGoldEarn - GPm.GoldInGame));
        GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + totalGoldEarn);

        var highestGoldEarn = GameDatas.GetDataProfile(IDInfo.HighestBattleGoldEarned);
        if (totalGoldEarn > highestGoldEarn)
            GameDatas.SetDataProfile(IDInfo.HighestBattleGoldEarned, totalGoldEarn);

        if ((totalGoldEarn) == 0)
        {
            TimeGame.Pause = false;
            LoadSceneManager.instance.LoadScene(TypeScene.GameMenu);
        } else
        {
            ObjectUI_Fly_Manager.instance.Get(20, txt_gold.transform.position, CurrencyContainer.instance.trans_gold.position, CurrencyType.GOLD, () =>
            {
                TimeGame.Pause = false;
                LoadSceneManager.instance.LoadScene(TypeScene.GameMenu);
            });
        }
    }
}
