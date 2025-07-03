using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelTechInfor : Singleton<PanelTechInfor>
{

    [SerializeField]
    private TMP_Text txtRarity;
    [SerializeField]
    private TMP_Text txtName;
    [SerializeField]
    private TMP_Text txtValue;
    [SerializeField]
    private TMP_Text txtLevel;

    [SerializeField]
    private GameObject uniqueEffect;
    [SerializeField]
    private TMP_Text txtDescUniqueEffect;
    [SerializeField]
    private TMP_Text txtPriceUpgrade;
    [SerializeField]
    private TMP_Text txtCoinUpgrade;

    [SerializeField]
    private List<SubStatsInfor> subStats;

    public void SetUp(ItemTechSystemData cellData)
    {
        gameObject.SetActive(true);
        txtRarity.text = cellData.typeRarityTech.ToString();
        txtName.text = cellData.typeTech.ToString();
        txtValue.text = cellData.typeClassTechSystem.ToString();

        txtLevel.text = $"Lv {cellData.level}";
    }

    public void Equip()
    {
        
    }

    public void MaxUpgrade()
    {

    }

    public void Upgrade()
    {
            
    }
}
