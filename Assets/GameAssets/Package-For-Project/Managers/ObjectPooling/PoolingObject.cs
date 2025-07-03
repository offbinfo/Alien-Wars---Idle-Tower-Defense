using System.Collections.Generic;
using UnityEngine;

public class PoolingObject
{
    public static GameObject GetFromPool (List<GameObject> pool, GameObject source)
    {
        GameObject obj = null;
        if (pool.Count > 0)
        {
            obj = pool [0];
            pool.RemoveAt (0);
        }
        else
        {
            obj = Object.Instantiate (source);

            ObjectPooled objectPooled = obj.GetComponent<ObjectPooled> ();
            if (!objectPooled)
                objectPooled = obj.AddComponent<ObjectPooled> ();
            objectPooled.SetPool (pool);
        }
        obj.transform.localScale = Vector3.one;
        obj.SetActive (true);
        return obj;
    }

    public static T LookUpFromPool<T> (List<T> pool, T source, Transform parent) where T : MonoBehaviour
    {
        var t = pool.Find (x => !x.gameObject.activeSelf);

        if (!t)
        {
            t = Object.Instantiate (source, parent);
            pool.Add (t);
        }

        t.gameObject.SetActive (true);
        return t;
    }
}
