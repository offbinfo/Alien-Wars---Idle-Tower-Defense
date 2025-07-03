using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ProjectTools
{
    [CustomPropertyDrawer (typeof (UnityAnimationAttribute))]
    public class UnityAnimationAttributeDrawer : PropertyDrawer
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
                var ac = anim.runtimeAnimatorController as AnimatorController;
                var states = ac.layers.SelectMany (x => x.stateMachine.states).Select (x => x.state).ToArray ();
                List<GUIContent> animationNames = new List<GUIContent> ();
                animationNames.Add (new GUIContent ("None ...", UnityAnimateEditorUtility.Icons.animation));

                int index = 0;
                for (int i = 0; i < states.Length; i++)
                {
                    var name = states [i].name;
                    animationNames.Add (new GUIContent (name, UnityAnimateEditorUtility.Icons.animation));

                    if (property.stringValue == name)
                        index = i + 1;
                }

                var textIndex = EditorGUI.Popup (position, label, index, animationNames.ToArray ());
                string text = textIndex == 0 ? "" : animationNames [textIndex].text;

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