using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PurchaseIDAttribute : PropertyAttribute
{
    public PurchaseIDAttribute ()
    {

    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (PurchaseIDAttribute))]
public class PurchaseIdDrawer : PropertyDrawer
{
    PurchaserSettings purchaserSettings = null;
    List<string> displayNames = new List<string> ();

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        if (!purchaserSettings)
        {
            purchaserSettings = Resources.Load<PurchaserSettings> ("PurchaserSettings");

            displayNames.Clear ();
            if (purchaserSettings)
            {
                displayNames.Add ("none");

                for (int i = 0; i < purchaserSettings.allProductID.Length; i++)
                {
                    displayNames.Add (purchaserSettings.allProductID [i]);
                }

                for (int i = 0; i < purchaserSettings.allProductID_NoneConsume.Length; i++)
                {
                    displayNames.Add (purchaserSettings.allProductID_NoneConsume [i]);
                }
            }
        }

        if (purchaserSettings)
        {
            int index = displayNames.IndexOf (property.stringValue);
            index = Mathf.Max (0, index);

            index = EditorGUI.Popup (position, property.displayName, index, displayNames.ToArray ());

            property.stringValue = index == 0 ? "" : displayNames [index];
        }
        else
        {
            EditorGUI.LabelField (position, property.displayName, "NOT FOUND PURCHASER SETTINGS");
        }
    }
}
#endif
