using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomahawkTower : GameMonoBehaviour
{
    [SerializeField] private UW_ID ID;
    private SO_UW_Active data;
    private TowerAttack tower_Attack;
    private int countAllMissile;
    private float dmgBonus;
    private int quantity;
    private int cooldown;

    private Damager damager;

    private void Start()
    {
        if (!GameDatas.isUnlockUltimateWeapon(ID))
            return;

        data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Active>(ID);
        tower_Attack = TowerCtrl.instance.GetComponent<TowerAttack>();

        dmgBonus = data.GetCurrentDmg();
        quantity = data.GetCurrentQuantity();
        cooldown = data.GetCurrentCooldown();

        damager = new Damager
        {
            damage = tower_Attack.dmg * dmgBonus,
            type = DamageType.NORMAL
        };

        countAllMissile = 0;

        EventDispatcher.AddEvent(EventID.OnMissileDestroy, (obj) => { countAllMissile--; });

        StartCoroutine(HandleSpawnMissile());
    }

    private IEnumerator HandleSpawnMissile()
    {
        yield return Yielders.Get(cooldown);
        while (true)
        {
            while (countAllMissile < quantity)
            {
                SpawnMissile();
                yield return Yielders.Get(0.1f);
            }

            yield return Yielders.Get(cooldown); 
        }
    }

    private void SpawnMissile()
    {
        var objPool = PoolCtrl.instance.Get(PoolTag.MISSILE, TowerCtrl.instance.transform.position, Quaternion.identity);
        var bullet = objPool.GetComponent<I_Bullet>();
        bullet.SetUp(damager, 0, 0, 0, 100, false, 0, false, false);
        countAllMissile++;
    }
}
