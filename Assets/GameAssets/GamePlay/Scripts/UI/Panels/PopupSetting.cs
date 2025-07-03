using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupSetting : UIPanel, IBoard
{
    [SerializeField]
    private Slider sliderSound;
    [SerializeField]
    private Slider sliderMusic;
    public override UiPanelType GetId()
    {
        return UiPanelType.PopupSetting;
    }

    private void OnEnable()
    {
        OnAppear();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private void Init()
    {
        sliderMusic.value = AudioManager.VolumeMusic;
        sliderSound.value = AudioManager.VolumeSound;
        TimeGame.Pause = true;
    }

    public void BtnQuit()
    {
        /*if (!GameDatas.IsEndTutorial) 
        {
            GameLayout.instance.ShowPanelNoti();
            return;
        }*/

        TimeGame.Pause = false;
        Close();
        Gm.EndGame();
        GPm.EndRoundWave(true);
    }

    public void OnActiveSound()
    {
        AudioManager.VolumeSound = AudioManager.VolumeSound == 1 ? 0 : 1;
        Init();
    }

    public void OnActiveMusic()
    {
        AudioManager.VolumeMusic = AudioManager.VolumeMusic == 1 ? 0 : 1f;
        Init();
    }

    public void BtnContinue()
    {
        TimeGame.Pause = false;
        Close();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}