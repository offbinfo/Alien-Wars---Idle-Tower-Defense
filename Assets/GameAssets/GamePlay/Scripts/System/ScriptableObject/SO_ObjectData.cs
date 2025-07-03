using ProjectTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BaseObjectData", menuName = "Data/BaseObjectData", order = 0)]
public class SO_ObjectData : ScriptableObjectCustom
{
    public float maxHP;
    public float regenHPPerSecond;
    public float damage_resistant;
    public float dodge_chance;
    public float damage;
    public float attackRange;
    public float attackSpeed;
    public float resistanceDmg;
}

