using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoElement : MonoBehaviour
{
    [SerializeField] TMP_Text txt_title;
    [SerializeField] TMP_Text txt_info;
    [SerializeField] Image icon;
    public void Setup(DataProfile profile)
    {
        txt_title.text = profile.title;
        icon.sprite = profile.icon;
        txt_info.text = profile.info;
    }
}