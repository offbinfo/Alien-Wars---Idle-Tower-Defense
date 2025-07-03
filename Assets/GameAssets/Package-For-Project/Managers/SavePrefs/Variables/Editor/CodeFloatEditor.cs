using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof(Float))]
public class CodeFloatEditor : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        var value = property.FindPropertyRelative ("valueSave");
        var stringValue = value.stringValue;
        var floatValue = EncodingData.Decode (stringValue);
        floatValue = EditorGUI.FloatField (position, label, floatValue);
        value.stringValue = EncodingData.Encode (floatValue);
    }
}
