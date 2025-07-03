using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

#if UNITY_EDITOR
public static class PublicAudioUtil
{
    public static void PlayClip (AudioClip clip, bool loop)
    {
        StopClips ();

        if (!clip)
            return;

        Assembly unityEditorAssembly = typeof (AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType ("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod (
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type [] {
                typeof(AudioClip),
                typeof(int),
                typeof(bool)
            },
            null
        );

        method.Invoke (
            null,
            new object [] {
                clip,
                0,
                loop
            }
        );
    }

    public static void StopClips ()
    {
        Assembly unityEditorAssembly = typeof (AudioImporter).Assembly;

        Type audioUtilClass = unityEditorAssembly.GetType ("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod (
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type [] { },
            null
        );

        method.Invoke (
            null,
            new object [] { }
        );
    }
}
#endif
