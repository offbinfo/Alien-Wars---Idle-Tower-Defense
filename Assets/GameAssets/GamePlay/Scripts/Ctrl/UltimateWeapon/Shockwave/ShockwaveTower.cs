using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTower : GameMonoBehaviour
{
    [SerializeField]
    private UW_ID ID;

    private SO_UW_Active data;
    private TowerAttack towerAttack;
    private Damager cachedDamager;
    private WaitForSeconds waitOneSecond;
    private WaitForSeconds waitCooldown;

    private SO_UW_Active Data
    {
        get
        {
            if (data == null)
                data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Active>(ID);
            return data;
        }
    }

    private float DmgBonus => Data.GetCurrentDmg();
    private int Quantity => Data.GetCurrentQuantity();
    private int Cooldown => Data.GetCurrentCooldown();

    private TowerAttack TowerAttack
    {
        get
        {
            if (towerAttack == null)
                towerAttack = TowerCtrl.instance.GetComponent<TowerAttack>();
            return towerAttack;
        }
    }

    private Damager Dmg
    {
        get
        {
            if (cachedDamager == null)
            {
                cachedDamager = new Damager
                {
                    damage = TowerAttack.dmg * DmgBonus,
                    type = DamageType.NORMAL,
                    objAttack = gameObject,
                    isShockwave = true
                };
            }
            return cachedDamager;
        }
    }

    [SerializeField]
    private TowerData objData;

    private void Start()
    {
        if (!GameDatas.isUnlockUltimateWeapon(UW_ID.SHOCKWAVE))
            return;

        if (Quantity > 0)
        {
            waitOneSecond = new WaitForSeconds(2f);
            waitCooldown = new WaitForSeconds(Cooldown);
            StartCoroutine(IE_Action());
        }
    }

    private IEnumerator IE_Action()
    {
        yield return Yielders.Get(Cooldown);

        while (true)
        {
            for (int i = 0; i < Quantity; i++)
            {
                var obj = PoolCtrl.instance.Get(PoolTag.SHOCKWAVE, TowerCtrl.instance.transform.position, Quaternion.identity);
                obj.GetComponent<I_Bullet>().SetUp(Dmg, 0, 0, 0, 100, false, 0, false, false);

                EventDispatcher.PostEvent(EventID.ShockwaveBonusHealth, 0);

                yield return waitOneSecond;
            }

            yield return waitCooldown;
        }
    }
}
