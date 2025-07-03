using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Data/MapData", order = 0)]
public class SO_MapData : SerializedScriptableObject
{
    public Dictionary<WorldType, MapData> mapDatas = new();   

    public MapData GetMapDataByWorldType(WorldType type)
    {
        if(mapDatas.ContainsKey(type))
        {
            return mapDatas[type];
        }
        return mapDatas[0];
    }
}

public class MapData
{
    public WorldType worldType;
    public Sprite leftBoard1;
    public Sprite leftBoard2;
    public Sprite rightBoard1;
    public Sprite rightBoard2;
    public Sprite bg;
}
