using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataShopSO", menuName = "Data/Shop/DataShopSO", order = 0)]
public class DataShopSO : SerializedScriptableObject
{
    public Dictionary<TypePack, List<BasePack>> shopDicts = new();


    public T GetDataPackById<T>(TypePack id) where T : BasePack
    {
        if (shopDicts.ContainsKey(id))
            return shopDicts[id] as T;
        else
            return null;
    }
}



