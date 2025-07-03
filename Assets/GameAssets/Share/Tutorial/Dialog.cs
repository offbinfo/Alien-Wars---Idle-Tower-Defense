using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class Dialog : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] TMP_Text txt_Dialog;

    private void OnEnable()
    {
        BlockInputUI.instance.blockInput(true);
    }

    public Coroutine Action(string text, Func<bool> condition, bool isOpenDialog = true, bool isEndDialog = true)
    {
        if (isOpenDialog) _rect.anchoredPosition = new Vector2(1080, 0);//sang bên phải
        gameObject.SetActive(true);
        return StartCoroutine(IE_Action(text, condition, isEndDialog));
    }
    private IEnumerator IE_Action(string text, Func<bool> condition, bool isEndDialog)
    {
        ControlAllButton.DeactiveAllButton();
        txt_Dialog.text = "";
        //chạy anim ra giữa màn hình trong khoảng 0.3f s
        var isCompleteTween = false;
        var tweenContent = _rect.DOAnchorPosX(0, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            isCompleteTween = true;
        });
        yield return new WaitUntil(() => isCompleteTween);
        //chạy anim chữ 
        var animText = StartCoroutine(IE_AnimText(text));
        //nếu tap thì chữ sẽ chạy hết
        StartCoroutine(IE_CheckInput(text, animText));
        yield return new WaitUntil(() => txt_Dialog.text == text);
        yield return null;
        yield return new WaitUntil(() => condition.Invoke());
        if (isEndDialog)
        {
            bool isDone = false;
            DebugCustom.Log("End dialog");
            _rect.DOAnchorPosX(1080, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDone = true;
            }).SetUpdate(true);
            yield return new WaitUntil(() => isDone);
            gameObject.SetActive(false);
            ControlAllButton.ActiveAllButton();
            DebugCustom.Log("Done");
        }
        else
            gameObject.SetActive(true);
    }

    #region Anim text
    private IEnumerator IE_AnimText(string fullText)
    {
        var chars = fullText.ToCharArray();
        var s = "";
        foreach (var c in chars)
        {
            s += c;
            txt_Dialog.text = s;
            yield return new WaitForSecondsRealtime(0.05f);
        }

    }
    private IEnumerator IE_CheckInput(string fullText, Coroutine co)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        //destroy coroutine 
        txt_Dialog.text = fullText;
        StopCoroutine(co);
    }
    #endregion

    private void OnDisable()
    {
        BlockInputUI.instance.blockInput(false);
    }
}



/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialog : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TMP_Text txt_Dialog;
    private bool isSkipText = false;

    private void OnEnable()
    {
        BlockInputUI.instance.blockInput(true);
    }

    public Coroutine Action(string text, Func<bool> condition, bool isOpenDialog = true, bool isEndDialog = true)
    {
        if (isOpenDialog) _rect.anchoredPosition = new Vector2(1080, 0);
        gameObject.SetActive(true);
        return StartCoroutine(IE_Action(text, condition, isEndDialog));
    }

    private IEnumerator IE_Action(string text, Func<bool> condition, bool isEndDialog)
    {
        ControlAllButton.DeactiveAllButton();
        txt_Dialog.text = "";

        yield return _rect.DOAnchorPosX(0, 0.3f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();

        yield return StartCoroutine(IE_AnimText(text));

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        yield return new WaitUntil(() => condition.Invoke());

        if (isEndDialog)
        {
            yield return _rect.DOAnchorPosX(1080, 0.3f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
            gameObject.SetActive(false);
            ControlAllButton.ActiveAllButton();
        }
    }

    #region Anim text
    private IEnumerator IE_AnimText(string fullText)
    {
        txt_Dialog.text = "";
        isSkipText = false;
        float delay = 0.05f;

        for (int i = 0; i < fullText.Length; i++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSkipText = true;
                break;
            }
            txt_Dialog.text += fullText[i];
            yield return new WaitForSecondsRealtime(delay);
        }

        txt_Dialog.text = fullText;
    }
    #endregion

    private void OnDisable()
    {
        BlockInputUI.instance.blockInput(false);
    }
}*/
