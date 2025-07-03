using System;
using System.Text;
using UnityEngine;

public class EncodingData
{
    static int hashCode => Application.companyName.GetHashCode ();

    public static string Encode (string value)
    {
        byte [] bytes = Encoding.ASCII.GetBytes (value);
        return Convert.ToBase64String (bytes);
    }

    public static string Encode (float value)
    {
        byte [] bytes = BitConverter.GetBytes (value);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes [i] = (byte)(bytes [i] + i + hashCode);
        }
        return Convert.ToBase64String (bytes);
    }

    public static float Decode (string text)
    {
        if (string.IsNullOrEmpty (text))
            return 0;

        byte [] encodeBytes = Convert.FromBase64String (text);
        for (int i = 0; i < encodeBytes.Length; i++)
        {
            encodeBytes [i] = (byte)(encodeBytes [i] - i - hashCode);
        }

        return BitConverter.ToSingle (encodeBytes, 0);
    }
}
