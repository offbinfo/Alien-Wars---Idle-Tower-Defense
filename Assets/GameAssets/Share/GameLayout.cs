using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayout : Singleton<GameLayout>
{
    [SerializeField] GameObject processObj;
    [SerializeField] GameObject panelNoti;

    int process;

    public static void ShowProcess(bool value)
    {
        instance.process += value ? 1 : -1;
        instance.processObj.SetActive(instance.process > 0);
    }

    public void ShowPanelNoti()
    {
        StartCoroutine(IEActivePanelNoti());
    }

    private IEnumerator IEActivePanelNoti()
    {
        panelNoti.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        panelNoti.SetActive(false);
    }

}
