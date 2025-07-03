using UnityEngine;

public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : Object
{
    public static T instance;

    protected virtual void Awake ()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad (this);
        }
        else
        {
            Destroy (gameObject);
        }
    }
}
