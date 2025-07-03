using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Obj_Edit : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private Image imgAvatar;

    private void Start()
    {
        ChangeInforUser(null);
        EventDispatcher.AddEvent(EventID.OnChangedName, ChangeInforUser);
        EventDispatcher.AddEvent(EventID.OnChangedAvatar, ChangeInforUser);
    }

    private void ChangeInforUser(object o)
    {
        txtName.text = GameDatas.user_name;
        imgAvatar.sprite = AvatarSource.instance.dataAvatars[GameDatas.id_avatar].sprite;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
