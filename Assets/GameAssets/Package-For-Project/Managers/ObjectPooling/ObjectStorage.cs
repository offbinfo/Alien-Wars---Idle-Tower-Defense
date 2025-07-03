using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : Singleton<ObjectStorage>
{
    [SerializeField] List<GameObject> objects = new List<GameObject> ();

    Dictionary<string, GameObject> dic = new Dictionary<string, GameObject> ();

    public GameObject GetObject (string name)
    {
        return dic [name];
    }

    protected override void Awake ()
    {
        base.Awake ();

        foreach (var item in objects)
        {
            dic [item.name] = item;
        }
    }
}
