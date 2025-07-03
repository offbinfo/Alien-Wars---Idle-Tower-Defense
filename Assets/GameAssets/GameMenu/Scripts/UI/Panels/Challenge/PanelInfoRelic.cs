using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfoRelic : Singleton<PanelInfoRelic>
{
    [SerializeField]
    private GameObject bgRare;
    [SerializeField]
    private GameObject bgEpic;
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TMP_Text txtDesc;
    [SerializeField]
    private Image icon;

    public void SetUp(RelicData relicData)
    {
        gameObject.SetActive(true);
        SetImage(relicData.icon);
        if (relicData.typeRarityRelic == TypeRarityRelic.Rare)
        {
            bgRare.gameObject.SetActive(true);
            bgEpic.gameObject.SetActive(false);
        }
        else
        {
            bgRare.gameObject.SetActive(false);
            bgEpic.gameObject.SetActive(true);
        }

        txtName.text = LanguageManager.GetText(relicData.typeRelic.ToString());
        txtDesc.text = LanguageManager.GetText("desc_"+relicData.typeRelic.ToString());
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
            rt.sizeDelta = new Vector2(100, 100 / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(100 * aspectRatio, 100);
        }
    }
}
