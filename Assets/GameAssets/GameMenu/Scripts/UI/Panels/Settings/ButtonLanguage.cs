using System.Collections;
using System.Collections.Generic;
using language;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonLanguage : MonoBehaviour
{
    [Language] public string language;
    [SerializeField] TMP_Text txt_language;
    [SerializeField] GameObject obj_select;
    public Obj_Language obj_Language;

    private void Awake()
    {
        EventDispatcher.AddEvent(EventID.OnChangedLanguage, (o) => {
            if (o.ToString() != language)
            {
                obj_select.SetActive(false);
                txt_language.color = Color.white;
            }
        });
        EventDispatcher.AddEvent(EventID.OnRefreshUICHangeLanguage, (o) => {
            if (o.ToString() != language)
            {
                obj_select.SetActive(false);
                txt_language.color = Color.white;
            }
        });
        GetComponent<Button>().onClick.AddListener(OnClickChangeLanguage);

    }
    public void OnClickChangeLanguage()
    {
        obj_Language.language = language;
        obj_select.SetActive(true);
        txt_language.color = Color.yellow;
        EventDispatcher.PostEvent(EventID.OnRefreshUICHangeLanguage, language);
    }
    private void OnEnable()
    {
        txt_language.text = LanguageManager.GetText("languageName", language);
        obj_select.SetActive(LanguageManager.language == language);
        txt_language.color = LanguageManager.language == language ? Color.yellow : Color.white;
    }
}
