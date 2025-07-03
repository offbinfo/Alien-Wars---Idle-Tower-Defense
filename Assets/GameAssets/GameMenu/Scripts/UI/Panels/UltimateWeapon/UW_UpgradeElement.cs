using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UW_UpgradeElement : BaseUICellView
{

    [SerializeField] TMP_Text txt_title;
    [SerializeField] TMP_Text txt_description;
    [SerializeField] Image icon;

    [SerializeField] GameObject objProperties;
    [SerializeField] GameObject objInfo;

    [SerializeField] List<UW_PropertyElementUI> properties;
    private SO_UW_Base _data;

    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnUnlockUltimateWeapon, CheckUnlock);
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);

        UW_UpgradeCellData dataUW = data as UW_UpgradeCellData;
        this._data = dataUW.Data;
        Refresh();
        txt_description.text = _data.Description;
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

        if(_data.id == UW_ID.SHOCKWAVE)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x + 30, rt.sizeDelta.y + 30);
        }
    }

    private void Refresh()
    {
        txt_title.text = _data.Name;
        SetImage(_data.iconUW);

        int index = 0;
        if (_data.levelMaxQuantity == 0) index = 1;
        for (int i = 0; i < properties.Count; i++)
        {
            var item = properties[i];
            item.SetData(_data, i + index);
        }
    }
    public void CheckUnlock(object obj = null)
    {
        if (obj != null && (UW_ID)obj != _data.id)
            return;
        gameObject.SetActive(_data.isUnlock);
        Refresh();
    }
    public void ClickOnIcon()
    {
        objProperties.SetActive(!objProperties.activeInHierarchy);
        objInfo.SetActive(!objInfo.activeInHierarchy);
    }

}
