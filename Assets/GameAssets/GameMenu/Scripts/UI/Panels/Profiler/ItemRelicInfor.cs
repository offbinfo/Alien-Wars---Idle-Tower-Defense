using language;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRelicInfor : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtNameRelic;
    [SerializeField]
    private TypeRelic typeRelic;

    [SerializeField]
    private GameObject bgRare;
    [SerializeField]
    private GameObject bgEpic;
    [SerializeField]
    private Image icon;

    public void SetUp(RelicData cellData)
    {
        this.typeRelic = cellData.typeRelic;
        txtNameRelic.text = LanguageManager.GetText(typeRelic.ToString());

        if (cellData.typeRarityRelic == TypeRarityRelic.Rare)
        {
            bgRare.gameObject.SetActive(true);
            bgEpic.gameObject.SetActive(false);
        }
        else
        {
            bgRare.gameObject.SetActive(false);
            bgEpic.gameObject.SetActive(true);
        }

        SetImage(cellData.icon);
    }

    void SetImage(Sprite sprite)
    {
        if (sprite == null || icon == null) return;

        icon.sprite = sprite;
        float originalWidth = sprite.texture.width;
        float originalHeight = sprite.texture.height;

        float aspectRatio = originalWidth / originalHeight;

        RectTransform rt = icon.GetComponent<RectTransform>();

        if (aspectRatio > 1)
        {
            rt.sizeDelta = new Vector2(80, 80 / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(80 * aspectRatio, 80);
        }
    }
}
