using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface I_BulletEnemy
{
    void SetUp(Damager damager);
}

public class BulletCollidedEnemy : GameMonoBehaviour, I_BulletEnemy
{
    Damager damager;
    protected Object_Destroy objDestroy;
    protected Object_Pool objPool;
    private GameObject _tower;

    protected virtual void Awake()
    {
        objDestroy = GetComponent<Object_Destroy>();
        objPool = GetComponent<Object_Pool>();
    }

    private void Start()
    {
        _tower = GPm.Tower.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_TOWER))
        {
            _tower.GetComponent<Object_TakeDamage>()?.TakeDamage(damager);
            gameObject.SetActive(false);    
            objPool.ReturnPool();
            objDestroy?.Destroy(null);
        }
    }

    public void SetUp(Damager damager)
    {
        this.damager = damager;
    }
}
