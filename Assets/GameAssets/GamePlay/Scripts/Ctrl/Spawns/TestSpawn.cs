using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < 1000; i++)
        {
            Vector3 posRandom = transform.position + Extensions.GetRandomPosition(8f);
            PoolCtrl.instance.Get(PoolTag.ENEMY0, posRandom, Quaternion.identity);
            yield return null;
        }
    }
}
