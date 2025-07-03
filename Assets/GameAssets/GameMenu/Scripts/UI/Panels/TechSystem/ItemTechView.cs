using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTechView : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text txtEquip;
    [SerializeField]
    private TMP_Text txtLevel;
    public ItemTechSystemData cellData;

    public void SetUp(ItemTechSystemData cellData)
    {
        this.cellData = cellData;
        this.txtLevel.text = "Lv " + cellData.level;
    }

    public void OnClick()
    {
        PanelTechInfor.instance.SetUp(cellData);
    }
}
