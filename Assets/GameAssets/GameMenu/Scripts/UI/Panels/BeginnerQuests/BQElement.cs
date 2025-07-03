using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using language;

public class BQElement : MonoBehaviour
{
    public BeginnerQuestID id => data.id;
    public float currentProgress => data.currentProgress;
    public float maxTarget => data.maxTarget;
    public BeginnerQuest_SO d => data;

    [SerializeField] TMP_Text txt_Describe;
    [SerializeField] TMP_Text txt_Reward;
    [SerializeField] TMP_Text txt_GemRwd;
    [SerializeField] TMP_Text txt_PowerStoneRwd;
    [SerializeField] TMP_Text txt_progress;
    [SerializeField] Image img_mainProgress;
    [SerializeField] GameObject obj_GemRwd;
    [SerializeField] GameObject obj_PowerStoneRwd;
    [SerializeField] GameObject obj_Gold;
    [SerializeField] GameObject obj_frame;
    [SerializeField] GameObject obj_Claim;
    [SerializeField] GameObject obj_UnClaim;
    [SerializeField] GameObject obj_Claimed;
    private BeginnerQuest_SO data;

    public bool isClaim
    {
        get
        {
            return GameDatas.IsClaimBeginnerQuest(id);
        }
        set
        {
            GameDatas.ClaimBeginnerQuest(id, value);
        }
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnBeginnerQuestProgressChanged, (o) =>
        {
            if(data.id == (BeginnerQuestID)o)
                SetUp(data);
        });
        EventDispatcher.AddEvent(EventID.OnBeginnerQuestClaim, (o) =>
        {
            if(data.id == (BeginnerQuestID)o)
                SetUp(data);
        });
    }

    public void SetUp(BeginnerQuest_SO data)
    {
        
        this.data = data;
        txt_Describe.text = data.describe;
        txt_Reward.text = data.rwdGold.ToString();
        obj_Gold.SetActive(data.rwdGold > 0);

        txt_GemRwd.text = data.rwdGem.ToString();
        obj_GemRwd.SetActive(data.rwdGem > 0);

        txt_PowerStoneRwd.text = data.Powerstone.ToString();
        obj_PowerStoneRwd.SetActive(data.Powerstone > 0);

        txt_progress.text = data.isClaim || data.isDone ? "Done" : string.Format("{0}/{1}",data.currentProgress,data.maxTarget);

        CheckClaimed();

        img_mainProgress.fillAmount = 1f / data.maxTarget * data.currentProgress;

        if(data.isClaim) transform.SetAsLastSibling();
    }

    private void CheckClaimed()
    {
        obj_Claim.SetActive(data.isDone && !data.isClaim);
        obj_UnClaim.SetActive(!data.isDone || !data.isClaim);
        obj_Claimed.SetActive(data.isClaim);
        obj_frame.SetActive(data.isDone || data.isClaim);
    }

    public void OnClickBtnClaim()
    {
        if (isClaim) return;

        data.ClaimReward();

        float amountGold = data.rwdGold;
        float amountGem = data.rwdGem;
        float amountPowerStone = data.Powerstone;

        if (amountGold > 0)
        {
            GameDatas.Gold += amountGold;
            GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + amountGold);

            ObjectUI_Fly_Manager.instance.Get(20, transform.position, TopUI_Currency.instance.goldIcon.transform.position, CurrencyType.GOLD);
        }


        if (amountGem > 0)
        {
            GameDatas.Gem += amountGem;
            GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amountGem);
            ObjectUI_Fly_Manager.instance.Get(20, transform.position, TopUI_Currency.instance.gemIcon.transform.position, CurrencyType.GEM);
        }

        if (amountPowerStone > 0)
        {
            GameDatas.PowerStone += amountPowerStone;
            if (amountPowerStone != 0) ObjectUI_Fly_Manager.instance.Get(20, transform.position,
                TopUI_Currency.instance.powerStoneIcon.transform.position, CurrencyType.POWER_STONE);
        }

        EventDispatcher.PostEvent(EventID.OnBeginnerQuestClaim, 0);
        SetUp(data);
    }
}
