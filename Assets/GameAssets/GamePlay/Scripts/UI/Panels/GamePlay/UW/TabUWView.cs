using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUWView : MonoBehaviour
{
    [SerializeField]
    private UntimateWeaponElement uWEelement;
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private GameObject infoText;
    [SerializeField]
    private SO_UltimateWeaponManager weaponManager;

    private void Start()
    {
        BuildData();   
    }

    public void BuildData()
    {
        foreach(SO_UW_Base uw in weaponManager.ultimateWeapons)
        {
            UntimateWeaponElement cellView = Instantiate(uWEelement, parent);
            cellView.SetUp(uw);
        }
        CheckTabUWEmpty();
    }

    private void CheckTabUWEmpty()
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return;
            }
        }
        infoText.gameObject.SetActive(true);
    }
}
