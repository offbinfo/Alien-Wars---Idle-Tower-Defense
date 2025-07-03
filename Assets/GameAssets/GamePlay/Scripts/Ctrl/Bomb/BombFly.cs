using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface I_Bomb_Fly
{
    void Fly(Vector3 target);
}


public class BombFly : MonoBehaviour, I_Bomb_Fly
{
    public void Fly(Vector3 target)
    {
        var timeFly = Vector3.Distance(target, transform.position) / 2f;
        transform.DOMove(target, timeFly).SetEase(Ease.OutQuint);
    }
}
