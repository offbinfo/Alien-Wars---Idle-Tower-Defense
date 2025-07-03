using UnityEngine;

public class Singleton<T> : GameMonoBehaviour where T : Object
{
    static T i;
    public static T instance
    {
        get
        {
            if (!i)
                i = FindObjectOfType<T>(true);
            return i;
        }
    }

    protected virtual void Awake()
    {
        i = this as T;
    }
}
