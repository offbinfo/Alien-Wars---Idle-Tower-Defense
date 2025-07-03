using EasyTransition;
using System.Collections;
using UnityEngine.SceneManagement;
public enum TypeScene
{
    GameMenu = 0,
    GamePlay = 1,
    Arena = 2,
}
public class LoadSceneManager: SingletonGame<LoadSceneManager>
{
    public TransitionSettings transition;

    public void LoadScene(TypeScene sceneName)
    {
        StartCoroutine(IELoadSceneAndTransition(sceneName));
    }

    public void LoadSceneAsync(TypeScene sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName.ToString());
    }

    private IEnumerator IELoadSceneAndTransition(TypeScene sceneName)
    {
        TransitionManager.instance.Transition(transition, 0f);
        yield return Yielders.Get(0.8f);
        SceneManager.LoadScene(sceneName.ToString());
    }
}
