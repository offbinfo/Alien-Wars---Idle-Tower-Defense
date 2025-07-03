using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public HPBarObject hpbar;

    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBarObject>();
    }
}
