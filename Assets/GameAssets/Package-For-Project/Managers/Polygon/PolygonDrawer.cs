using System.Collections.Generic;
using UnityEngine;

namespace ProjectTools
{
    public class PolygonDrawer : MonoBehaviour
    {
        [HideInInspector] public List<Vector2> points = new List<Vector2> ();

        Grid grid;
        public Grid Grid
        {
            get
            {
                if (!grid)
                    grid = GetComponentInParent<Grid> ();

                return grid;
            }
        }
    }
}