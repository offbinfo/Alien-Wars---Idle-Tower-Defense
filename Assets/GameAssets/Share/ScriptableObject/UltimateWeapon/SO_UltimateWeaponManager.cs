using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UltimateWeaponManager", menuName = "Data/UltimateWeapon/UltimateWeaponManager", order = 0)]
public class SO_UltimateWeaponManager : SerializedScriptableObject
{
    public List<SO_UW_Base> ultimateWeapons = new();

    public int[] price = new int[] { 5, 50, 150, 300, 800, 1250, 1750, 2400, 3000 };
    public Dictionary<UW_ID, SO_UW_Base> dicsData = new Dictionary<UW_ID, SO_UW_Base>();

    public int CountUnlock
    {
        get
        {
            int count = 0;
            foreach (var data in ultimateWeapons)
            {
                if (data.isUnlock)
                    count++;
            }
            return count;
        }
    }

    public T GetDataById<T>(UW_ID id) where T : SO_UW_Base
    {
        if (dicsData.ContainsKey(id))
            return dicsData[id] as T;
        else
            return null;
    }

#if UNITY_EDITOR
    [Button("Load Data Dict")]
    private void InitFirst()
    {
        foreach (var item in ultimateWeapons)
        {
            dicsData.Add(item.id, item);
        }
    }

#endif
}
