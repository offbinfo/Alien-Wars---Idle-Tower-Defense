using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BaseDataEnemySpawn", menuName = "Data/BaseDataEnemySpawn/SO_BaseDataEnemySpawn", order = 0)]
public class SO_BaseDataEnemySpawn : SerializedScriptableObject
{
    public Dictionary<WorldType, BaseDataInforEnemy> dataBaseInforEnemys = new();

    public BaseDataInforEnemy GetBaseDataInforEnemy(WorldType worldType)
    {
        if (dataBaseInforEnemys.ContainsKey(worldType)) return dataBaseInforEnemys[worldType];
        return null;
    }
}

public class BaseDataInforEnemy
{
    public float maxHp;
    public float damage;
    public float attackRange;
    public float attackSpeed;
    public float speedMove;
    public double goldWorldBase;
}
