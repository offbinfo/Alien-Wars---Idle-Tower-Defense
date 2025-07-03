using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour, I_MoveData
{
    [SerializeField] SO_BulletData data;

    public float spdMove
    {
        get
        {
            return data._spdMove;
        }
        set
        {

        }
    }
}
