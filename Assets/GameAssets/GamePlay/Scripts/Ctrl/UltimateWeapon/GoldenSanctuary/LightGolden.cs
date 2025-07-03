using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICastedLightGolden
{
    void CastedLight(float bonus);
    void OutLight();
}

public class LightGolden : GameMonoBehaviour
{
    private float bonus = 2;
    [SerializeField]
    private CircleCollider2D circleCollider;

    private void Start()
    {
        Init(null);

        EventDispatcher.AddEvent(EventID.OnUpgradeSubjectSuccessLab, Init);
    }

    private void Init(object o)
    {
        bonus += Cfg.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.GOLDEN_SANCTUARY_BONUS).GetCurrentProperty();
    }

    public void SetUp(float radius)
    {
        circleCollider.radius = radius;
        transform.localScale = new Vector3(radius,
             radius, 1);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            collision.GetComponent<ICastedLightGolden>().CastedLight(bonus);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf) return;
        if (collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            collision.GetComponent<ICastedLightGolden>().OutLight();
        }
    }
}
