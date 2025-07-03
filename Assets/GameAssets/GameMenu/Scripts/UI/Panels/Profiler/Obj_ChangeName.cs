using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Obj_ChangeName : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField inputField;

    public void ChangedName()
    {
        if (inputField.text.Length < 3)
            return;
        GameDatas.user_name = inputField.text;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
