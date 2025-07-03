using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUI_Currency_Horizontal : MonoBehaviour
{
    public static TopUI_Currency_Horizontal instance;
    public RectTransform goldIcon;
    public RectTransform gemIcon;
    public RectTransform powerStoneIcon;
    public RectTransform tourament;
    public RectTransform badges;

    public GameObject touramanent;
    private void Awake()
    {
        instance = this;
        if (touramanent != null)
        {
            touramanent.SetActive(false);
        }
    }
}
