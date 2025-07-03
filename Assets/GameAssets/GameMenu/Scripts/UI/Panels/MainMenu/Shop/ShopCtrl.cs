using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCtrl : MonoBehaviour
{

    [SerializeField] RectTransform content;
    [SerializeField] RectTransform center;

    [PurchaseID] public string idPack1, idPack2, idPack3;

    public static ShopCtrl instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
    }

    public ScrollRect scrollRect;

    public void StopScrolling()
    {
        scrollRect.velocity = Vector2.zero;
    }

    #region BUTTON

    #endregion
    #region IAP Reward callback
    public void GemPack(int gem)
    {
        GameDatas.Gem += gem;
        GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + gem);
        var amount = Mathf.Clamp(gem / 10, 5, 20);
        var targetUI = TopUI_Currency.instance ? TopUI_Currency.instance.gemIcon.transform.position : CurrencyContainer.instance._trans_gem.position;
        ObjectUI_Fly_Manager.instance.Get(amount, center.position, targetUI, CurrencyType.GEM);

    }
    #endregion
}
