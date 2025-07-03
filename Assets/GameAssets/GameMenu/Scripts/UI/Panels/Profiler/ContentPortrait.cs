using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class ContentPortrait : MonoBehaviour
{
    [SerializeField] AvatarElement prefab;
    List<AvatarElement> elements;
    [SerializeField]
    private TMP_InputField inputField;


    AvatarElement currentElement;
    int indexAvatar = 0;
    private void Awake()
    {
        elements = new List<AvatarElement>();
        List<DataAvatar> avatars = AvatarSource.instance.dataAvatars;
        for (int i = 0; i < avatars.Count; i++)
        {

            var a = Instantiate(prefab, transform);
            a.SetUp(avatars[i], i);
            elements.Add(a);
        }
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnRefeshAvatarUnlock, OnRefeshAvatarUnlock);
    }

    private void OnRefeshAvatarUnlock(object o)
    {
        foreach (AvatarElement element in elements)
        {
            if (GameDatas.IsUnlockAvatar(element.typeAvatar))
            {
                element.CheckUnlockAvatar();
            }
        }
    }

    private void OnEnable()
    {
        var element = elements.Find(x => x.index == GameDatas.id_avatar);
        SelectElement(element);
    }
    public void SelectElement(AvatarElement e)
    {
        currentElement?.Selection(false);
        e?.Selection(true);
        indexAvatar = (int)e.typeAvatar;
        currentElement = e;
    }
    public void Confirm()
    {
        DebugCustom.Log("SkinChanged");
        QuestEventManager.SkinChanged(1);
        GameDatas.id_avatar = indexAvatar;
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnRefeshAvatarUnlock, OnRefeshAvatarUnlock);
    }
}
