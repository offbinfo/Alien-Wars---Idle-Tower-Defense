using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGame<GameManager>
{

    private GameState gameState;
    private bool isEndGame = false;
    public bool isPaused = false;
    public bool isResumeWave = false;
    public bool isBattleArena = false;

    public bool isFirstBattleArena = false;
    public GameState GameState => gameState;

    protected override void Awake()
    {
        base.Awake();

        DOTween.Init().SetCapacity(1000, 500);
        GameDatas.DefaultFirstInGame();
    }

    private void Start()
    {
        /*GameDatas.SetHighestWaveInWorld(0, 1500);
        GameDatas.SetHighestWaveInWorld(1, 1500);
        GameDatas.SetHighestWaveInWorld(2, 1500);
        GameDatas.SetHighestWaveInWorld(3, 1500);
        GameDatas.SetHighestWaveInWorld(4, 1500);
        GameDatas.SetHighestWaveInWorld(5, 1500);*/
    }

    public void ReturnHome()
    {
        gameState = GameState.PrepareGame;
    }

    public void PlayGame(bool isResume)
    {
        isBattleArena = false;
        isResumeWave = isResume;
        TimeGame.PauseTutorial = false;
        TimeGame.Pause = false;

        isEndGame = false;
        gameState = GameState.PlayingGame;
        QuestEventManager.BattleStarted(1);
        LoadSceneManager.instance.LoadScene(TypeScene.GamePlay);
    }

    public void PlayArenaGame()
    {
        EventChallengeListenerManager.PlayArenaTimes(1);
        GameAnalytics.LogTotalUserRePlayArena(GameDatas.TotalReplayArena);

        if (!GameDatas.IsArenaChain())
        {
            GameAnalytics.LogTotalUserPlayArena(1);
            GameDatas.TotalArenaRankPlay += 1;
            GameDatas.ArenaChain(true);
        }

        GameDatas.StartBattleTimer();

        GameDatas.StartBattleArena(true);

        isBattleArena = true;
        TimeGame.PauseTutorial = false;
        TimeGame.Pause = false;

        isEndGame = false;
        gameState = GameState.PlayingGame;

        GameDatas.CurrentRank = GameDatas.GetHighestRank();

        QuestEventManager.BattleStarted(1);
        LoadSceneManager.instance.LoadScene(TypeScene.Arena);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.EndGame || gameState == GameState.WinGame) return;
        StartCoroutine(DelayShowPopupWin());
    }

    public void ResetStateGamePlay()
    {
        isEndGame = false;
        gameState = GameState.PlayingGame;
    }

    private IEnumerator DelayShowPopupWin()
    {
        yield return Yielders.Get(1f);
        isEndGame = true;
        gameState = GameState.WinGame;
    }

    public void OnLoseGame(float delayPopupShowTime = 1f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.EndGame || gameState == GameState.WinGame) return;
        StartCoroutine(DelayShowPopupEndGame(delayPopupShowTime));
    }

    private IEnumerator DelayShowPopupEndGame(float delayPopupShowTime)
    {
        yield return Yielders.Get(delayPopupShowTime);
        EndGame();
    }

    public void EndGame()
    {
        GameDatas.countLoseGame++;
        isEndGame = true;
        gameState = GameState.EndGame;
        Board_UIs.instance.OpenBoard(UiPanelType.PopupEndGame);
    }

    public void OnApplicationQuit()
    {
        DOTween.KillAll();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        GameDatas.IsEndTutorial = true;
    }

}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    WaitingResult,
    EndGame,
    WinGame,
}

