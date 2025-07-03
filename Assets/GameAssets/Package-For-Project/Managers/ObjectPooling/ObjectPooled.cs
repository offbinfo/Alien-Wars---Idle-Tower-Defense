using System.Collections.Generic;
using UnityEngine;

public class ObjectPooled : MonoBehaviour
{
    public bool poolObjectOnDisable = true;

    List<GameObject> pool;
    public void SetPool (List<GameObject> pool)
    {
        this.pool = pool;
    }

    public void PoolObject ()
    {
        if (!pool.Contains (gameObject))
            pool.Add (gameObject);
    }

    private void OnDisable ()
    {
        if (poolObjectOnDisable)
        {
            PoolObject ();
            gameObject.SetActive (false);
        }
    }
}
