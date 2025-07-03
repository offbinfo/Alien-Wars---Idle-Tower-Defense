using AYellowpaper.SerializedCollections;
using language;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMilestoneElement : GameMonoBehaviour
{

    [SerializeField]
    private MilestoneCellView milestoneStandard;
    [SerializeField]
    private MilestoneCellView milestonePremium;
    [SerializeField]
    private int wave;
    [SerializeField]
    private GameObject contentWave;
    [SerializeField]
    private GameObject contentWaveDesc;
    [SerializeField]
    private Slider progressMileStone;
    [SerializeField]
    private TMP_Text txtWaveItem;
    [SerializeField]
    private TMP_Text txtWavePlaying;
    private WorldType typeWorld;
    private List<int> waveLevels = new();

    private void OnEnable()
    {
        SetWavePlaying();
    }

    public void SetData(RwdAll rwdAll, TabTierCellView tabTierCell)
    {
        wave = rwdAll.wave;
        this.typeWorld = tabTierCell.typeWorld;
        txtWaveItem.text = wave.ToString();
        CheckWaveFirstInMileStones();
        milestoneStandard.SetData(rwdAll.standard, GetImageDictMileStone(tabTierCell.dictImageMileStones
            , rwdAll.standard.typeMilestone), wave, tabTierCell.typeWorld);
        milestonePremium.SetData(rwdAll.premium, GetImageDictMileStone(tabTierCell.dictImageMileStones
            , rwdAll.premium.typeMilestone), wave, tabTierCell.typeWorld);
        tabTierCell.milestoneCells.Add(milestoneStandard);
        tabTierCell.milestoneCells.Add(milestonePremium);
        waveLevels = tabTierCell.PointWaveMileStones;
    }

    public int index;
    [Button("Test")]
    public void Test()
    {
        GameDatas.SetHighestWaveInWorld(0, index);
        SetWavePlaying();
    }

    private void SetWavePlaying()
    {
        int wavePlaying = GameDatas.GetHighestWaveInWorld((int)this.typeWorld);
        txtWavePlaying.text = LanguageManager.GetText("wave") +" " + wavePlaying;

        SetProgressMileStones(wavePlaying);

/*        milestoneStandard.CheckUnLockItemReward(wavePlaying);
        milestonePremium.CheckUnLockItemReward(wavePlaying);*/
        OnRefresh();
    }

    public void OnRefresh()
    {
        int wavePlaying = GameDatas.GetHighestWaveInWorld((int)this.typeWorld);
        milestoneStandard.CheckUnLockItemReward(wavePlaying);
        milestonePremium.CheckUnLockItemReward(wavePlaying);
    }

    private void SetProgressMileStones(int wavePlaying)
    {
        if (!progressMileStone.gameObject.activeSelf) return;

        if (waveLevels.Count == 0) return;
        Dictionary<int, float> waveToSliderValue = new Dictionary<int, float>();
        for (int i = 0; i < waveLevels.Count; i++)
        {
            float normalizedValue = (float)i / (waveLevels.Count - 1);
            waveToSliderValue[waveLevels[i]] = normalizedValue;
        }

        int lowerWave = waveLevels[0], upperWave = waveLevels[waveLevels.Count - 1];
        foreach (var wave in waveLevels)
        {
            if (wave <= wavePlaying) lowerWave = wave;
            if (wave >= wavePlaying)
            {
                upperWave = wave;
                break;
            }
        }

        if (lowerWave == upperWave)
        {
            progressMileStone.value = waveToSliderValue[lowerWave];
            return;
        }

        float t = (float)(wavePlaying - lowerWave) / (upperWave - lowerWave);
        progressMileStone.value = Mathf.Lerp(waveToSliderValue[lowerWave], waveToSliderValue[upperWave], t);
    }

    private Sprite GetImageDictMileStone(SerializedDictionary<TypeMilestone, Sprite> dictImageMileStones
        , TypeMilestone typeMilestone)
    {
        foreach (var kvp in dictImageMileStones)
        {
            if (typeMilestone == TypeMilestone.UNLOCK_LAB_INFOR)
            {
                return dictImageMileStones[TypeMilestone.UNLOCK_LAB_INFOR];
            } 
            else if (typeMilestone == TypeMilestone.UNLOCK_UPGRADER_INFOR)
            {
                return dictImageMileStones[TypeMilestone.UNLOCK_UPGRADER_INFOR];
            }
            else
            {
                if (dictImageMileStones.ContainsKey(typeMilestone))
                {
                    return dictImageMileStones[typeMilestone];
                }
            }
        }
        return null;
    }

    private void CheckWaveFirstInMileStones()
    {
        bool isWaveFirst = (wave == 10);
        contentWave.SetActive(isWaveFirst);
        //contentWaveDesc.SetActive(!isWaveFirst);
        progressMileStone.gameObject.SetActive(isWaveFirst);
    }

}
