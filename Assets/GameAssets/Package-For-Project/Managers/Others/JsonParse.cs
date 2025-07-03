using Newtonsoft.Json;
using System;

public class JsonParse
{
    public static T FromJson<T> (string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T> (json);
        }
        catch
        {
            return default;
        }
    }

    public static object FromJson (string json, Type type)
    {
        try
        {
            return JsonConvert.DeserializeObject (json, type);
        }
        catch
        {
            return default;
        }
    }

    public static string ToJson (object obj)
    {
        return JsonConvert.SerializeObject (obj);
    }
}
