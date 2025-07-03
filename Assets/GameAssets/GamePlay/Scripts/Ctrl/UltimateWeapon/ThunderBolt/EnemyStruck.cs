using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStruck : MonoBehaviour
{
/*    private Object_Pool object_Pool;

    private void Awake()
    {
        object_Pool = GetComponent<Object_Pool>();  
    }*/

    private void Start()
    {
        Destroy(gameObject, .4f);
    }
}
