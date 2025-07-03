using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object_Attack : GameMonoBehaviour
{

    protected virtual bool canAttack => objData.isAlive;
    protected virtual float attackSpeed => objData.attackSpeed;
    float timeAttack => 1f / attackSpeed;
    protected Object_DataInformation objData;
    protected virtual Damager damager
    {
        get
        {
            var dm = new Damager();
            dm.damage = objData.damage;
            dm.type = DamageType.NORMAL;
            dm.objAttack = gameObject;
            return dm;
        }
    }

    public virtual void Awake()
    {
        objData = GetComponent<Object_DataInformation>();
    }

    private void OnEnable()
    {
        StartCoroutine(I_Attack());
    }

    public abstract void Attack();

    IEnumerator I_Attack()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitUntil(() => canAttack);
            Attack();
            yield return Yielders.Get(timeAttack);
        }
    }
}
