using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabRelicsChange : MonoBehaviour
{
    [SerializeField]
    private RelicManagerSO RelicManagerSO;
    [SerializeField]
    private RelicsContainer relicsContainerRare;
    [SerializeField]
    private RelicsContainer relicsContainerEpic;

    [SerializeField]
    private GameObject panelInfor;


    private void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        relicsContainerRare.SetUp(RelicManagerSO.relicRares);
        relicsContainerEpic.SetUp(RelicManagerSO.relicEpics);
    }

    public void OpenPanelInforRelic()
    {
        panelInfor.SetActive(true);
    }
}
