using System.Collections.Generic;
using UnityEngine;

public class Popup_UIs : MonoBehaviour
{
    static Popup_UIs Instance;
    Dictionary<string, GameObject> dicPopups = new Dictionary<string, GameObject> ();

    public static T OpenPopup<T> (string popupName)
    {
        if (Instance.dicPopups.ContainsKey (popupName))
        {
            var popup = Instance.dicPopups [popupName];
            popup.SetActive (true);
            return popup.GetComponent<T> ();
        }

        return default;
    }

    public static bool IsPopupOpening (string popupName)
    {
        foreach (var item in Instance.dicPopups)
        {
            if (item.Key == popupName && item.Value.activeSelf)
                return true;
        }

        return false;
    }

    public static bool IsPopupOpening ()
    {
        foreach (var item in Instance.dicPopups)
        {
            if (item.Value.activeSelf)
                return true;
        }

        return false;
    }

    private void Awake ()
    {
        Instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            var popup = transform.GetChild (i).gameObject;
            dicPopups.Add (popup.name, popup);
        }
    }

    private void Start ()
    {
        foreach (var item in dicPopups)
        {
            PushNoticeTo (item.Value);
        }
    }

    private void PushNoticeTo (GameObject target)
    {
        target.GetComponent<IPopup> ()?.InitPopup ();
        for (int i = 0; i < target.transform.childCount; i++)
        {
            PushNoticeTo (target.transform.GetChild (i).gameObject);
        }
    }
}
