using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : MonoBehaviour
{
    Animator anim;
    Object_Pool objPool;
    private void Awake()
    {
        objPool = GetComponent<Object_Pool>();
        anim = GetComponent<Animator>();

        objPool.AddEventInit(OnInit);
        objPool.AddEventReturn(OnReturn);
    }
    private void OnInit(object o)
    {
        anim.Play("clip");
    }
    private void OnReturn()
    {
        //anim.Play("idle");
    }
}
