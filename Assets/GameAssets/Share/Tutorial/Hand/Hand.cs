using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public RectTransform handGraphic;
    public Coroutine Action(GameObject obj, Func<bool> condition)
    {
        gameObject.SetActive(true);
        ControlAllButton.ActiveButton(obj);
        return StartCoroutine(IE_Action(obj, condition));
    }
    private IEnumerator IE_Action(GameObject obj, Func<bool> condition)
    {
        handGraphic.position = obj.transform.position;
        yield return new WaitUntil(() => condition.Invoke());
        ControlAllButton.ActiveAllButton();
        gameObject.SetActive(false);
    }
}
