using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopUI_Currency : MonoBehaviour
{

    public static TopUI_Currency instance;
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
