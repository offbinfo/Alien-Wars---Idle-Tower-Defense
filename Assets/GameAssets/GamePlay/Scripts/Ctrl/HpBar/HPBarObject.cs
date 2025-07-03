using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class HPBarObject : GameMonoBehaviour
{

    private Object_DataInformation _objData;
    [SerializeField] Transform main;
    [SerializeField] TMP_Text txt_HP;
    [SerializeField]
    private bool isActive = false;
    [SerializeField] TMP_Text txtHPTest;
    [SerializeField] TMP_Text txtDmgTest;
    public bool isTower;

    private void Awake()
    {
        _objData = GetComponentInParent<Object_DataInformation>();
        _objData.OnHPChanged += UpdateUI;
        gameObject.SetActive(isActive);
    }

    private void UpdateUI(float value, float maxValue)
    {
/*        float roundedValue = Mathf.Round(value * 100f) / 100f;
        float roundedMaxValue = Mathf.Round(maxValue * 100f) / 100f;*/

        var roundedFormat = Extensions.FormatCompactNumber(value);
        var roundedMaxFormat = Extensions.FormatCompactNumber(maxValue);
        /*        if (roundedValue > 10 || roundedMaxValue > 10)
                {
                     roundedFormat = Mathf.FloorToInt(roundedValue);
                     roundedMaxFormat = Mathf.FloorToInt(roundedMaxValue);
                } else
                {
                     roundedFormat = roundedValue;
                     roundedMaxFormat = roundedMaxValue;
                } */

        var hp = roundedFormat.ToString();
        var maxHP = roundedMaxFormat.ToString();

        txt_HP.text = string.Format("{0}/{1}", hp, maxHP);
        main.localScale = new Vector3(value / maxValue, 1, 1);
    }

    public void InActiveHpBar(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
