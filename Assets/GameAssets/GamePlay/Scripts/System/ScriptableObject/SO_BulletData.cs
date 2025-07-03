using ProjectTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bullet")]
public class SO_BulletData : ScriptableObjectCustom, I_MoveData
{

    public float _spdMove;

    public float spdMove
    {
        get => _spdMove;
        set => _spdMove = value;
    }
}
