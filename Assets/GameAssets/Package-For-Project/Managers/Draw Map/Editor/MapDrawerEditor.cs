using UnityEditor;
using UnityEngine;

namespace mapdrawer
{
    [CustomEditor (typeof (MapDrawer))]
    public class MapDrawerEditor : Editor
    {
        GameObject source => script.source;

        MapDrawer script;

        Vector2 pointCurrent
        {
            get
            {
                Ray mouseRay = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
                var mousePos = mouseRay.GetPoint (0f);
                var grid = script.Grid;
                var cell = grid.LocalToCell (mousePos);
                var point = grid.GetCellCenterLocal (cell);

                var a = script.sizeGrid / 2;
                var b = script.size / 2;

                var offset = b - a;

                point += new Vector3 (1, 1) * offset;

                return point;
            }
        }

        bool remove => Event.current.control;

        private void Awake ()
        {
            script = target as MapDrawer;
            script.Grid.hideFlags = HideFlags.NotEditable;
        }

        private void OnDestroy ()
        {
            script.Grid.hideFlags = HideFlags.None;
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            if (!source)
                return;

            GUILayout.Space (20f);

            var textures = new Texture2D [script.sources.Count];
            for (int i = 0; i < script.sources.Count; i++)
            {
                textures [i] = AssetPreview.GetAssetPreview (script.sources [i]);
            }

            script.sourceIndex = GUILayout.SelectionGrid (script.sourceIndex, textures, 5, GUILayout.Height (((textures.Length - 1) / 5 + 1) * 40));

            if (GUILayout.Button ("Clear"))
            {
                while (script.transform.childCount > 0)
                {
                    var child = script.transform.GetChild (0).gameObject;
                    Undo.DestroyObjectImmediate (child);
                }
            }

            script.Grid.cellSize = script.sizeGrid * Vector3.one;
            script.Grid.cellLayout = script.layout;
        }

        public void OnSceneGUI ()
        {
            HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));

            if (IsMouse (0) || IsMouseUp (0))
            {
                if (!remove)
                {
                    CreateAtPoint (pointCurrent);
                }
                else
                {
                    DestroyAtPoint (pointCurrent);
                }
            }

            Handles.color = !remove ? Color.green : Color.red;

            var size = script.size;

            Handles.DrawWireCube (
                pointCurrent,
                size * Vector3.one
            );

            SceneView.RepaintAll ();
        }

        private void CreateAtPoint (Vector3 position)
        {
            for (int i = 0; i < script.transform.childCount; i++)
            {
                var element = script.transform.GetChild (i).gameObject;

                if (InRect (position, script.size / 2f, element.transform.position))
                {
                    return;
                }
            }

            GameObject g = PrefabUtility.InstantiatePrefab (source) as GameObject;
            g.transform.position = position;
            g.transform.SetParent (script.transform);
            g.name = source.name + string.Format (" ({0})", g.transform.GetSiblingIndex ());
            Undo.RegisterCreatedObjectUndo (g, "Add child");
        }

        private void DestroyAtPoint (Vector3 position)
        {
            for (int i = 0; i < script.transform.childCount; i++)
            {
                var g = script.transform.GetChild (i).gameObject;

                if (InRect (position, script.size / 2f, g.transform.position))
                {
                    Undo.DestroyObjectImmediate (g);
                }
            }
        }
        
        private bool InRect (Vector3 center, float extent, Vector3 point)
        {
            return point.x >= center.x - extent && point.x <= center.x + extent
                && point.y >= center.y - extent && point.y <= center.y + extent;
        }

        private bool IsMouseDown (int mouseIndex)
        {
            return Event.current.type == EventType.MouseDown && Event.current.button == mouseIndex;
        }

        private bool IsMouseUp (int mouseIndex)
        {
            return Event.current.type == EventType.MouseUp && Event.current.button == mouseIndex;
        }

        private bool IsMouse (int mouseIndex)
        {
            return Event.current.type == EventType.MouseDrag && Event.current.button == mouseIndex;
        }
    }
}