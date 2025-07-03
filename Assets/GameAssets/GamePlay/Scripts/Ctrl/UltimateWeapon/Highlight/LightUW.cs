using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICastedLight
{
    void CastedLight(float dmgScale);
    void OutLight();
}

public class LightUW : GameMonoBehaviour
{

    private SO_UW_Passive data;
    private SO_UW_Passive Data
    {
        get
        {
            if (data == null)
                data = Cfg.UWCtrl.UWeaponManager.GetDataById<SO_UW_Passive>(UW_ID.HIGHLIGHT);
            return data;
        }
    }

    private float angle => Data.GetCurrentAngle();
    private float dmgScale => Data.GetCurrentDmgScale();

/*    private Dictionary<Collider2D, EnemyTakeDamage> lightCache = new Dictionary<Collider2D, EnemyTakeDamage>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(GameTags.TAG_ENEMIES))
            return;

        if (!collision.TryGetComponent(out EnemyTakeDamage enemy) || enemy.Pool == null)
            return;

        if (!lightCache.ContainsKey(collision))
        {
            lightCache[collision] = enemy;
            enemy.Pool.AddEventReturn(() => RemoveFromCache(collision)); 
        }

        enemy.CastedLight(dmgScale);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (lightCache.TryGetValue(collision, out EnemyTakeDamage enemy))
        {
            enemy.OutLight();
        }
    }

    private void RemoveFromCache(Collider2D collision)
    {
        lightCache.Remove(collision);
    }*/

}
