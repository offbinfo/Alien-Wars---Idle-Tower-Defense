using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRelicSelect : MonoBehaviour
{
    private TypeRelic typeRelic = TypeRelic.None;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private GameObject panelSelected;
    [SerializeField]
    private GameObject bgRare;
    [SerializeField]
    private GameObject bgEpic;

    [SerializeField]
    private GameObject panelRareLock;
    [SerializeField]
    private GameObject panelEpicLock;
    private TypeRarityRelic typeRarityRelic;
    private RelicData cellData;

    public void SetUp(RelicData relicData)
    {
        this.cellData = relicData;
        typeRarityRelic = relicData.typeRarityRelic;
        typeRelic = relicData.typeRelic;

        SetImage(relicData.icon);
        if (typeRarityRelic == TypeRarityRelic.Rare)
        {
            bgRare.gameObject.SetActive(true);
            bgEpic.gameObject.SetActive(false);
        } else
        {
            bgRare.gameObject.SetActive(false);
            bgEpic.gameObject.SetActive(true);
        }

        CheckUnlockRelic();
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

    public void ShowInfor()
    {
        PanelInfoRelic.instance.SetUp(cellData);
    }

    private void OnEnable()
    {
        CheckUnlockRelic();
    }

    public void Selected()
    {
        if (!GameDatas.IsRelicUnlock(typeRelic)) return;
        if (GameDatas.IsMaxSlotRelic() || GameDatas.IsRelicEquiped(typeRelic))
        {
            panelSelected.SetActive(false);
            GameDatas.UnEquipedRelic(typeRelic);
        }
        else
        {
            panelSelected.SetActive(true);
            GameDatas.SetRelicEquiped(typeRelic);
        }
        CheckRelicEquip();
    }

    public void CheckRelicEquip()
    {
        panelSelected.SetActive(GameDatas.IsRelicEquiped(typeRelic));
    }

    private void CheckUnlockRelic()
    {
        if (typeRelic == TypeRelic.None) return;

        if (typeRarityRelic == TypeRarityRelic.Rare)
        {
            panelRareLock.SetActive(!GameDatas.IsRelicUnlock(typeRelic));
            panelEpicLock.SetActive(false);
        }
        else
        {
            panelEpicLock.SetActive(!GameDatas.IsRelicUnlock(typeRelic));
            panelRareLock.SetActive(false);
        }
        CheckRelicEquip();
    }
}
