using language;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicRewardElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtName;
    private TypeRelic typeRelic;
    private TypeRarityRelic typeRarityRelic;
    public bool isClaim;
    [SerializeField]
    private GameObject panelRareClaimed;
    [SerializeField]
    private GameObject panelEpicClaimed;

    [SerializeField]
    private GameObject bgRare;
    [SerializeField]
    private GameObject bgEpic;
    [SerializeField]
    private Image icon;
    private RelicData cellData;
    private Sprite iconRelic;
    [SerializeField]
    private TMP_Text txtDesc;
    [SerializeField]
    private float amount;

    public void Setup(RelicData relicData, Sprite iconRelic)
    {
        this.iconRelic = iconRelic;
        this.cellData = relicData;  
        this.typeRelic = relicData.typeRelic;
        this.typeRarityRelic = relicData.typeRarityRelic;
        txtName.text = LanguageManager.GetText(relicData.typeRelic.ToString());
        CheckClaimedRelic();

        SetImage(iconRelic);
        if (typeRarityRelic == TypeRarityRelic.Rare)
        {
            bgRare.gameObject.SetActive(true);
            bgEpic.gameObject.SetActive(false);
        }
        else
        {
            bgRare.gameObject.SetActive(false);
            bgEpic.gameObject.SetActive(true);
        }
        txtDesc.text = LanguageManager.GetText("receive_when_enough") + " " + amount.ToString();
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

    public void ClaimRelic()
    {
        if(isClaim)
        {
            if(GameDatas.IsRelicUnlock(typeRelic)) return;
            PanelClaimedRelic.instance.SetUp(cellData, iconRelic);
            GameDatas.RelicUnlock(typeRelic);
            CheckClaimedRelic();
        }
    }

    public void CheckClaimedRelic()
    {
        if (typeRarityRelic == TypeRarityRelic.Rare)
        {
            panelRareClaimed.SetActive(GameDatas.IsRelicUnlock(typeRelic));
            panelEpicClaimed.SetActive(false);
        }
        else
        {
            panelEpicClaimed.SetActive(GameDatas.IsRelicUnlock(typeRelic));
            panelRareClaimed.SetActive(false);
        }
    }
}
