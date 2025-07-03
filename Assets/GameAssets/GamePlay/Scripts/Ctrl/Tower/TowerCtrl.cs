using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCtrl : Singleton<TowerCtrl>
{
    private HPBarObject hpbar;
    private TowerData towerData;
    private TowerRevival towerRevival;
    public Transform bodyTower;
    public CircleCollider2D col;

    public TowerData TowerData => towerData;
    public TowerRevival TowerRevival => towerRevival;
    public HPBarObject HPBarTower => hpbar;

    public GameObject arrow;
    public GameObject iconArrow;
    public TowerUltimateWeapon towerUltimateWeapon;

    protected override void Awake()
    {
        base.Awake();
        hpbar = GetComponentInChildren<HPBarObject>();
        towerData = GetComponent<TowerData>();
        towerRevival = GetComponent<TowerRevival>();
        col = GetComponent<CircleCollider2D>();
    }

}
