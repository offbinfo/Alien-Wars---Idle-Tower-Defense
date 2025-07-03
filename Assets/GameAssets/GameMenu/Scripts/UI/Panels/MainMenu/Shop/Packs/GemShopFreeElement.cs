using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using language;

public class GemShopFreeElement : MonoBehaviour
{
    [SerializeField] TMP_Text txt_time;
    [SerializeField] TMP_Text txtAmount;
    [SerializeField] Button btn_Claim;
    [SerializeField] Transform center;
    [SerializeField]
    private BasePack basePack;
    private GemPack gemPack;

    private void Awake()
    {
        gemPack = basePack as GemPack;
        txtAmount.text = gemPack.amount.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(IE_CountTimeClaimFree());
    }
    IEnumerator IE_CountTimeClaimFree()
    {
        btn_Claim.enabled = false;
        while (DateTime.Now < GameDatas.timeClaimFreeGem_Target)
        {
            var timespan = GameDatas.timeClaimFreeGem_Target - DateTime.Now;
            txt_time.text = timespan.Display();
            yield return new WaitForSecondsRealtime(1f);
        }
        txt_time.text = LanguageManager.GetText("free");
        btn_Claim.enabled = true;
    }
    public void OnClickBtnClaim()
    {
        QuestEventManager.FreeGemClaimed(1);
        EventChallengeListenerManager.FreeGemClaimed(1);

        GameDatas.Gem += gemPack.amount;
        GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + gemPack.amount);
        var amount = Mathf.Clamp(gemPack.amount / 10, 5, 20);
        var targetUI = TopUI_Currency.instance ? TopUI_Currency.instance.gemIcon.transform.position : CurrencyContainer.instance._trans_gem.position;
        ObjectUI_Fly_Manager.instance.Get(amount, center.position, targetUI, CurrencyType.GEM);
        GameDatas.timeClaimFreeGem_Target = DateTime.Now.AddHours(3);
        StartCoroutine(IE_CountTimeClaimFree());
    }
}

