using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PurchaserSettings : ScriptableObject
{
    public bool activate = true;
    public string title;

    [Header ("NonConsumable")]
    public string [] allProductID_NoneConsume = null;
    public string [] AllProductID_NoneConsume
    {
        get
        {
            var ids = new List<string> ();
            foreach (var item in allProductID_NoneConsume)
            {
                ids.Add (title + "." + item);
            }

            return ids.ToArray ();
        }
    }

    [Header ("Consumable")]
    public string [] allProductID = null;
    public string [] AllProductID
    {
        get
        {
            var ids = new List<string> ();
            foreach (var item in allProductID)
            {
                ids.Add (title + "." + item);
            }

            return ids.ToArray ();
        }
    }

#if UNITY_EDITOR
    [ContextMenu ("Copy")]
    private void Export ()
    {
        var text = "";
        foreach (var item in AllProductID)
        {
            text += string.Format ("\n{0}", item);
        }

        text += string.Format ("\n");

        foreach (var item in AllProductID_NoneConsume)
        {
            text += string.Format ("\n{0}", item);
        }

        EditorGUIUtility.systemCopyBuffer = text;
    }
#endif
}
