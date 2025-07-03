using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InforBannerHome : GameMonoBehaviour
{
    [SerializeField]
    private TMP_Text txtHighWaveInWorld;
    [SerializeField]
    private TMP_Text txtBonusGold;

    private void Start()
    {
        InitData();
    }

    public void InitData()
    {
        int indexWave = GameDatas.GetHighestWaveInWorld(GameDatas.CurrentWorld);
        txtHighWaveInWorld.text = LanguageManager.GetText("higest_wave_x") +" " +(indexWave == 0 ? "..." : indexWave.ToString());
    }
}
