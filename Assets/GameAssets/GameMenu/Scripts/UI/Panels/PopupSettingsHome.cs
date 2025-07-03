using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSettingsHome : UIPanel , IBoard
{
    [SerializeField]
    private Slider sliderSound;
    [SerializeField]
    private Slider sliderMusic;
    [SerializeField]
    private Obj_Language obj_Language;


    public override UiPanelType GetId()
    {
        return UiPanelType.PopupSettingsHome;
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
    }

    public void BtnGiftCode()
    {
        Board_UIs.instance.ShowPanelNoti();
    }

    public void BtnChangeLanguage()
    {
        //Board_UIs.instance.ShowPanelNoti();
        obj_Language.gameObject.SetActive(!obj_Language.gameObject.activeSelf);
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
