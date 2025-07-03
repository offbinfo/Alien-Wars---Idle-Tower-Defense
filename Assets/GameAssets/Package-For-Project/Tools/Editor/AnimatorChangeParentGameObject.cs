using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimatorChangeParentGameObject : MonoBehaviour
{
    [MenuItem ("GameObject/Animator/Add Parent")]
    public static void AddParent ()
    {
        if (!Selection.activeObject)
            return;

        var target = Selection.activeObject as GameObject;

        if (!target)
            return;

        var anim = FindAnim (target);

        if (anim)
        {
            var currentPath = GetPath (target);

            var index = target.transform.GetSiblingIndex ();
            var newParent = new GameObject("Parent");
            newParent.transform.SetParent (target.transform.parent);
            newParent.transform.localPosition = Vector3.zero;
            newParent.transform.localScale = Vector3.one;
            newParent.transform.SetSiblingIndex (index);

            target.transform.SetParent (newParent.transform);
            var newPath = GetPath (target);

            var runtimeController = anim.runtimeAnimatorController;
            if (runtimeController)
                ChangeAnimationPath (runtimeController, currentPath, newPath);

            EditorUtility.SetDirty (target);
        }
        else
        {
            Debug.Log ("Not fould animator controller!");
        }
    }

    [MenuItem ("GameObject/Animator/Set Parent")]
    public static void SetParentAction ()
    {
        if (!Selection.activeObject)
            return;

        var target = Selection.activeObject as GameObject;

        if (!target)
            return;

        var currentPath = GetPath (target);

        Selection.selectionChanged += SetParent;

        void SetParent ()
        {
            Selection.selectionChanged -= SetParent;

            if (Selection.activeObject is GameObject)
            {
                var anim = FindAnim (target);

                if (anim)
                {
                    var newParent = Selection.activeObject as GameObject;
                    if (newParent.transform.IsChildOf (anim.transform) || newParent == anim.transform.gameObject)
                    {
                        if (!newParent.transform.IsChildOf (target.transform))
                        {
                            target.transform.SetParent (newParent.transform);
                            var newPath = GetPath (target);
                            var runtimeController = anim.runtimeAnimatorController;
                            if (runtimeController)
                                ChangeAnimationPath (runtimeController, currentPath, newPath);
                        }
                    }
                    else
                    {
                        EditorGUIUtility.PingObject (anim.gameObject);
                    }

                    EditorUtility.SetDirty (target);
                }
                else
                {
                    Debug.Log ("Not fould animator controller!");
                }
            }
        }
    }

    private static void ChangeAnimationPath (RuntimeAnimatorController runtimeAnimatorController, string oldPath, string newPath)
    {
        if (!runtimeAnimatorController)
            return;

        var clips = runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips [i];
            var path = AssetDatabase.GetAssetPath (clip);

            var text = File.ReadAllText (path);
            text = text.Replace (AddFullPath (oldPath), AddFullPath (newPath));
            File.WriteAllText (path, text);
        }

        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh ();
    }

    private static Animator FindAnim (GameObject target)
    {
        Animator anim = null;

        while (target.transform.parent)
        {
            target = target.transform.parent.gameObject;
            anim = target.GetComponent<Animator> ();

            if (anim)
                break;
        }

        return anim;
    }

    private static string AddFullPath (string path)
    {
        return "path: " + path;
    }

    private static string GetPath (GameObject g)
    {
        string path = g.name;
        while (g.transform.parent && !g.transform.parent.GetComponent<Animator> ())
        {
            g = g.transform.parent.gameObject;
            path = g.name + "/" + path;
        }

        return path;
    }
}
