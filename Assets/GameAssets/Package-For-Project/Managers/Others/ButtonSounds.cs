using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent, RequireComponent (typeof (Button))]
public class ButtonSounds : MonoBehaviour
{
    public string soundName = "click_ui";

    private void Awake ()
    {
        GetComponent<Button> ()?.onClick.AddListener (OnClick);
        GetComponent<Toggle> ()?.onValueChanged.AddListener (delegate
        {
            OnClick ();
        });
    }

    private void OnClick ()
    {
        AudioManager.PlaySoundStatic (soundName);
    }
}
