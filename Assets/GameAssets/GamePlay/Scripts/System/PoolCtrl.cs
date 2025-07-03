using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCtrl : Singleton<PoolCtrl>
{
    public List<PoolObject> poolObjects;
    Dictionary<PoolTag, Queue<Object_Pool>> dicPool = new();
    private Object_Pool temp;
    public List<Object_Pool> allBulletsNotInPool = new();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < poolObjects.Count; i++)
        {
            Queue<Object_Pool> temp = new Queue<Object_Pool>();
            for (int n = 0; n < poolObjects[i].amountInit; n++)
            {
                Object_Pool b = Instantiate(poolObjects[i]._object, transform);
                b.gameObject.SetActive(false);
                temp.Enqueue(b);
            }
            dicPool.Add(poolObjects[i]._tag, temp);
        }
    }

/*    private void Init()
    {
        StartCoroutine(PreloadObjects());
    }

    private IEnumerator PreloadObjects()
    {
        for (int i = 0; i < poolObjects.Count; i++)
        {
            Queue<Object_Pool> temp = new Queue<Object_Pool>();
            dicPool.Add(poolObjects[i]._tag, temp);

            for (int n = 0; n < poolObjects[i].amountInit; n++)
            {
                Object_Pool b = Instantiate(poolObjects[i]._object, transform);
                b.gameObject.SetActive(false);
                temp.Enqueue(b);

                if (n % 5 == 0)
                    yield return null;
            }
        }
    }*/

    /*public Object_Pool Get(PoolTag tag, Vector2 pos, Quaternion rot, object obj = null)
    {
        if (dicPool[tag].Count <= 0)
        {
            for (int i = 0; i < poolObjects.Count; i++)
            {
                if (poolObjects[i]._tag == tag)
                {
                    temp = Instantiate(poolObjects[i]._object, transform);
                    dicPool[tag].Enqueue(temp);
                    temp.gameObject.SetActive(false);
                    temp = null;
                    break;
                }
            }
        }
        temp = null;
        temp = dicPool[tag].Dequeue();

        temp.transform.position = pos;
        temp.transform.rotation = rot;
        temp.transform.localScale = Vector3.one;
        //temp.OnInit();
        temp.gameObject.SetActive(true);
        temp.OnInit(obj);
        allBulletsNotInPool.Add(temp);
        return temp;
    }*/
    public Object_Pool Get(PoolTag tag, Vector2 pos, Quaternion rot, object obj = null)
    {
        if (dicPool[tag].Count <= 0)
        {
            StartCoroutine(ExpandPool(tag, 5));
        }

        temp = dicPool[tag].Dequeue();
        temp.transform.position = pos;
        temp.transform.rotation = rot;
        temp.transform.localScale = Vector3.one;
        temp.gameObject.SetActive(true);
        temp.OnInit(obj);
        allBulletsNotInPool.Add(temp);
        return temp;
    }

    private IEnumerator ExpandPool(PoolTag tag, int amount)
    {
        for (int i = 0; i < poolObjects.Count; i++)
        {
            if (poolObjects[i]._tag == tag)
            {
                for (int j = 0; j < amount; j++)
                {
                    Object_Pool b = Instantiate(poolObjects[i]._object, transform);
                    b.gameObject.SetActive(false);
                    dicPool[tag].Enqueue(b);

                    if (j % 2 == 0)
                        yield return new WaitForEndOfFrame();
                }
                break;
            }
        }
    }

    public void Return(Object_Pool b, System.Action OnReturn = null)
    {
        if (!dicPool[b._tag].Contains(b)) dicPool[b._tag].Enqueue(b);
        allBulletsNotInPool.Remove(b);
        //b.SetAnimNull();
        b.OnReturn();
        b.gameObject.SetActive(false);
    }
    public void ReturnAll()
    {
        allBulletsNotInPool.ForEach(b =>
        {
            if (!dicPool[b._tag].Contains(b))
            {
                dicPool[b._tag].Enqueue(b);
            }

            b.gameObject.SetActive(false);
        });
        allBulletsNotInPool.Clear();
    }

}

[System.Serializable]
public class PoolObject
{
    public PoolTag _tag;
    public Object_Pool _object;
    public int amountInit;
}

