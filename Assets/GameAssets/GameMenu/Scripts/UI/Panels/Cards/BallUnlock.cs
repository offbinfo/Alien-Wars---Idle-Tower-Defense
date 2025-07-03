using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallUnlock : MonoBehaviour
{
    [SerializeField]
    private Image imgBall;
    [SerializeField] PanelUnlockNewCard obj_newCard;
    private Card_SO data;
    private List<Card_SO> listcard;
    private Vector3 posCurrent;
    public float moveY = 400f;
    public float durationFade = 0.5f;
    public float duration = 1f;
    [SerializeField]
    private List<Sprite> iconBall;

    public bool isRoll = false;

    private void Start()
    {
        posCurrent = transform.localPosition;    
    }

    private void OnEnable()
    {
        
    }

    public void RollX1(Card_SO data, Action onRoll)
    {
        gameObject.SetActive(true);
        this.data = data;
        AnimateBall(OnSuccessRollX1, onRoll);
    }

    public void RollX5(List<Card_SO> listcard, Action onRoll)
    {
        gameObject.SetActive(true);
        this.listcard = listcard;
        AnimateBall(OnSuccessRollX5, onRoll);
    }

    private void OnSuccessRollX1()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); // chạy cả khi timeScale = 0

        seq.Append(imgBall.DOFade(0f, durationFade));
        //seq.Join(imgBall.rectTransform.DOLocalMoveY(imgBall.rectTransform.localPosition.y, duration));
        seq.AppendInterval(0.5f);

        obj_newCard.ShowCard(data);
        isRoll = false;
    }

    private void OnSuccessRollX5()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); // chạy cả khi timeScale = 0

        seq.Append(imgBall.DOFade(0f, durationFade));

        imgBall.rectTransform.localPosition = posCurrent;
        seq.AppendInterval(0.5f);

        obj_newCard.Show5Card(listcard);
        isRoll = false;
    }

    private void AnimateBall(Action callBack, Action onRoll)
    {
        isRoll = true;
        int random = UnityEngine.Random.Range(0, iconBall.Count);
        imgBall.sprite = iconBall[random];

        Color startColor = imgBall.color;
        startColor.a = 0;
        imgBall.color = startColor;

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); // chạy cả khi timeScale = 0

        seq.Append(imgBall.DOFade(1f, durationFade));
        seq.AppendInterval(1f);

        imgBall.rectTransform.localPosition = posCurrent;
        seq.Join(imgBall.rectTransform.DOLocalMoveY(imgBall.rectTransform.localPosition.y + moveY, duration))
            .OnComplete(() => {
                callBack?.Invoke();
                onRoll?.Invoke();
                });
    }
}
