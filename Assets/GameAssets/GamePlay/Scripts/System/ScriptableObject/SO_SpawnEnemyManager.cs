using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpawnEnemyManager", menuName = "Data/SO_SpawnEnemyManager", order = 0)]
public class SO_SpawnEnemyManager : ScriptableObject
{
    public List<PoolTag> NormalEnemys = new();
    public List<PoolTag> FastEnemys = new();
    public List<PoolTag> TankEnemys = new();
    public List<PoolTag> RangedEnemys = new();
    public List<PoolTag> GuardenEnemys = new();
}
