using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Text : MonoBehaviour
{
    public TMP_Text txt_FPS;
    float fps;
    private void Start()
    {
        StartCoroutine(I_UpdateText());
    }
    private void Update()
    {
        fps = 1f/ Time.deltaTime;
        //txt_FPS.text = "FPS:"+fps.ToString("00.");
    }
    IEnumerator I_UpdateText()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            txt_FPS.text = "FPS:"+fps.ToString("00.");
        }
    }

}
