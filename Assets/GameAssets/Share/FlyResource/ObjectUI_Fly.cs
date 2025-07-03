using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI_Fly : MonoBehaviour
{

    RectTransform rectTransform;
    [SerializeField] AnimationCurve EaseFly;
    [SerializeField] GameObject obj_Gold;
    [SerializeField] GameObject obj_Gem;
    [SerializeField] GameObject obj_PowerStone;
    [SerializeField] GameObject obj_Badges;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnInit()
    {
        gameObject.SetActive(false);
    }
    public float Fly(Vector3 pos, Vector3 target, Canvas canvas, CurrencyType currency)
    {
        obj_Gold.SetActive(currency == CurrencyType.GOLD);
        obj_Gem.SetActive(currency == CurrencyType.GEM);
        obj_PowerStone.SetActive(currency == CurrencyType.POWER_STONE);
        obj_Badges.SetActive(currency == CurrencyType.BADGES);

        float rand = 0f;
        if(canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
            rand = 150f;
        }
        else
        {
            rand = 10f;
        }

        transform.position = pos;
        var posRandom = pos + (Vector3)Random.insideUnitCircle * rand;
        Vector3[] vec3_list = {
            pos,
            posRandom,
            target
        };
        var distance = Vector3.Distance(pos, posRandom) + Vector3.Distance(posRandom, target);
        var time = distance / 900f;
        gameObject.SetActive(true);
        rectTransform.DOPath(vec3_list, Random.Range(1, 1 * 1.25f) /*Random.Range(time, time * 1.25f)*/, PathType.CatmullRom, PathMode.TopDown2D).SetEase(EaseFly).OnComplete(() => {
            ObjectUI_Fly_Manager.instance.Return(this);
        }).SetUpdate(true);
        return time;
    }
}
