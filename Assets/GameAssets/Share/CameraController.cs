using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    private Animator animator;
    public bool isInActive = true;
    private const string nameCloseBtnAnim = "close";
    private const string nameOpenBtnAnim = "open";

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    public Camera mainCam;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private IEnumerator IEEndShake(float intensity)
    {
        noise.m_AmplitudeGain = intensity;
        yield return Yielders.Get(0.4f);
        noise.m_AmplitudeGain = 0f;
    }

    public void InActiveCamCinematic()
    {
        if(isInActive) return;
        isInActive = true;
        animator.Play(nameOpenBtnAnim);
    }
    public void ActiveCamCinematic()
    {
        isInActive = false;
        animator.Play(nameCloseBtnAnim);
    }
}
