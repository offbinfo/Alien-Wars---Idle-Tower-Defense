using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    private TowerData tower_data;
    private void Awake()
    {
        tower_data = GetComponentInParent<TowerData>();
    }

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.UpgraderTowerInGame, OnAtkRangeChange);
        OnAtkRangeChange(UpgraderID.attack_range);
    }

    private void OnAtkRangeChange(object o)
    {
        if ((UpgraderID)o != UpgraderID.attack_range) return;

        float range = tower_data.attackRange;
        transform.localScale = Vector3.one * range;
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.UpgraderTowerInGame, OnAtkRangeChange);
    }
}
