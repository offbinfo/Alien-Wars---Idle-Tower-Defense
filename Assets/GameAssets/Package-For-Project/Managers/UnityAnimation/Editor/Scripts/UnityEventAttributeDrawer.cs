using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectTools
{
    [CustomPropertyDrawer (typeof (UnityEventAttribute))]
    public class UnityEventAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField (position, "ERROR:", "May only apply to type string");
                return;
            }

            var targetObject = property.serializedObject.targetObject as Component;

            Animator anim = targetObject.GetComponentInChildren<Animator> ();
            if (anim && anim.runtimeAnimatorController)
            {
                var infos = anim.runtimeAnimatorController.animationClips;

                List<GUIContent> eventNames = new List<GUIContent> ();

                eventNames.Add (new GUIContent ("None ...", UnityAnimateEditorUtility.Icons.userEvent));

                for (int i = 0; i < infos.Length; i++)
                {
                    var events = infos [i].events;

                    for (int j = 0; j < events.Length; j++)
                    {
                        string eventName = events [j].stringParameter;
                        eventNames.Add (new GUIContent (eventName, UnityAnimateEditorUtility.Icons.userEvent));
                    }
                }

                int index = eventNames.FindIndex (x => x.text == property.stringValue);
                index = Mathf.Clamp (index, 0, eventNames.Count - 1);

                var textIndex = EditorGUI.Popup (position, label, index, eventNames.ToArray ());
                string text = textIndex == 0 ? "" : eventNames [textIndex].text;

                if (GUI.changed)
                {
                    Undo.RecordObject (targetObject, "Change animation name");
                    property.stringValue = text;
                }
            }
            else
            {
                EditorGUI.LabelField (position, "ERROR:", "Missing animator");
            }
        }
    }
}