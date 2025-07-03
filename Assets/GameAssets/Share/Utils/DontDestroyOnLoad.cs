using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : GameMonoBehaviour
{
    private static DontDestroyOnLoad instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(this);
    }
}
