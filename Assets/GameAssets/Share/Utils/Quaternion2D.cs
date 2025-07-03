using UnityEngine;

public class Quaternion2D : MonoBehaviour
{
    public static Quaternion FromToRotation (Vector2 fromDirection, Vector2 toDirection)
    {
        var angle = Vector2.SignedAngle (fromDirection, toDirection);
        return Quaternion.Euler (0, 0, angle);
    }
}
