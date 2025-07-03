using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarElement : MonoBehaviour
{
    [SerializeField] Image img_Frame;
    [SerializeField] GameObject obj_Select;
    public int index;
    public TypeAvatar typeAvatar;
    [SerializeField]
    private GameObject lockAvatar;
    private bool isUnlock;
    private BuyingType buyingType;

    private void OnEnable()
    {
        CheckUnlockAvatar();
    }

    public void SetUp(DataAvatar dataAvatar, int index)
    {
        img_Frame.sprite = dataAvatar.sprite;
        this.index = index;
        this.typeAvatar = dataAvatar.typeAvatar;
        this.buyingType = dataAvatar.buyingType;
        CheckUnlockAvatar();
    }

    public void CheckUnlockAvatar()
    {
        if (GameDatas.IsUnlockAvatar(typeAvatar) || buyingType == BuyingType.NoBuy)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock = false;
        }
        lockAvatar.SetActive(!isUnlock);
    }

    public void OnClick()
    {
        if (!isUnlock) return;
        GetComponentInParent<ContentPortrait>().SelectElement(this);
    }
    public void Selection(bool value)
    {
        obj_Select.SetActive(value);
    }
}


