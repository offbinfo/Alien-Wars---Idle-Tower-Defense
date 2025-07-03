using language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Obj_Language : MonoBehaviour
{
    bool isChangedLanguage = false;
    [Language] public string language;
    public Button btnChange;

    private void Awake() {
        
        EventDispatcher.AddEvent(EventID.OnChangedLanguage,(o)=>{
            isChangedLanguage = true;
        });
    }
    private void OnEnable() {
        isChangedLanguage = false;
        btnChange.interactable = true;
    }
    public void OnclickbtnChange()
    {
        btnChange.interactable = false;
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        LanguageManager.language = language;
        EventDispatcher.PostEvent(EventID.OnChangedLanguage, language);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(TypeScene.GameMenu.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        gameObject.SetActive(false);
        btnChange.interactable = true;
        DebugCustom.LogColor("Change Language Success");
    }
}
