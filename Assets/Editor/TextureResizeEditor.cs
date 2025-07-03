#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class TextureResizeEditor : EditorWindow
{
    private Texture2D texture;
    private int newWidth;
    private int newHeight;
    private static TextureResizeEditor window;

    [MenuItem("Assets/Auto Resize Texture", true)]
    private static bool ValidateAutoResizeTexture()
    {
        foreach (var obj in Selection.objects)
        {
            if (!(obj is Texture2D))
                return false;
        }
        return true;
    }
    
    [MenuItem("Assets/Auto Resize Texture")]
    public static void OnClickAutoSize()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is Texture2D)
            {
                AutoResize(obj as Texture2D);
            }
        }
    }

    [MenuItem("Assets/Resize Texture", true)]
    private static bool ValidateResizeTexture()
    {
        return Selection.activeObject is Texture2D;
    }

    [MenuItem("Assets/Resize Texture")]
    public static void ShowWindow()
    {
        window = GetWindow<TextureResizeEditor>("Resize Texture");
        window.texture = Selection.activeObject as Texture2D;
    }

    private void OnGUI()
    {
        GUILayout.Label("Resize Texture", EditorStyles.boldLabel);
        Texture2D selectedTexture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);

        if (selectedTexture != texture)
        {
            texture = selectedTexture;
            if (texture != null)
            {
                newWidth = texture.width;
                newHeight = texture.height;
            }
        }

        newWidth = EditorGUILayout.IntField("New Width", newWidth);
        newHeight = EditorGUILayout.IntField("New Height", newHeight);

        if (texture != null && GUILayout.Button("Apply Resize"))
        {
            ApplyResize(texture, newWidth, newHeight);
        }
        else if (texture != null && GUILayout.Button("Auto Resize"))
        {
            AutoResize(texture);
        }
    }

    private static void ApplyResize(Texture2D texture, int width, int height)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            Texture2D originalTexture = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);
            Graphics.CopyTexture(texture, originalTexture);

            importer.maxTextureSize = Mathf.Max(2048, 2048);
            importer.SaveAndReimport();

            Texture2D resizedTexture = ResizeTexture(originalTexture, width, height);

            byte[] bytes = resizedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);

            AssetDatabase.Refresh();
            Debug.Log("Texture " + texture.name + " resized to " + width + "x" + height);
        }
    }

    private static void AutoResize(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            Texture2D originalTexture = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);
            Graphics.CopyTexture(texture, originalTexture);

            Vector2 newSize = FindNearestDivisibleSize(new Vector2(originalTexture.width, originalTexture.height));

            importer.maxTextureSize = Mathf.Max(2048, 2048);
            importer.SaveAndReimport();

            Texture2D resizedTexture = ResizeTexture(originalTexture, (int)newSize.x, (int)newSize.y);

            byte[] bytes = resizedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);

            AssetDatabase.Refresh();
            Debug.Log("Texture " + texture.name + " auto-resized to " + newSize.x + "x" + newSize.y);
        }
    }

    private static Texture2D ResizeTexture(Texture2D originalTexture, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 32);
        RenderTexture.active = rt;
        Graphics.Blit(originalTexture, rt);
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        newTexture.Apply();
        return newTexture;
    }

    private static Vector2 FindNearestDivisibleSize(Vector2 size)
    {
        Vector2 newSize = new Vector2();
        newSize.x = Mathf.Ceil(size.x / 4) * 4;
        newSize.y = Mathf.Ceil(size.y / 4) * 4;
        return newSize;
    }
}
#endif