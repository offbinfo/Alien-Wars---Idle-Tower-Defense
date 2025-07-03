using System.Collections.Generic;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace mapdrawer
{
    [ExecuteAlways, DisallowMultipleComponent, RequireComponent (typeof (Grid))]
    public class MapDrawer : MonoBehaviour
    {
        public string path;

        public float sizeGrid = 1f;
        public GridLayout.CellLayout layout;

        [HideInInspector] public int sourceIndex;

        [HideInInspector] public List<GameObject> sources;

        public GameObject source
        {
            get
            {
                if (sources == null || sources.Count == 0)
                {
                    return null;
                }

                return sources [Mathf.Clamp (sourceIndex, 0, sources.Count - 1)] as GameObject;
            }
        }

        public float size
        {
            get
            {
                var source = this.source;
                if (!source)
                    return 0;

                return source.GetComponent<DrawerElement> ().size;
            }
        }

        Grid grid;
        public Grid Grid
        {
            get
            {
                if (!grid)
                    grid = GetComponent<Grid> ();
                return grid;
            }
        }

#if UNITY_EDITOR
        private void Update ()
        {
            sources = Load<GameObject> (path);
        }

        private static List<T> Load<T> (string path) where T : Object
        {
            if (!Directory.Exists (path))
                return null;

            List<T> list = new List<T> ();

            List<string> allFile = new List<string> (Directory.EnumerateFiles (path, "*.prefab"));
            for (int i = 0; i < allFile.Count; i++)
            {
                list.Add (AssetDatabase.LoadAssetAtPath<T> (allFile [i]));
            }

            List<string> allFolder = new List<string> (Directory.EnumerateDirectories (path));
            for (int i = 0; i < allFolder.Count; i++)
            {
                list.AddRange (Load<T> (allFolder [i]));
            }

            return list;
        }

        [ContextMenu("Fix name")]
        private void FixName ()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild (i).name = string.Format ("TowerPlacement ({0})", i);
                EditorUtility.SetDirty (gameObject);
            }
        }
#endif
    }
}