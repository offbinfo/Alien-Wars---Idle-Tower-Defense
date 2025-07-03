using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolingManager
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init ()
    {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private static void SceneManager_sceneUnloaded (Scene arg0)
    {
        dic.Clear ();
    }

    static Dictionary<GameObject, List<GameObject>> dic = new Dictionary<GameObject, List<GameObject>> ();

    public static GameObject GetObjectPooled (GameObject source)
    {
        return GetObjectPooled (source, Vector3.zero, Quaternion.identity);
    }

    public static GameObject GetObjectPooled (GameObject source, Vector3 position)
    {
        return GetObjectPooled (source, position, Quaternion.identity);
    }

    public static GameObject GetObjectPooled (GameObject source, Vector3 position, float scale)
    {
        GameObject obj = GetObjectPooled (source, position);
        obj.transform.localScale = Vector3.one * scale;

        return obj;
    }

    public static GameObject GetObjectPooled (GameObject source, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject (source);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = Vector3.one;

        return obj;
    }

    public static GameObject GetObjectPooled (GameObject source, Transform parent)
    {
        var obj = GetObjectPooled (source, parent.position);
        obj.transform.SetParent (parent);
        obj.transform.localScale = Vector3.one;

        return obj;
    }

    static GameObject GetObject (GameObject source)
    {
        if (!dic.ContainsKey (source))
            dic.Add (source, new List<GameObject> ());

        var pool = dic [source];

        return PoolingObject.GetFromPool (pool, source);
    }
}
