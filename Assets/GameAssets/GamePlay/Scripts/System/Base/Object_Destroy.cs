using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object_Destroy : GameMonoBehaviour
{
    public abstract void Destroy(Damager dmg);
}

[System.Serializable]
public class Damager : ICloneable
{
    public float damage;
    public DamageType type;
    public GameObject objAttack;
    public bool isShockwave;

    public object Clone()
    {
        return MemberwiseClone();
    }
    public Damager _Clone()
    {
        return Clone() as Damager;
    }
}
