using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUltimateChoose : GameMonoBehaviour
{

    [SerializeField]
    private UWoptionElement uWoptionElement;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TabUltimateInfor tabUltimateInfor;

    private void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        List<SO_UW_Base> data = Cfg.UWCtrl.UWeaponManager.ultimateWeapons;
        foreach (var item in data)
        {
            var element = Instantiate(uWoptionElement, content);
            element.OnClick = OpenTabInforUW;
            UWoptionElementCellData cellData = new(item);
            element.SetData(cellData);
        }
    }

    public void OpenTabInforUW(SO_UW_Base data)
    {
        tabUltimateInfor.SetData(data);
    }

    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
