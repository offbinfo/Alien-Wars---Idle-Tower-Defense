using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectTools
{
    public static class Extension
    {
#if UNITY_EDITOR
        public static void ImportData (this Object o, List<(string, string)> values)
        {
            var type = o.GetType ();

            foreach (var value in values)
            {
                // parse
                var key = value.Item1;
                if (string.IsNullOrWhiteSpace (key))
                    continue;

                var s = value.Item2;
                var field = type.GetField (key);

                try
                {

                    field.SetValue (o, s.Parse (field.FieldType));
                }
                catch
                {
                    Debug.Log (key);
                }
            }

            UnityEditor.EditorUtility.SetDirty (o);
        }
#endif

        public static object Parse (this string s, Type type)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                s = s.Replace (",", ".");

            if (type.IsPrimitive)
            {
                return Convert.ChangeType (s, type);
            }
            else if (type.IsArray || type.IsGenericType)
            {
                var memberType = type.IsArray ? type.GetElementType () : type.GenericTypeArguments [0];

                s = s.Replace ("[", "").Replace ("]", "");
                var parts = s.Split (";");

                var json = JsonParse.ToJson (parts.Select (x => x.Parse (memberType)));
                return JsonParse.FromJson (json, type);
            }
            else if (type.IsEnum)
            {
                return Enum.Parse (type, s);
            }

            return s;
        }
    }
}