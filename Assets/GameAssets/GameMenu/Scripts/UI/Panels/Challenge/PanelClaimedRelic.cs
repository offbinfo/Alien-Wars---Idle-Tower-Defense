using language;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelClaimedRelic : Singleton<PanelClaimedRelic>
{
    public Image icon;
    public GameObject bgRare;
    public GameObject bgEpic;
    [SerializeField]
    private TMP_Text txtName;

    public void SetUp(RelicData relicData, Sprite iconRelic)
    {
        gameObject.SetActive(true);
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
        SetImage(iconRelic);
        txtName.text = LanguageManager.GetText(relicData.typeRelic.ToString());
        StartCoroutine(InvokeUnscaledClose(2.5f));
    }

    private IEnumerator InvokeUnscaledClose(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        OnCLose();
    }

    public void OnCLose()
    {
        gameObject.SetActive(false);
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
            rt.sizeDelta = new Vector2(120, 120 / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(120 * aspectRatio, 120);
        }
    }
}
