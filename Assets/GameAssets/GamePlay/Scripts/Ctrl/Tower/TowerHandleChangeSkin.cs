using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHandleChangeSkin : GameMonoBehaviour
{

    [SerializeField]
    [SerializedDictionary("Type", "Obj Tower")]
    private SerializedDictionary<TypeSkinTower, GameObject> dictTower = new();

    private void Start()
    {
        ChangeSkin();
        EventDispatcher.AddEvent(EventID.OnChangedSkinTower, OnChangedSkinTower);
    }

    private void OnChangedSkinTower(object o)
    {
        DebugCustom.Log("OnChangeSkin");
        ChangeSkin();
    }

    private void ChangeSkin()
    {
        //TypeSkinTower typeSkinUsing = (TypeSkinTower)GameDatas.id_avatar;

    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnChangedSkinTower, OnChangedSkinTower);
    }
}
