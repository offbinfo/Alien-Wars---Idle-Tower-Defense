using UnityEngine;

public class Sounder : MonoBehaviour
{
    [SerializeField] AudioPackClip clip;

    public void OnEnable ()
    {
        AudioManager.PlaySoundStatic (clip);
    }
}
