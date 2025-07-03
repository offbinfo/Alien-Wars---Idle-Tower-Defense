using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle_Chase : Object_Chase
{

    [SerializeField] private LayerMask layerTarget;
    private TowerData towerData;
    private float radius = 1f;

    private void OnEnable()
    {
        towerData = TowerCtrl.instance.GetComponent<TowerData>();
        if (towerData != null)
            StartCoroutine(IE_SignTarget());
    }

    private IEnumerator IE_SignTarget()
    {
        var waitTime = new WaitForSecondsRealtime(Random.Range(0.5f, 1f));

        while (true)
        {
            Collider2D col2d = Physics2D.OverlapCircle(transform.position, radius, layerTarget);
            TargetPos = col2d ? col2d.transform.position : towerData.transform.position + (towerData.attackRange + 0.5f).GetRandomPosition();
            yield return waitTime;
        }
    }

}
