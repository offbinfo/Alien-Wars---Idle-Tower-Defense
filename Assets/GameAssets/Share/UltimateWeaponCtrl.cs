using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateWeaponCtrl : GameMonoBehaviour
{
    [SerializeField]
    private SO_UltimateWeaponManager datas;

    public SO_UltimateWeaponManager UWeaponManager => datas;

    public int GetCountUW()
    {
        return datas.ultimateWeapons.Count;
    }

    public int GetPriceUnlockUW()
    {
        return datas.price[datas.CountUnlock];
    }
}
