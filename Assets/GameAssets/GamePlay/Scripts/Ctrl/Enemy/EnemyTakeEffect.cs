using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Slow
{
    void Slow(float slowPower, float timeSlow);
}
public interface I_Stun
{
    void Stun(float timeStun);
}
public interface I_Poison
{
    void Poison(float timePoison, Damager dmg);
}
public interface I_KnockBack
{
    void KnockBack();
}

public class EnemyTakeEffect : GameMonoBehaviour, I_Slow, I_Stun, I_KnockBack, I_Poison
{
    private EnemyTakeDamage _takedmg;
    //knock back
    private float _timeKnockBack = 0.15f;
    private float _spdKnockBack => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.knockback_force) + 2f;
    private Coroutine c_KnockBack;

    //stun
    public bool isStun;
    private Coroutine c_Stun;

    //slow
    public float slowPower
    {
        get
        {
            if (list_slowPower.Count <= 0)
                return 0;
            return Mathf.Max(list_slowPower.ToArray());
        }
    }

    //poison
    private float timePoison;
    private Coroutine c_Poison;

    private Transform posTower;

    private void Awake()
    {
        _takedmg = GetComponent<EnemyTakeDamage>();
    }

    private void Start()
    {
        posTower = GPm.Tower.transform;
    }

    private void OnEnable()
    {
        //slowPower = 0;
        isStun = false;
        StopAllCoroutines();
        list_slowPower.Clear();
    }

    List<float> list_slowPower = new List<float>();


    public void Slow(float slowPower, float timeSlow)
    {
        list_slowPower.Add(slowPower);
        if (gameObject.activeInHierarchy) StartCoroutine(IE_Slow(slowPower, timeSlow));
    }
    IEnumerator IE_Slow(float slowPower, float timeSlow)
    {
        yield return Yielders.Get(timeSlow);
        list_slowPower.Remove(slowPower);
    }

    public void Stun(float timeStun)
    {
        isStun = true;
        if (c_Stun != null) StopCoroutine(c_Stun);
        if (gameObject.activeInHierarchy) c_Stun = StartCoroutine(IE_UnStun(timeStun));
    }
    private IEnumerator IE_UnStun(float timeStun)
    {
        yield return Yielders.Get(timeStun);
        isStun = false;
    }

    public void KnockBack()
    {
        if (c_KnockBack != null) StopCoroutine(c_KnockBack);
        if (gameObject.activeInHierarchy) c_KnockBack = StartCoroutine(IE_KnockBack());
    }
    private IEnumerator IE_KnockBack()
    {
        var dir = (transform.position - posTower.position).normalized;
        var totalTime = 0f;
        while (totalTime < _timeKnockBack)
        {
            yield return Yielders.Get(Time.deltaTime);
            transform.position += dir * _spdKnockBack * Time.deltaTime;
            totalTime += Time.deltaTime;
        }
    }

    public void Poison(float timePoison, Damager dmg)
    {
        this.timePoison = timePoison;
        if (c_Poison != null) StopCoroutine(c_Poison);
        if (gameObject.activeInHierarchy) c_Poison = StartCoroutine(IE_Poison(dmg));
    }
    private IEnumerator IE_Poison(Damager dmg)
    {
        while (timePoison > 0)
        {
            yield return Yielders.Get(0.25f);
            timePoison -= 0.25f;
            _takedmg.TakeDamage(dmg);
        }
    }
}
