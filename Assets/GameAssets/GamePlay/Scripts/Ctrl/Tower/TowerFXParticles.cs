using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFXParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem[] fxUpgrade;
    [SerializeField] ParticleSystem fxAttack;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.UpgraderTowerInGame, PlayFxUpgrade);
    }

    private void PlayFxUpgrade(object obj)
    {
        foreach (var item in fxUpgrade)
        {
            item.Play();
        }
    }

    public void PlayFxAttack()
    {
        fxAttack.Play();
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.UpgraderTowerInGame, PlayFxUpgrade);
    }
}
