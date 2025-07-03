using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevivalUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtCountRevive;

    private void Start()
    {
        UpdateUI(null);
        EventDispatcher.AddEvent(EventID.OnChangeAmountRevive, UpdateUI);
    }
    private void UpdateUI(object obj)
    {
        if (GameDatas.ReviveTowerCount <= 0)
            StartCoroutine(I_ShowRevivalTime());
        else
            txtCountRevive.text = GameDatas.ReviveTowerCount + "/5 " + LanguageManager.GetText("revive");
    }
    IEnumerator I_ShowRevivalTime()
    {
        while (DateTime.Now < GameDatas.timeTargetFullRevive)
        {
            txtCountRevive.text = "0/5 " + LanguageManager.GetText("revive") + " " + (GameDatas.timeTargetFullRevive - DateTime.Now).Display();
            yield return new WaitForSeconds(1f);

        }
        txtCountRevive.text = "5/5 " + LanguageManager.GetText("revive");
    }
}
