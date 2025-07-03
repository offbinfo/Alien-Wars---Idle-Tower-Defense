using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof(Int))]
public class CodeIntEditor : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        var value = property.FindPropertyRelative ("valueSave");
        var stringValue = value.stringValue;
        var intValue = (int)EncodingData.Decode (stringValue);
        intValue = EditorGUI.IntField (position, label, intValue);
        value.stringValue = EncodingData.Encode (intValue);
    }
}
