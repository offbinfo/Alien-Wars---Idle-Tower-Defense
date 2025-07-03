using DG.Tweening;
using EasyTransition;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : GameMonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject textLoading;
    [SerializeField] private CanvasGroup loadingCanvas;

    private AsyncOperation asyncLoad;
    private string loadedScene = "";

    private void Start()
    {
        Application.targetFrameRate = 200;
        textLoading.gameObject.SetActive(true);
        StartCoroutine(PreloadScene());
    }

    private IEnumerator PreloadScene()
    {
        loadedScene = GameDatas.IsFirstInGame ? TypeScene.GameMenu.ToString() : TypeScene.GamePlay.ToString();
        asyncLoad = SceneManager.LoadSceneAsync(loadedScene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.DOValue(targetProgress, 0.5f);

            if (asyncLoad.progress >= 0.9f)
            {
                GameDatas.IsFirstInGame = true;
                yield return new WaitForSeconds(0.5f);
                loadingSlider.DOValue(1, 0.5f).OnComplete(() =>
                {
                    asyncLoad.allowSceneActivation = true;
                });

                yield return new WaitUntil(() => asyncLoad.isDone);
                yield return Yielders.Get(0.5f);

                loadingCanvas.DOFade(0, 0.5f).OnComplete(() =>
                {
                    EventChallengeListenerManager.LoginDays(1);
                    SceneManager.UnloadSceneAsync(0);
                });
                yield break;
            }

            yield return null;
        }
    }
}
