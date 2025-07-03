using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectUI_Fly_Manager : MonoBehaviour
{
    public static ObjectUI_Fly_Manager instance;
    [SerializeField] private ObjectUI_Fly objUIFly;
    Queue<ObjectUI_Fly> objs = new Queue<ObjectUI_Fly>();
    [SerializeField] private int amountObjectStart;
    Canvas canvas;

    private void Awake()
    {
        instance = this;
        canvas = GetComponentInParent<Canvas>();
        DebugCustom.Log(canvas.gameObject.name);
        for (int i = 0; i < amountObjectStart; i++)
        {
            var obj = Instantiate(objUIFly, transform);
            obj.OnInit();
            objs.Enqueue(obj);
        }
    }
    private float Get(Vector3 pos, Vector3 target, CurrencyType currency)
    {
        if (objs.Count <= 0)
        {
            objs.Enqueue(Instantiate(objUIFly, transform));
        }
        var obj = objs.Dequeue();
        return obj.Fly(pos, target, canvas, currency);

    }
    public void Get(int amount, Vector3 pos, Vector3 target, CurrencyType currency, Action OnComplete = null, Action OnFirstObjectComplete = null)
    {
        List<float> list_time_fly = new List<float>();
        for (int i = 0; i < amount; i++)
        {
            var value = Get(pos, target, currency);
            list_time_fly.Add(value);
        }
        var maxTime = list_time_fly.Max();
        var minTime = list_time_fly.Min();
        if (OnComplete != null) DOVirtual.DelayedCall(1.25f, () => {
            OnComplete?.Invoke();
        }).SetUpdate(true);
        if (OnFirstObjectComplete != null) DOVirtual.DelayedCall(minTime, () => {
            OnFirstObjectComplete?.Invoke();
        }).SetUpdate(true);

    }
    public void Return(ObjectUI_Fly obj)
    {
        obj.gameObject.SetActive(false);
        objs.Enqueue(obj);
    }
}
