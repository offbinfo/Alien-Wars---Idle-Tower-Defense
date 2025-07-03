using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextPop : MonoBehaviour
{
    private Object_Pool objPool;
    [SerializeField] TMP_Text txt_pop;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        objPool = GetComponent<Object_Pool>();
        objPool.AddEventInit(SetText);
        objPool.AddEventInit((o) =>
        {
            anim.Play("clip");
        });
    }

    private void SetText(object obj)
    {
        txt_pop.text = obj.ToString();
    }
}
