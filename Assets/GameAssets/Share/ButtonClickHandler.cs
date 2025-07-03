using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btn;
    private Transform _buttonTransform;
    private Tween _scaleTween;
    public float _scaleUp = 1.1f;
    public float _scale = 1f;
    private readonly float _scaleDuration = 0.1f;

    private void Awake()
    {
        _buttonTransform = transform;
        btn = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.PlaySoundStatic("click_ui");
        _scaleTween?.Kill();
        _scaleTween = _buttonTransform.DOScale(_scaleUp, _scaleDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _scaleTween?.Kill();       
        _scaleTween = _buttonTransform.DOScale(_scale, _scaleDuration).SetEase(Ease.OutQuad);
    }

/*    private void Start()
    {
        if (btn != null)
            btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        AudioManager.PlaySoundStatic("click_ui");
    }

    private void OnDestroy()
    {
        if (btn != null)
            btn.onClick.RemoveListener(OnClick);
    }*/
}
