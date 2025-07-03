using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonKeyBoard : MonoBehaviour
{
    [SerializeField] KeyCode keyCode;

    Button button;
    Canvas canvas;
    PointerEventData eventData = new PointerEventData (EventSystem.current);
    private void Awake ()
    {
        button = GetComponent<Button> ();
        canvas = GetComponentInParent<Canvas> ();
    }

    List<RaycastResult> raycastResults = new List<RaycastResult> ();
    private void Update ()
    {
        if (Input.GetKeyDown (keyCode))
        {
            eventData.position = canvas.worldCamera.WorldToScreenPoint (transform.position);
            EventSystem.current.RaycastAll (eventData, raycastResults);

            if (raycastResults.Count > 0 && raycastResults [0].gameObject.transform.IsChildOf (gameObject.transform))
            {
                if (button.interactable)
                    button.onClick.Invoke ();
            }
        }
    }
}
