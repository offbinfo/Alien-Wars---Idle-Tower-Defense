using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProjectTools
{
    [CustomEditor (typeof (PolygonDrawer))]
    public class PolygonDrawerEditor : Editor
    {
        float distanceFlag => SceneView.currentDrawingSceneView.size * .05f;
        Vector2 mousePos
        {
            get
            {
                Ray mouseRay = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
                var mousePos = mouseRay.GetPoint (0f);
                var grid = polygonDrawer.Grid;
                var cell = grid.LocalToCell (mousePos);
                var point = grid.GetCellCenterLocal (cell);

                return point;
            }
        }
        float pointSize => SceneView.currentDrawingSceneView.size * .05f;

        PolygonDrawer polygonDrawer;
        PolygonCollider2D polygonCollider;
        Vector2 posCurrent;
        int indexPointHold;

        List<Vector2> points
        {
            get => polygonDrawer.points;
            set => polygonDrawer.points = value;
        }

        private void Awake ()
        {
            polygonDrawer = target as PolygonDrawer;
            polygonCollider = polygonDrawer.GetComponent<PolygonCollider2D> ();
        }

        private void OnEnable ()
        {
            points = new List<Vector2> (polygonCollider.points);
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            if (GUILayout.Button ("Clear"))
            {
                Undo.RecordObject (target, "Add a point");
                points.Clear ();
            }

            if (GUILayout.Button ("Simply"))
            {
                Undo.RecordObject (target, "Symply points");

                float max = .5f;
                for (int i = 1; i < points.Count; i++)
                {
                    var d = Vector2.Distance (points [i - 1], points [i]);
                    if (d < max)
                    {
                        points.RemoveAt (i);
                        i--;
                    }
                }

                polygonCollider.points = points.ToArray ();
                points.Clear ();
            }

            if (GUILayout.Button ("Create"))
            {
                polygonCollider.points = points.ToArray ();
                polygonCollider.offset = polygonCollider.transform.position * -1f;
                points.Clear ();
            }
        }

        public void OnSceneGUI ()
        {
            HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));

            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0)
                    DrawPoint (points [i], Color.cyan);
                else if (i == points.Count - 1)
                    DrawPoint (points [i], Color.red);
                else
                    DrawPoint (points [i], Color.green);

                if (points.Count > 1 && i < points.Count - 1)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine (points [i], points [i + 1]);
                }
            }

            if (IsMouseDown (0))
            {
                indexPointHold = -1;
            }

            if (Event.current.control)
            {
                if (IsMouseDown (0))
                {
                    posCurrent = mousePos;
                    var index = points.FindIndex (x => Vector2.Distance (x, posCurrent) < distanceFlag);
                    if (index != -1)
                    {
                        Undo.RecordObject (target, "Change a point");
                        points.RemoveAt (index);
                    }
                }
            }
            else if (Event.current.shift)
            {
                if (IsMouseDown (0))
                {
                    posCurrent = mousePos;
                    var index = points.FindIndex (x => Vector2.Distance (x, posCurrent) < distanceFlag);
                    if (index != -1)
                    {
                        Undo.RecordObject (target, "Change a point");
                        points.Insert (index, posCurrent);
                        indexPointHold = index;
                    }
                }
            }
            else
            {
                if (IsMouseDown (0))
                {
                    posCurrent = mousePos;
                    indexPointHold = points.FindIndex (x => Vector2.Distance (x, posCurrent) < distanceFlag);
                    if (indexPointHold == -1)
                        CreatePoint (posCurrent);
                }
                else if (IsMouse (0))
                {
                    if (indexPointHold == -1)
                    {
                        if (Vector2.Distance (posCurrent, mousePos) > distanceFlag)
                        {
                            posCurrent = mousePos;
                            CreatePoint (posCurrent);
                        }
                    }
                }
            }

            if (IsMouse (0))
            {
                if (indexPointHold != -1)
                {
                    posCurrent = mousePos;
                    Undo.RecordObject (target, "Change a point");
                    points [indexPointHold] = posCurrent;
                }
            }

            if (Event.current.type == EventType.KeyDown && Event.current.control && Event.current.keyCode == KeyCode.Backspace)
            {
                Undo.RecordObject (target, "Add a point");
                points.Clear ();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.control && Event.current.keyCode == KeyCode.Return)
            {
                polygonCollider.points = points.ToArray ();
                points.Clear ();
            }

            SceneView.RepaintAll ();
        }

        private void DrawPoint (Vector2 point, Color c)
        {
            Handles.color = c;
            Handles.SphereHandleCap (
                0,
                point,
                Quaternion.Euler (0, 0, 0),
                pointSize,
                EventType.Repaint
            );
        }

        private void CreatePoint (Vector3 position)
        {
            Undo.RecordObject (target, "Add a point");
            points.Add (position);
        }

        bool IsMouseDown (int mouseIndex)
        {
            return Event.current.type == EventType.MouseDown && Event.current.button == mouseIndex;
        }

        bool IsMouseUp (int mouseIndex)
        {
            return Event.current.type == EventType.MouseUp && Event.current.button == mouseIndex;
        }

        bool IsMouse (int mouseIndex)
        {
            return Event.current.type == EventType.MouseDrag && Event.current.button == mouseIndex;
        }
    }
}