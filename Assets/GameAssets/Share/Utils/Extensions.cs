using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Extensions
{

    public static int CustomRound(double value, double threshold = 0.6)
    {
        return (value % 1 >= threshold) ? (int)Math.Ceiling(value) : (int)Math.Floor(value);
    }

    public static bool EqualsLayer (this Component c, LayerMask layerMask)
    {
        return c.gameObject.CompareLayer (layerMask);
    }

    public static bool CompareLayer (this GameObject g, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << g.layer));
    }

    public static void LookAt (this Component t, Vector2 startDirection, Vector3 pos)
    {
        Vector2 dir = pos - t.transform.position;
        t.transform.rotation = Quaternion2D.FromToRotation (startDirection, dir.normalized);
    }

    public static void LookAt (this GameObject t, Vector2 startDirection, Vector3 pos)
    {
        LookAt (t.transform, startDirection, pos);
    }

    public static Vector3 Center (this GameObject t)
    {
        return t.transform.position;
    }

    public static T RandomValue<T> (this T [] array)
    {
        if (array.Length == 0)
            return default;

        return array [Random.Range (0, array.Length)];
    }

    public static T RandomValue<T> (this List<T> list)
    {
        if (list.Count == 0)
            return default;

        return list [Random.Range (0, list.Count)];
    }

    public static string SentenceCase (this string text)
    {
        string s = "";
        bool isUpper = false;
        for (int i = 0; i < text.Length; i++)
        {
            if (i == 0 || text [i - 1] == '.')
            {
                isUpper = true;
            }

            var c = text [i];
            if (isUpper && IsAlphabet (c))
            {
                s += c.ToString ().ToUpper ();
                isUpper = false;
            }
            else
            {
                s += c;
            }
        }

        bool IsAlphabet (char c)
        {
            return (c >= 97 && c <= 122) || (c >= 65 && c <= 90);
        }

        return s;
    }

    public static string ToColor (this object obj, Color c)
    {
        var colorHTML = ColorUtility.ToHtmlStringRGBA (c);
        return obj.ToColor (colorHTML);
    }

    public static string ToColor (this object obj, string colorHTML)
    {
        return string.Format ("<color=#{0}>{1}</color>", colorHTML, obj);
    }

    public static string ForceSizeText (this object obj, float size)
    {
        return string.Format ("<size={0}>{1}</size>", size, obj);
    }

    public static Sprite ToSprite (this Texture2D texture)
    {
        return Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.one / 2f);
    }

    public static string GetDisplay (this TimeSpan timeSpan)
    {
        string day = timeSpan.Days > 0 ? string.Format ("{0}{1}, ", timeSpan.Days, "d") : "";
        return string.Format ("{0}{1}:{2}:{3}", day, timeSpan.Hours.ToString ("00"), timeSpan.Minutes.ToString ("00"), timeSpan.Seconds.ToString ("00"));
    }

    public static Vector2 ParseVector2 (this string s)
    {
        var part = s.Split ("-");
        return new Vector2 (float.Parse (part [0]), float.Parse (part [1]));
    }

    public static float Round (this float n, int decem)
    {
        var a = Mathf.Pow (10, decem);
        return Mathf.Round (n * a) / a;
    }

    public static Vector2 RandomCircle (float range)
    {
        var angle = Random.Range (0, 360f) * Mathf.Deg2Rad;
        return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * range;
    }

    public static float Pow (this float n, float p)
    {
        return Mathf.Pow (n, p);
    }

    public static float Pow (this int n, float p)
    {
        return ((float)n).Pow (p);
    }

    public static Vector3 GetRandomPosition(this float range)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);

        float angleRad = angle * Mathf.Deg2Rad;

        float x = range * Mathf.Cos(angleRad);
        float y = range * Mathf.Sin(angleRad);

        Vector3 randomPosition = new Vector3(x, y, 0);

        return randomPosition;
    }

    public static bool Chance(this float chance)
    {
        return UnityEngine.Random.Range(0f, 100f) < chance;
    }

    public static string FormatNumber(this double number)
    {
        if (number >= 1000000000)
        {
            return (number / 1000000000.0).ToString("0.#") + "b";
        }
        else if (number >= 1000000)
        {
            return (number / 1000000.0).ToString("0.#") + "M";
        }
        else if (number >= 10000)
        {
            return (number / 1000.0).ToString("0.#") + "K";
        }
        else
        {
            return number.ToString();
        }
    }


    public static string ToTime(this int t)
    {
        return t.ToString("00");
    }

    public static string Display(this TimeSpan t)
    {
        if (t.Days <= 0 && t.Hours <= 0)
            return string.Format("{0}:{1}", t.Minutes.ToTime(), t.Seconds.ToTime());
        if (t.Days > 0)
            return string.Format("{0}d.{1}:{2}:{3}", t.Days, t.Hours.ToTime(), t.Minutes.ToTime(), t.Seconds.ToTime());
        return string.Format("{0}:{1}:{2}", t.Hours.ToTime(), t.Minutes.ToTime(), t.Seconds.ToTime());
    }
    public static float QuadraticEquation(this Vector3 formula, int x)
    {
        return formula.x * Mathf.Pow(x, 2) + formula.y * x + formula.z;
    }

    public static string SecondsToTime(this int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int _seconds = timeSpan.Seconds;
        if (days > 0)
            return string.Format("{0}d:{1}h", days, hours);
        if (hours > 0)
            return string.Format("{0}h:{1}m", hours, minutes);
        if (minutes > 0)
            return string.Format("{0}m:{1}s", minutes, _seconds);
        else
            return string.Format("{0}s", _seconds);

    }

    public static Vector2 ConvertWorldPointToCanvas(Canvas canvas, Vector3 worldPoint, Camera cam)
    {
        if (cam == null) cam = Camera.main;
        var viewPortPos = cam.WorldToViewportPoint(worldPoint);
        var canvasRect = canvas.GetComponent<RectTransform>();
        var worldObject_to_canvasPos = new Vector2(
            (viewPortPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)
            , (viewPortPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
        );
        return worldObject_to_canvasPos;
    }

    public static bool HasInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    public static string FormatEnumName(Enum value)
    {
        return value.ToString().Replace("_", " ");
    }

    public static bool CheckArenaTime()
    {
        DateTime now = DateTime.Now;
        DayOfWeek day = now.DayOfWeek;
        int hour = now.Hour;

        DebugCustom.LogColor("hour " + hour);

        if ((day == DayOfWeek.Monday || day == DayOfWeek.Thursday) && hour >= 19)
        {
            return true;
        }

        if ((day == DayOfWeek.Tuesday || day == DayOfWeek.Friday) && hour < 19)
        {
            return true;
        }

        return false;
    }

    public static string FormatCompactNumber(double value)
    {
        string suffix = "";
        double shortenedValue = value;

        if (value >= 1_000_000_000_000)
        {
            shortenedValue = value / 1_000_000_000_000;
            suffix = "T";
        }
        else if (value >= 1_000_000_000)
        {
            shortenedValue = value / 1_000_000_000;
            suffix = "B";
        }
        else if (value >= 1_000_000)
        {
            shortenedValue = value / 1_000_000;
            suffix = "M";
        }
        else if (value >= 1_000)
        {
            shortenedValue = value / 1_000;
            suffix = "K";
        }

        return shortenedValue.ToString("0.##", CultureInfo.InvariantCulture) + suffix;
    }


}
