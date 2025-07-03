using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUpgradeGamePlay : GameMonoBehaviour
{

    [SerializeField]
    private List<TabUpgradeView> tabUpgrades;
    [SerializeField]
    private TabInformationGamePlay tabInformation;

    [SerializeField]
    private Animator animatorContentUpgrade;
    [SerializeField] private RectTransform containerUpgrader;
    private const string nameCloseBtnAnim = "Close";
    private const string nameOpenBtnAnim = "Open";
    [SerializeField]
    private Animator animatorBuyX;

    private void Start()
    {
        BuildData();

        Invoke(nameof(OpenContentUpgrade), 0.5f);        
    }

    private void OpenContentUpgrade()
    {
        PlayAnimatorContentUpgrade(nameOpenBtnAnim);
    }

    private bool isOpenListBuyX = false;
    public void BtnBuyX()
    {
        if (isOpenListBuyX)
        {
            isOpenListBuyX = false;
            animatorBuyX.Play(nameCloseBtnAnim);
        }
        else
        {
            isOpenListBuyX = true;
            animatorBuyX.Play(nameOpenBtnAnim);
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

    private void PlayAnimatorContentUpgrade(string animationName)
    {
        int targetHeight = Mathf.Clamp((int)(Screen.height * 0.407f), 800, 880);

        containerUpgrader.DOSizeDelta(new Vector2(containerUpgrader.sizeDelta.x, targetHeight), 0.5f)
                  .SetEase(Ease.Linear);
        //animatorContentUpgrade.Play(animationName);
    }
}
