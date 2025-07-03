using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : BulletCollided
{

    //int currentTargetBounceLeft;
    float powerBounce => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bounce_shot_damage);
    private float rangeBounce => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.bounce_shot_range) + 5f;
    Object_Move objMove;
    [SerializeField] LayerMask enemiesLayer;
    protected override void Awake()
    {
        base.Awake();
        objMove = GetComponent<Object_Move>();
    }
    public override void Bounce()
    {
        base.Bounce();

        //tìm target gần nhất và move tới đó. nếu éo có thì thôi
        //if (currentTargetBounceLeft <= 0) objDestroy.Destroy(null);
        //currentTargetBounceLeft -= 1;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, rangeBounce, enemiesLayer);


        if (collider2Ds.Length <= 0)
            objDestroy.Destroy(null);

        //nếu không nằm trong list của bullet collided thì bắn ko thì destroy
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            var col = collider2Ds[i];

                var dir = (col.transform.position - transform.position).normalized;
                countBounce++;
                objMove.SetDirectionMove(dir);
                return;
                /*if (!cols2dCollided.Contains(col))
                {
                    var dir = (col.transform.position - transform.position).normalized;
                    objMove.SetDirectionMove(dir);
                    return;
                }*/
            }
        objDestroy.Destroy(null);

    }
/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeBounce);
    }*/
}
