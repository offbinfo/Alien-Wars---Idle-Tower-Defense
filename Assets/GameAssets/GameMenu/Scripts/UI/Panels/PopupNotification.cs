using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupNotification : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtCOntent;

    public void SetUp(string content)
    {
        txtCOntent.text = content;
    }
}
