using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_EnemyData : SO_ObjectData, I_MoveData
{
    public float _spdMove;

    public float spdMove
    {
        get => _spdMove;
        set => _spdMove = value;
    }
    public Vector3 hpScale_formula;
    public Vector3 dmgScale_formula;
    public EnemyAttackType enemyAttackType;
}
