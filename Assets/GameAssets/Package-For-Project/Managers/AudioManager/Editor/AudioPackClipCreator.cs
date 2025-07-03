using UnityEngine;
using UnityEditor;

public class AudioPackClipCreator
{
    [MenuItem("Assets/Create/Audio/AudioPack %#F11")]
    static void CreateAudioPack()
    {
        AudioClip clip = null;
        var selections = Selection.objects;
        Object[] packs = new Object[selections.Length];

        for (int i = 0; i < selections.Length; i++)
        {
            var selection = selections[i];
            if (!selection)
                continue;

            if(selection is AudioClip)
            {
                clip = selection as AudioClip;
            }

            var pack = ScriptableObject.CreateInstance<AudioPackClip> ();
            pack.audioClip = clip;
            packs[i] = pack;

            string path = AssetDatabase.GetAssetPath (selection);
            if(!AssetDatabase.IsValidFolder (path))
            {
                string[] parts = path.Split ('/');
                var end = "/" + parts[parts.Length - 1];
                path = path.Replace(end, "");
                var name = end.Split ('.')[0];
                AssetDatabase.CreateAsset(pack, path + name + ".asset");
            }
            else
            {
                AssetDatabase.CreateAsset(pack, path + "/AudioPack.asset");
            }


        }

        Selection.objects = packs;
    }
}
