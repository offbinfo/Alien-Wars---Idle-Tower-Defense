using AYellowpaper.SerializedCollections;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUltimateWeapon : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<UW_ID, GameObject> ultimateWeaponDicts = new();

    private void Start()
    {
        CheckActiveUW();
    }

    public void ActiveUW(UW_ID id, bool isActive)
    {
        ultimateWeaponDicts[id].SetActive(isActive);
    } 

    private void CheckActiveUW()
    {
        foreach (var kvp in ultimateWeaponDicts)
        {
            kvp.Value.SetActive(GameDatas.isUnlockUltimateWeapon(kvp.Key));
        }
    }
}
