using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizesElement : MonoBehaviour
{

    [SerializeField]
    private TypeRank typeRank;
    [SerializeField]
    private GameObject frameSelect;
    public Action<TypeRank ,GameObject> OnClick;

    public void OnClickElement()
    {
        OnClick?.Invoke(typeRank, frameSelect);
    }
}
