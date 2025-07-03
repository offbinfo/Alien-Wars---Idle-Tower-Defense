using System;
using UnityEngine;

public class SavePrefs
{
    public static void SetInt (string key, int value)
    {
        key = EncodingData.Encode (key);
        string encode = EncodingData.Encode (value);
        SetString (key, encode);
    }

    public static void SetFloat (string key, float value)
    {
        key = EncodingData.Encode (key);
        string encode = EncodingData.Encode (value);
        SetString (key, encode);
    }

    public static int GetInt (string key, int valueDefault)
    {
        valueDefault = PlayerPrefs.GetInt (key, valueDefault);
        key = EncodingData.Encode (key);
        string encodeDefault = EncodingData.Encode (valueDefault);
        string encode = GetString (key, encodeDefault);
        float value = EncodingData.Decode (encode);
        return (int)value;
    }

    public static float GetFloat (string key, float valueDefault)
    {
        valueDefault = PlayerPrefs.GetFloat (key, valueDefault);
        key = EncodingData.Encode (key);
        string encodeDefault = EncodingData.Encode (valueDefault);
        string encode = GetString (key, encodeDefault);
        float value = EncodingData.Decode (encode);
        return value;
    }

    public static void SetString (string key, string value)
    {
        PlayerPrefs.SetString (key, value);
    }

    public static string GetString (string key, string valueDefault)
    {
        return PlayerPrefs.GetString (key, valueDefault);
    }

    public static void DeleteKey (string key)
    {
        PlayerPrefs.DeleteKey (EncodingData.Encode (key));
    }

    public static void DeleteKeyString (string key)
    {
        PlayerPrefs.DeleteKey (key);
    }

    public static void DeleteAll ()
    {
        PlayerPrefs.DeleteAll ();
    }

    internal static int GetInt (object getKeyGold, int v)
    {
        throw new NotImplementedException ();
    }
}
