using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimator : MonoBehaviour
{
    //--------------------------------------------------//
    private void PlaySound (string name)
    {
        AudioManager.PlaySoundStatic (name);
    }

    private void PlayMusic (string name)
    {
        AudioManager.PlayMusicStatic (name);
    }
}
