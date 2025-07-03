
using UnityEngine;

public class BlockInputUI : Singleton<BlockInputUI>
{
    [SerializeField] GameObject obj_blockInput;
    public void blockInput(bool isActive)
    {
        obj_blockInput.SetActive(isActive);
    }
}
