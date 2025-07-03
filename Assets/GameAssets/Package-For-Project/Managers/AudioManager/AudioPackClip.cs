using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioPackClip : ScriptableObject
{

    public AudioClip audioClip = null;
    public bool loop = false;
}

#if UNITY_EDITOR
[CustomEditor (typeof (AudioPackClip))]
public class AudioPackClipEditor : Editor
{
    private void Awake ()
    {
        PlaySound ();
    }

    private void OnDisable ()
    {
        StopSound ();
    }

    void PlaySound ()
    {
        var pack = target as AudioPackClip;
        PublicAudioUtil.PlayClip (pack.audioClip, pack.loop);
    }

    void StopSound ()
    {
        PublicAudioUtil.StopClips ();
    }

    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI ();

        if (GUILayout.Button (">>", GUILayout.ExpandWidth (false)))
        {
            PlaySound ();
        }
    }
}
#endif
