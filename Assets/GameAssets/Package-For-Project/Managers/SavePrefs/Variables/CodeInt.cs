using UnityEngine;

[System.Serializable]
public struct Int
{
    [SerializeField, HideInInspector] string valueSave;
    public int value
    {
        get => (int)EncodingData.Decode (valueSave);
        set => valueSave = EncodingData.Encode (value);
    }

    public Int (int value)
    {
        valueSave = null;
        this.value = value;
    }

    #region opera self
    public static Int operator + (Int a, Int b)
    {
        return new Int (a.value + b.value);
    }

    public static Int operator - (Int a, Int b)
    {
        return new Int (a.value - b.value);
    }

    public static Int operator * (Int a, Int b)
    {
        return new Int (a.value * b.value);
    }

    public static Int operator / (Int a, Int b)
    {
        return new Int (a.value / b.value);
    }

    public static bool operator == (Int a, Int b)
    {
        return a.value == b.value;
    }

    public static bool operator != (Int a, Int b)
    {
        return a.value != b.value;
    }

    public static Int operator ++ (Int a)
    {
        return new Int (a.value + 1);
    }

    public static Int operator -- (Int a)
    {
        return new Int (a.value - 1);
    }

    public static implicit operator int (Int a)
    {
        return a.value;
    }

    public static implicit operator Int (int v)
    {
        return new Int (v);
    }

    public override bool Equals (object obj)
    {
        return value.Equals (((Int)obj).value);
    }

    public override int GetHashCode ()
    {
        return value.GetHashCode ();
    }

    public override string ToString ()
    {
        return value.ToString ();
    }
    #endregion
}
