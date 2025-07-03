using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_DataInformation : GameMonoBehaviour
{

    [SerializeField] protected SO_ObjectData objectData;
    public Action<float, float> OnHPChanged;

    public virtual float maxHP { 
            get { return objectData.maxHP; }
            set
            {
                objectData.maxHP = value;
            }
        }
    public virtual float damage
    {
        get { return objectData.damage; }
        set
        {
            objectData.damage = value;
        }
    }
    public virtual float attackRange
    {
        get { return objectData.attackRange; }
        set
        {
            objectData.attackRange = value;
        }
    }
    public float attackSpeed
    {
        get { return objectData.attackSpeed; }
        set
        {
            objectData.attackSpeed = value;
        }
    }
    float ratioHP;
    public float hpCurrent
    {
        get
        {
            return ratioHP * maxHP;
        }
        set
        {
            value = Mathf.Min(value, maxHP);
            ratioHP = value / maxHP;
            OnHPChanged?.Invoke(value, maxHP);
        }
    }

    public bool isAlive => hpCurrent > 0;
    public virtual float regen_speed => objectData.regenHPPerSecond;
    public virtual float damage_resistant => objectData.damage_resistant;
    public virtual float dodge_chance => objectData.dodge_chance;
    public virtual float resistance_chance => objectData.resistanceDmg;
    public virtual float refect_damage_chance => 0;
    public virtual float reflect_damage => 0;
    public virtual void OnEnable()
    {
        hpCurrent = maxHP;
    }
    private void Start()
    {
        hpCurrent = maxHP;
    }
}
