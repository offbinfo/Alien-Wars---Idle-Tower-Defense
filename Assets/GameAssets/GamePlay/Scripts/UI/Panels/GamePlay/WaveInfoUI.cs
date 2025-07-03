using DG.Tweening;
using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : GameMonoBehaviour
{

    [SerializeField] private Slider sliderWave;
    [SerializeField] private TMP_Text txt_wave;

    private void Start()
    {
        OnRefreshTextWave(null);
        EventDispatcher.AddEvent(EventID.OnRefreshUIWave, OnRefreshUIWave);
        EventDispatcher.AddEvent(EventID.OnRefreshTextWaveUI, OnRefreshTextWave);
        EventDispatcher.AddEvent(EventID.OnEnemyKilled, OnValueChangeSliderWave);
    }

    private void OnRefreshTextWave(object obj)
    {
        string newWaveText = LanguageManager.GetText("wave")+" " +GPm.wavePlaying;
        if (txt_wave.text != newWaveText)
            txt_wave.text = newWaveText;
    }

    private void OnRefreshUIWave(object countEnemies)
    {
        int enemyCount = (int)countEnemies;
        sliderWave.maxValue = enemyCount;

        sliderWave.value = enemyCount;
        OnRefreshTextWave(null);
    }

    private void OnValueChangeSliderWave(object o)
    {
        sliderWave.value -= 1;
    }
}
