using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Shield : GameMonoBehaviour
{

    [SerializeField] GameObject shieldGraphic;
    float ratioShieldHP;
    public float shieldHP
    {
        get
        {
            return ratioShieldHP * shieldHPMax;
        }
        set
        {
            value = Mathf.Min(value, shieldHPMax);
            ratioShieldHP = value / shieldHPMax;
            shieldGraphic.SetActive(ratioShieldHP > 0);
        }
    }
    public virtual float shieldHPMax => 0;
    public virtual float timeRefillShield => 5f;
    private void Start()
    {
        shieldGraphic.SetActive(false);
        StartCoroutine(IE_RefillShield());
    }
    IEnumerator IE_RefillShield()
    {
        do
        {
            yield return new WaitUntil(() => shieldHPMax > 0);
            shieldHP = shieldHPMax;
            shieldGraphic.SetActive(true);
            yield return new WaitUntil(() => shieldHP <= 0);
            ExplodeShield();


            yield return Yielders.Get(timeRefillShield);
        }
        while (true);
    }
    protected virtual void ExplodeShield()
    {

    }
}
