using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Chase : GameMonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;

    private Vector3 targetPos;

    public Vector3 TargetPos
    {
        get { return targetPos; }
        set
        {
            targetPos = value;
        }
    }

    protected virtual void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
        var dir = TargetPos - transform.position;

        var angle = Vector2.SignedAngle(Vector2.up, dir);
        var rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

}
