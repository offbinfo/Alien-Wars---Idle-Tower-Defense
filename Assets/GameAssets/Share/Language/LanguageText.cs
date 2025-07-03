using UnityEngine;
using TMPro;
using language;

[ExecuteAlways, DisallowMultipleComponent]
public class LanguageText : MonoBehaviour
{
    [SerializeField] string key = null;
    [SerializeField, TextArea] string [] formats = null;

    TextMeshProUGUI text = null;

    private void Awake ()
    {
        text = GetComponent<TextMeshProUGUI> ();
    }

    private void Start ()
    {
        LanguageManager.onChangeLanguage += SetText;
        SetText ();
    }

    void OnDestroy ()
    {
        LanguageManager.onChangeLanguage -= SetText;
    }

    private void SetText ()
    {
        if (!text)
            Awake ();

        if (text && !string.IsNullOrEmpty (key))
        {
            if (formats != null && formats.Length > 0)
                text.SetText (string.Format (LanguageManager.GetText (key), formats));
            else
                text.SetText (LanguageManager.GetText (key));
        }
    }

#if UNITY_EDITOR
    private void Update ()
    {
        SetText ();
    }
#endif
}
