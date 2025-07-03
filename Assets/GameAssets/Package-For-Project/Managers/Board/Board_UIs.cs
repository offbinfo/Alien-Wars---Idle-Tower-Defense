using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board_UIs : Singleton<Board_UIs>
{
    Dictionary<UiPanelType, UIPanel> dicBoards = new();

    List<UIPanel> boardsActive = new List<UIPanel> ();
    private UIPanel uiParent;
    public GameObject panelNoti;

    public UIPanel UiParent => uiParent;

    public Dictionary<UiPanelType, UIPanel> DicBoards { get => dicBoards; }

    public UIPanel GetBoard (UiPanelType boardName)
    {
        return dicBoards [boardName];
    }

    public bool IsBoardOpening ()
    {
        return boardsActive.Count > 0;
    }

    public bool IsBoardOpening (UIPanel board)
    {
        return boardsActive.Contains (board);
    }

    public bool IsBoardOpening (UiPanelType boardName)
    {
        if (!dicBoards.ContainsKey (boardName))
            return false;

        return boardsActive.Contains (dicBoards [boardName]);
    }

    public void OpenBoard (UiPanelType boardName)
    {
        OpenAndReturnBoard (boardName);
    }

    public UIPanel OpenAndReturnBoard (UiPanelType boardName)
    {
        if (!dicBoards.ContainsKey (boardName))
        {
            Debug.Log (string.Format ("{0} is not exits.", boardName));
            return null;
        }

        var board = dicBoards [boardName];
        board.gameObject.SetActive (true);

        var i = board.GetComponents<IBoard> ();
        foreach (var item in i)
        {
            item.OnBegin ();
        }

        if (boardsActive.Contains (board))
        {
            boardsActive.Remove (board);
        }

        if (boardsActive.Count > 0)
        {
            boardsActive [boardsActive.Count - 1].gameObject.SetActive (false);

            var ii = boardsActive [boardsActive.Count - 1].GetComponents<IBoard> ();
            foreach (var item in ii)
            {
                item.OnClose ();
            }
        }

        boardsActive.Add (board);
        return board;
    }

    public void CloseBoard (UiPanelType boardName)
    {
        if (!dicBoards.ContainsKey (boardName))
            return;

        var board = dicBoards [boardName];

        if (!boardsActive.Contains (board))
            return;

        board.gameObject.SetActive (false);
        var i = board.GetComponents<IBoard> ();
        foreach (var item in i)
        {
            item.OnClose ();
        }

        boardsActive.Remove (board);

        if (boardsActive.Count > 0)
        {
            boardsActive [boardsActive.Count - 1].gameObject.SetActive (true);
            boardsActive [boardsActive.Count - 1].GetComponent<IBoard> ().OnBegin ();
        }
    }

    public void CloseBoard (UIPanel board)
    {
        CloseBoard (board);
    }

    public void OpenBoard (UIPanel board)
    {
        OpenBoard (board);
    }

    protected override void Awake ()
    {
        base.Awake ();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<UIPanel>() != null)
            {
                var board = transform.GetChild(i).GetComponent<UIPanel>();
                if (board.GetId() != UiPanelType.ScreenMainMenu
                    && board.GetId() != UiPanelType.ScreenGamePlay)
                {
                    dicBoards.Add(board.GetId(), board);
                } else
                {
                    uiParent = board;
                }
            }
        }
    }

    public void ShowPanelNoti()
    {
        StartCoroutine(IEActivePanelNoti());
    }

    private IEnumerator IEActivePanelNoti()
    {
        panelNoti.SetActive(true);
        yield return Yielders.Get(1f);
        panelNoti.SetActive(false);
    }
}
/*
public class ControlAllButton : MonoBehaviour
{
    private static List<Button> allButtons = new List<Button>();

    private void Awake()
    {
        allButtons.Clear();
        allButtons.AddRange(FindObjectsOfType<Button>(true));
    }

    public static void ActiveButton(GameObject btn)
    {
        if (btn.TryGetComponent(out Button button))
        {
            foreach (var b in allButtons)
            {
                bool isActive = (b == button);
                foreach (var item in b.GetComponentsInChildren<Graphic>())
                {
                    item.raycastTarget = isActive;
                }
            }
        }
    }

    public static void DeactiveAllButton()
    {
        foreach (var b in allButtons)
        {
            foreach (var item in b.GetComponentsInChildren<Graphic>())
            {
                item.raycastTarget = false;
            }
        }
    }

    public static void ActiveAllButton()
    {
        foreach (var b in allButtons)
        {
            foreach (var item in b.GetComponentsInChildren<Graphic>())
            {
                item.raycastTarget = true;
            }
        }
    }
}*/


public class ControlAllButton
{
    public static void ActiveButton(GameObject btn)
    {
        var allButtons = Object.FindObjectsOfType<Button>(true);
        var button = btn.GetComponent<Button>();
        foreach (var b in allButtons)
        {
            foreach (var item in b.GetComponentsInChildren<Graphic>())
            {
                item.raycastTarget = button == b;
            }
        }
    }
    public static void DeactiveAllButton()
    {
        var allButtons = Object.FindObjectsOfType<Button>(true);
        foreach (var b in allButtons)
        {
            foreach (var item in b.GetComponentsInChildren<Graphic>())
            {
                item.raycastTarget = false;
            }
        }
    }
    public static void ActiveAllButton()
    {
        var allButtons = Object.FindObjectsOfType<Button>(true);
        foreach (var b in allButtons)
        {
            foreach (var item in b.GetComponentsInChildren<Graphic>())
            {
                item.raycastTarget = true;
            }
        }
    }
}
