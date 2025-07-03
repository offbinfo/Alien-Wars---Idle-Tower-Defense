using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BaseDataEnemySpawnArena", menuName = "Data/BaseDataEnemySpawn/SO_BaseDataEnemySpawnArena", order = 0)]
public class SO_BaseDataEnemySpawnArena : SerializedScriptableObject
{

    public Dictionary<TypeRank, BaseDataInforEnemy> dataBaseInforEnemys = new();

    public BaseDataInforEnemy GetBaseDataInforEnemy(TypeRank typeRank)
    {
        if (dataBaseInforEnemys.ContainsKey(typeRank)) return dataBaseInforEnemys[typeRank];
        return null;
    }
}
