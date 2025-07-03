using UnityEngine;

public class TabsManager : MonoBehaviour
{
    [SerializeField] ButtonTab [] buttonTabs;
    [SerializeField] GameObject [] tabs;

    public void SetTab (int tabIndex)
    {
        for (int i = 0; i < buttonTabs.Length; i++)
        {
            var active = i == tabIndex;
            tabs [i].SetActive (active);
            buttonTabs [i].Activate (active);
        }
    }

    private void Start ()
    {
        SetTab (0);
    }
}
