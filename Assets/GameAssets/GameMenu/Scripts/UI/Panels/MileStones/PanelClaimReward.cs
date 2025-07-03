using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelClaimReward : Singleton<PanelClaimReward>
{
    [SerializeField]
    private Animator anim;
    private const string nameOpenAnim = "Open";
    private const string nameCloseAnim = "Close";

    private void OnEnable()
    {
        anim.Play(nameOpenAnim);
        StartCoroutine(DelayClosePanel());
    }

    private IEnumerator DelayClosePanel()
    {
        yield return Yielders.Get(1f);
        anim.Play(nameCloseAnim);
        yield return Yielders.Get(0.5f);
        gameObject.SetActive(false);
    }
}
