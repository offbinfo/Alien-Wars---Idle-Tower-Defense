using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUICellView : GameMonoBehaviour
{

    protected BaseUICellData _data;
    [SerializeField]
    protected string cellIdentifier;

    public int DataIndex { get; set; }
    public int RepeatIndex { get; set; }
    public Transform ContainerTransform { get; set; }

    public virtual string GetCellTypeIdentifier()
    {
        return "";
    }

    public float GetDrawPriority()
    {
        return 0;
    }

    public virtual void SetData(BaseUICellData data)
    {
        _data = data;
    }
}
