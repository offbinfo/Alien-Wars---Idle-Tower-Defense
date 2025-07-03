using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEventGame : SingletonGame<DoubleEventGame>
{

    private DateTime gameStartTime;
    private DateTime doubleGoldStartTime;
    private DateTime doubleGoldEndTime;
    private DateTime doubleAdGemStartTime;
    private DateTime doubleAdGemEndTime;

    private const int doubleGoldEventDelay = 24; 
    private const int doubleAdGemEventDelay = 20; 

    private bool isDoubleGoldActive = false;
    private bool isDoubleAdGemActive = false;

    /*private void Start()
    {
        gameStartTime = DateTime.Now;

        doubleGoldStartTime = gameStartTime.AddHours(doubleGoldEventDelay);
        doubleGoldEndTime = doubleGoldStartTime.AddHours(24);

        doubleAdGemStartTime = doubleGoldStartTime.AddHours(doubleAdGemEventDelay);
        doubleAdGemEndTime = doubleAdGemStartTime.AddHours(12); 

        StartCoroutine(CheckEvents());
    }

    private IEnumerator CheckEvents()
    {
        while (true)
        {
            if (DateTime.Now >= doubleGoldStartTime && DateTime.Now <= doubleGoldEndTime)
            {
                if (!isDoubleGoldActive)
                {
                    isDoubleGoldActive = true;
                    DebugCustom.Log("Double Gold Event Started!");
                    //StartCoroutine(DoubleGoldEvent());
                    if (GameDatas.ActiveDoubleEventGold) yield break;
                    GameDatas.DoubleEventGold(true);
                }
            }

            if (DateTime.Now >= doubleAdGemStartTime && DateTime.Now <= doubleAdGemEndTime)
            {
                if (!isDoubleAdGemActive)
                {
                    isDoubleAdGemActive = true;
                    DebugCustom.Log("Double Ad-Gem Event Started!");
                    //StartCoroutine(DoubleAdGemEvent());
                    if (GameDatas.ActiveDoubleEventGem) yield break;
                    GameDatas.DoubleEventGem(true);
                }
            }

            if (DateTime.Now > doubleGoldEndTime)
            {
                if (isDoubleGoldActive)
                {
                    isDoubleGoldActive = false;
                    DebugCustom.Log("Double Gold Event Ended!");
                    GameDatas.DoubleEventGold(false);
                    GameDatas.ActiveDoubleEventGold = true;
                }
            }

            if (DateTime.Now > doubleAdGemEndTime)
            {
                if (isDoubleAdGemActive)
                {
                    isDoubleAdGemActive = false;
                    DebugCustom.Log("Double Ad-Gem Event Ended!");
                    GameDatas.DoubleEventGem(false);
                    GameDatas.ActiveDoubleEventGem = true;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }*/

}
