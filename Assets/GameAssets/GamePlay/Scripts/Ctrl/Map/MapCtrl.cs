using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Animations.AimConstraint;

public class MapCtrl : GameMonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _leftBoard1;
    [SerializeField]
    private SpriteRenderer _leftBoard2;
    [SerializeField]
    private SpriteRenderer _rightBoard1;
    [SerializeField]
    private SpriteRenderer _rightBoard2;
    [SerializeField]
    private SpriteRenderer _bg;
    [SerializeField]
    private SO_MapData sO_MapData;
    public WorldType worldType = WorldType.WORLD1;
    private MapData _mapData;

    private void Awake()
    {
        worldType = (WorldType)GameDatas.CurrentWorld;
        _mapData = sO_MapData.GetMapDataByWorldType(worldType);
    }

    private void Start()
    {
        InitMap();
    }

    private void InitMap()
    {
        if (_mapData != null)
        {
            _leftBoard1.sprite = _mapData.leftBoard1;
            _leftBoard2.sprite = _mapData.leftBoard2;
            _rightBoard1.sprite = _mapData.rightBoard1;
            _rightBoard2.sprite = _mapData.rightBoard2;
            _bg.sprite = _mapData.bg;
        }
    }

#if UNITY_EDITOR
    [Button]
    public void TestChangeBG()
    {
        _mapData = sO_MapData.GetMapDataByWorldType(worldType);
        InitMap();
    }
#endif
}
