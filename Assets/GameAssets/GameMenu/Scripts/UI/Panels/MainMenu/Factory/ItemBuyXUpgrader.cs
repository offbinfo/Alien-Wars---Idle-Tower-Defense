using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuyXUpgrader : MonoBehaviour
{
    [SerializeField]
    private int bonus;
    [SerializeField]
    private int index;
    [SerializeField]
    private bool max;

    private void Start()
    {
        ChecKUnlock(null);
        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, ChecKUnlock);
    }

    private void OnEnable()
    {
        ChecKUnlock(null);
    }

    private void ChecKUnlock(object o)
    {
        if(GameDatas.GetLevelSubjectLab(IdSubjectType.BUYX) > index)
        {
            gameObject.SetActive(true); 
        } else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnUsingBuyX()
    {
        GameDatas.CountBuyXUpgrader = bonus;
    }
}
