using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TabUpgraderManager : Singleton<TabUpgraderManager>
{

    [SerializeField]
    private TabUpgradeFactory[] tabUpgrades;
    private int indexTab = -1;
    [SerializeField]
    private List<Animator> animatorBtns;
    private bool isActiveTab;

    private const string nameCloseBtnAnim = "Close";
    private const string nameOpenBtnAnim = "Open";

    [SerializeField]
    private List<UpgraderClusterUpgradeTab> upgraderClusterUpgradeTabs;
    public List<UpgraderFactoryElement> upgraderFactoryElements;

    [SerializeField]
    private Animator animatorBuyX;

    private int IndexTabUpgrade 
    {  
        get { return indexTab; }
        set {
            if (indexTab != value)
            {
                indexTab = value;
                InActiveTab();
                animatorBtns[indexTab].Play(nameOpenBtnAnim);
                tabUpgrades[indexTab].gameObject.SetActive(true);
            }
        }
    }

    public TabUpgradeFactory[] TabUpgrades { get => tabUpgrades; }

    private void Start()
    {
        BuildData();
        InitDefault();
    }
    private bool isOpenListBuyX = false;

    public void BtnBuyX()
    {
        if(isOpenListBuyX)
        {
            isOpenListBuyX = false;
            animatorBuyX.Play(nameCloseBtnAnim);
        } else
        {
            isOpenListBuyX = true;
            animatorBuyX.Play(nameOpenBtnAnim);
        }
    } 

    private void OnEnable()
    {
        animatorBtns[0].Play(nameOpenBtnAnim);
    }

    public void OnRefreshClusterUpgrader(/*object o*/)
    {
        foreach (var upgrader in upgraderClusterUpgradeTabs)
        {
            upgrader.OnRefreshClusterUpgrade();
        }
        foreach (var factory in upgraderFactoryElements)
        {
            factory.CheckUnlockItemUpgrade();
        }
        foreach (var tab in tabUpgrades)
        {
            tab.gameObject.SetActive(false);
        }
        /*IndexTabUpgrade = 0;
        tabUpgrades[0].gameObject.SetActive(true);*/
        for (int i = 0; i < tabUpgrades.Length; i++)
        {
            tabUpgrades[i].gameObject.SetActive(false);
        }
        tabUpgrades[0].gameObject.SetActive(true);
    }

    public void ChangeTab(int index)
    {
        IndexTabUpgrade = index;
    }

    private void InitDefault()
    {
        InActiveTab();
        animatorBtns[0].Play(nameOpenBtnAnim);
        tabUpgrades[0].gameObject.SetActive(true);
    }

    private void InActiveTab()
    {
        for (int i = 0; i < tabUpgrades.Length; i++)
        {
            animatorBtns[i].Play(nameCloseBtnAnim);
            tabUpgrades[i].gameObject.SetActive(false);
        }
    }

    private void BuildData()
    {
        foreach (var tab in tabUpgrades)
        {
            tab.SetData(Cfg.upgraderCtrl.
                UpgradeManager.GetUpgradeDataByKey(tab.upgradeGroup));
        }
    }
}
