using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UWoptionElement : BaseUICellView
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text txtContent;
    [SerializeField]
    private TMP_Text textDesc;
    private UW_ID typeUW;

    public Action<SO_UW_Base> OnClick;
    private SO_UW_Base dataUW;

    private void OnEnable()
    {
        CheckActive();
    }

    private void CheckActive()
    {
        gameObject.SetActive(!GameDatas.isUnlockUltimateWeapon(typeUW));
    }

    public override void SetData(BaseUICellData data)
    {
        base.SetData(data);
        UWoptionElementCellData cellData = data as UWoptionElementCellData;
        if (cellData != null)
        {
            dataUW = cellData.Data;
            typeUW = cellData.Data.id;

            SetImage(cellData.Data.iconUW);

            txtContent.text = cellData.Data.Name;
            textDesc.text = cellData.Data.Description;
        }
        CheckActive();
    }

    void SetImage(Sprite sprite)
    {
        if(typeUW == UW_ID.SHOCKWAVE)
        {
            icon.sprite = sprite;
            icon.SetNativeSize();
        } else
        {
            if (sprite == null || icon == null) return;

            int sizeDefault = 150;
            icon.sprite = sprite;
            float originalWidth = sprite.texture.width;
            float originalHeight = sprite.texture.height;

            float aspectRatio = originalWidth / originalHeight;

            RectTransform rt = icon.GetComponent<RectTransform>();

            if (aspectRatio > 1)
            {
                rt.sizeDelta = new Vector2(sizeDefault, sizeDefault / aspectRatio);
            }
            else
            {
                rt.sizeDelta = new Vector2(sizeDefault * aspectRatio, sizeDefault);
            }
        }
    }

    public void OnShowInforUW()
    {
        if (dataUW == null)
            return;
        OnClick?.Invoke(dataUW);
    }
}
