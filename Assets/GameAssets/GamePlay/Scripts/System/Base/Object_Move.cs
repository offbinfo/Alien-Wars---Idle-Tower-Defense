using UnityEngine;

public interface I_MoveData
{
    float spdMove { get; set; }
}

public class Object_Move : GameMonoBehaviour
{
    I_MoveData moveData;
    public virtual float speedMove => moveData.spdMove;
    [SerializeField] Transform graphic;
    protected Vector3 directionMove;
    public bool isNoMove = false;

    public virtual void Awake()
    {
        moveData = GetComponent<I_MoveData>();
    }

    public virtual void SetDirectionMove(Vector3 direction)
    {
        graphic.right = -direction;
        directionMove = direction;
    }
}


