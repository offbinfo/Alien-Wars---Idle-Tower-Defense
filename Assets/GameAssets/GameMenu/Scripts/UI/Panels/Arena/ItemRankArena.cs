using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRankArena : MonoBehaviour
{
    [SerializeField]
    private GameObject[] iconRanks;
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TMP_Text txtWave;
    [SerializeField]
    private TMP_Text txtIndexRank;
    [SerializeField]
    private Image iconAvatar;
    [SerializeField]
    private GameObject userBlur;
    [SerializeField]
    private RectTransform rect;

    private int sizeUpRank = 240;
    private int sizeDefault = 120;

    [SerializeField]
    private GameObject objUpRank;
    [SerializeField]
    private GameObject objDownRank;
    private int indexRank = -1;
    [SerializeField]
    private List<Sprite> iconAvatars;
    private int indexRankPlayer;
    private bool isRankPlayer;

    private int indexRankOld;
    private string nameOld;
    private int waveOld;

    private BotData data;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnChangedName, OnRefreshNamePlayer);
    }

    private void OnRefreshNamePlayer(object obj)
    {
        if (isRankPlayer)
        {
            txtName.text = GameDatas.user_name;
        }
    }

    public void SetData(BotData botData)
    {
        data = botData;

        this.indexRank = botData.indexRank;
        indexRankOld = this.indexRank;
        nameOld = botData.nameType.ToString();
        waveOld = botData.wave;

        Array.ForEach(iconRanks, icon => icon.SetActive(false));
        if (indexRank >= 1 && indexRank <= iconRanks.Length)
        {
            iconRanks[indexRank - 1].SetActive(true);
        }
        txtIndexRank.text = indexRank.ToString();

        txtName.text = botData.nameType.ToString();
        txtWave.text = botData.wave.ToString();

        int rand = UnityEngine.Random.Range(0, iconAvatars.Count);
        iconAvatar.sprite = iconAvatars[rand];

        CheckRankPlayer();
        CheckUpOrDownRank();
    }

    private void OnEnable()
    {
        if(data != null)
        {
            CheckRankPlayer();
        }
        CheckUpOrDownRank();
    }

    private void CheckRankPlayer()
    {
        TypeRank typeRank = (TypeRank)GameDatas.GetHighestRank();
        if (indexRank == GameDatas.GetIndexRank(typeRank))
        {
            isRankPlayer = true;
            indexRankPlayer = indexRank;
            userBlur.SetActive(true);
            txtName.text = GameDatas.user_name;
            txtWave.text = GameDatas.GetHighestWaveInRank(typeRank).ToString();
            iconAvatar.sprite = AvatarSource.instance.dataAvatars[GameDatas.id_avatar].sprite;
        }
        else
        {
            isRankPlayer = false;
/*            indexRankPlayer = indexRankOld;
            userBlur.SetActive(false);
            txtName.text = nameOld;
            txtWave.text = waveOld.ToString();*/
        }
    }

    private void CheckUpOrDownRank()
    {
        objUpRank.SetActive(false);
        objDownRank.SetActive(false);

        Vector2 sizeDelta = rect.sizeDelta;
        sizeDelta.y = sizeDefault;
        rect.sizeDelta = sizeDelta;

        if (indexRank == 27)
        {
            sizeDelta.y = sizeUpRank;
            objDownRank.SetActive(true);
            rect.sizeDelta = sizeDelta;
        }
        else if (indexRank == 5)
        {
            sizeDelta.y = sizeUpRank;
            objUpRank.SetActive(true);
            rect.sizeDelta = sizeDelta;
        }
        else
        {
            sizeDelta.y = sizeDefault;
            rect.sizeDelta = sizeDelta;
        }

        /*if (isRankPlayer)
        {
            objUpRank.SetActive(false);
            objDownRank.SetActive(false);

            int maxIndexRank = 30;
            int highestRankIndex = GameDatas.GetIndexRank((TypeRank)GameDatas.GetHighestRank());
            if (maxIndexRank > highestRankIndex)
            {
                objUpRank.SetActive(true);
                sizeDelta.y = sizeUpRank;
            }
            else
            {
                sizeDelta.y = sizeDefault;
            }

            rect.sizeDelta = sizeDelta;
        }*/
    }

}
