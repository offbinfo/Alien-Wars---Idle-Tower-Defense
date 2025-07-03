using UnityEngine;

[System.Serializable]
public struct Float
{
    [SerializeField, HideInInspector] string valueSave;
    public float value
    {
        get => EncodingData.Decode (valueSave);
        set => valueSave = EncodingData.Encode (value);
    }

    public Float (float value)
    {
        valueSave = null;
        this.value = value;
    }

    #region opera self
    public static Float operator + (Float a, Float b)
    {
        return new Float (a.value + b.value);
    }

    public static Float operator - (Float a, Float b)
    {
        return new Float (a.value - b.value);
    }

    public static Float operator * (Float a, Float b)
    {
        return new Float (a.value * b.value);
    }

    public static Float operator / (Float a, Float b)
    {
        return new Float (a.value / b.value);
    }

    public static bool operator == (Float a, Float b)
    {
        return a.value == b.value;
    }

    public static bool operator != (Float a, Float b)
    {
        return a.value != b.value;
    }

    public static Float operator ++ (Float a)
    {
        return new Float (a.value + 1);
    }

    public static Float operator -- (Float a)
    {
        return new Float (a.value - 1);
    }

    public static implicit operator float (Float a)
    {
        return a.value;
    }

    public static implicit operator Float (float v)
    {
        return new Float (v);
    }

    public override bool Equals (object obj)
    {
        return value.Equals (((Float)obj).value);
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
