using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBottomThemes : MonoBehaviour
{
    public GameObject tickMute;
    public GameObject loopSong;

    private void OnEnable()
    {
        CheckMute();
        CheckLoopSong();
    }

    public void Mute()
    {
        AudioManager.VolumeMusic = AudioManager.VolumeMusic == 1 ? 0 : 1f;
        CheckMute();
    }

    private void CheckMute()
    {
        bool isMute = AudioManager.VolumeMusic == 1 ? true : false;
        tickMute.SetActive(!isMute);
    }

    private void CheckLoopSong()
    {
        bool isLoop = GameDatas.IsLoopSong == 1 ? true : false;
        loopSong.SetActive(isLoop);
    }

    public void LoopSong()
    {
        GameDatas.IsLoopSong = GameDatas.IsLoopSong == 1 ? 0 : 1;
        CheckLoopSong();
    }
}
