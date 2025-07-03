using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class SavePrefsModify : EditorWindow
{
    [MenuItem("SavePrefs/Modify")]
    public static void CreateWindows()
    {
        EditorWindow editorWindow = GetWindow<SavePrefsModify> (false);
        editorWindow.Show (true);
    }

    [MenuItem("SavePrefs/Delete All %K")]
    public static void DeleteAll()
    {
        SavePrefs.DeleteAll ();
        Debug.Log ("Clean all player prefs");
    }

    public enum TypePrefs
    {
        Int,
        Float,
        String,
    }

    [SerializeField] TypePrefs typePrefs = 0;
    [SerializeField] string key = null;
    [SerializeField] int intValue = 0;
    [SerializeField] int floatValue = 0;
    [SerializeField] string stringValue = null;

    void Action()
    {
        if(typePrefs == TypePrefs.Int)
            SavePrefs.SetInt (key, intValue);
        else if (typePrefs == TypePrefs.Float)
            SavePrefs.SetFloat (key, floatValue);
        else if (typePrefs == TypePrefs.String)
            SavePrefs.SetString (key, stringValue);

        ShowNotification (new GUIContent ("Modify success!"));
    }

    SerializedObject serializedObject = null;
    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        keyProperty = serializedObject.FindProperty ("key");
        typePrefsProperty = serializedObject.FindProperty ("typePrefs");
        intValueProperty = serializedObject.FindProperty ("intValue");
        floatValueProperty = serializedObject.FindProperty ("floatValue");
        stringValueProperty = serializedObject.FindProperty ("stringValue");
    }

    SerializedProperty keyProperty = null, typePrefsProperty = null, intValueProperty = null, floatValueProperty = null, stringValueProperty = null;
    void OnGUI()
    {
        EditorGUILayout.PropertyField (typePrefsProperty);
        GUILayout.BeginHorizontal ();
        EditorGUILayout.PropertyField (keyProperty);
        if(GUILayout.Button("Delete key", GUILayout.ExpandWidth(false)))
        {
            SavePrefs.DeleteKey (keyProperty.stringValue);
            ShowNotification (new GUIContent ("Delete success"));
        }

        GUILayout.EndHorizontal ();
        GUILayout.Space (20);

        if (typePrefsProperty.enumValueIndex == 0)
            EditorGUILayout.PropertyField (intValueProperty);
        else if (typePrefsProperty.enumValueIndex == 1)
            EditorGUILayout.PropertyField (floatValueProperty);
        else if (typePrefsProperty.enumValueIndex == 2)
            EditorGUILayout.PropertyField (stringValueProperty);


        GUILayout.FlexibleSpace ();

        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        if(GUILayout.Button("Modify", GUILayout.ExpandWidth(false), GUILayout.MinWidth(75)))
        {
            Action ();
        }
        GUILayout.EndHorizontal ();

        if (GUI.changed)
            serializedObject.ApplyModifiedProperties ();
    }
}
