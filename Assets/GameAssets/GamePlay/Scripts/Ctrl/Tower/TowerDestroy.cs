using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDestroy : Object_Destroy
{
    private TowerRevival towerRevival;

    private void Awake()
    {
        towerRevival = GetComponent<TowerRevival>();
    }

    public override void Destroy(Damager d)
    {
        gameObject.SetActive(false);
        if (GameDatas.IsTut_PlayDemo)
        {
            GameDatas.IsEndTutorial = true;
        }
        if (towerRevival.revival > 0)
            Gm.OnLoseGame();
        else
            if (GameDatas.ReviveTowerCount > 0) Gui.OpenBoard(UiPanelType.PopupRevive);
        else
            Gm.OnLoseGame();

        PoolCtrl.instance.Get(PoolTag.EXPLOSIVE3, transform.position, Quaternion.identity);
    }

    private IEnumerator DelayShowPopupRevive()
    {
        yield return Yielders.Get(1f);
        Gui.OpenBoard(UiPanelType.PopupRevive);
    }
}
