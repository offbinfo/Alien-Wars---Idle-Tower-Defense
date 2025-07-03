using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UntimateWeaponElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtUW;
    public UW_ID typeUW;
    private bool isActive;

    [SerializeField]
    private GameObject btnActive;
    [SerializeField]
    private GameObject btnUnActive;

    public void SetUp(SO_UW_Base data)
    {
        typeUW = data.id;
        txtUW.text = data.Name;

        CheckUnlock();
    }

    public void BtnActive()
    {
        isActive = !isActive;
        TowerCtrl.instance.towerUltimateWeapon.ActiveUW(typeUW, isActive);

        CheckBtnActive();
    }

    private void CheckBtnActive()
    {
        btnActive.SetActive(isActive);
        btnUnActive.SetActive(!isActive);
    }

    private void CheckUnlock()
    {
        bool isUnlock = GameDatas.isUnlockUltimateWeapon(typeUW);
        gameObject.SetActive(isUnlock);
        isActive = isUnlock;

        CheckBtnActive();
    }

}
