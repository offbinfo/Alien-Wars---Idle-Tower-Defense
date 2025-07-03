using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ProjectTools
{
    [CreateAssetMenu (menuName = "All Datas/ScriptableObject")]
    public class ScriptableObjectCustom : ScriptableObject
    {
#if UNITY_EDITOR
        [ContextMenu ("Copy Value")]
        private void CopyValue ()
        {
            var json = JsonUtility.ToJson (this);
            GUIUtility.systemCopyBuffer = json;
        }

        [ContextMenu ("Paste Value")]
        private void PasteValue ()
        {
            var json = GUIUtility.systemCopyBuffer;
            JsonUtility.FromJsonOverwrite (json, this);
        }

        [ContextMenu ("Copy Properties Name")]
        private void CopyPropertiesName ()
        {
            var type = GetType ();
            var fields = type.GetFields ().Reverse ();
            var key = "Properties\t";
            foreach (var field in fields)
            {
                key += field.Name + "\t";
            }

            GUIUtility.systemCopyBuffer = key;
        }

        [ContextMenu ("Copy Properties")]
        private void CopyProperties ()
        {
            var type = GetType ();
            var fields = type.GetFields ().Reverse ();
            var value = name + "\t";
            foreach (var field in fields)
            {
                var v = field.GetValue (this);

                if (v is IList)
                {
                    var array = v as IList;
                    var s = "[";
                    foreach (var item in array)
                    {
                        s += string.Format ("{0};", item);
                    }

                    s = s.Remove (s.Length - 1, 1);
                    s += "]";

                    value += s + "\t";
                }
                else
                {
                    value += v + "\t";
                }
            }

            GUIUtility.systemCopyBuffer = value;
        }
#endif
    }
}