using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneTemplateCreator : EditorWindow {
	[MenuItem("Project-Tools/Scene Template Creator")]
	public static void CreateWindows()
	{
		EditorWindow editorWindow = GetWindow (typeof(SceneTemplateCreator), false, "Scenes Template Creator");
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.BeginWindows ();
	}

	string sceneName = "NewScene";

    private void OnGUI()
	{
		GUILayout.Space (20);
		sceneName = EditorGUILayout.TextField ("Scene Name", sceneName);

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Create"))
		{
			CreateSceneTemplate ();
		}
    }

	private void CreateSceneTemplate ()
	{
        if (string.IsNullOrEmpty (sceneName))
			return;

		var parentFolder = "Assets/GameAssets";
		var sceneFolder = Path.Combine (parentFolder, sceneName);

		CreateFolder (parentFolder, sceneName);
		CreateFolder (parentFolder, "Share");
        CreateFolder (sceneFolder, "Textures");
		CreateFolder (sceneFolder, "Scripts");
		CreateFolder (sceneFolder, "Prefabs");
		CreateFolder (sceneFolder, "Animations");

		var scenePath = Path.Combine (sceneFolder, sceneName + ".unity");
		var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset> (scenePath);

		if (!sceneAsset)
		{
			var scene = EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects);
			EditorSceneManager.SaveScene (scene, scenePath);
			sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset> (scenePath);
        }

		EditorGUIUtility.PingObject (sceneAsset);

        Debug.Log ("Created!");
    }

    private void CreateFolder (string parentFolder, string folder)
	{
        if (!AssetDatabase.IsValidFolder (Path.Combine (parentFolder, folder)))
		{
            AssetDatabase.CreateFolder (parentFolder, folder);
		}
    }
}
