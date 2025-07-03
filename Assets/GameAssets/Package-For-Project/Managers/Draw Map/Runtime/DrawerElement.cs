using UnityEngine;

namespace mapdrawer
{
    public class DrawerElement : MonoBehaviour
    {
        [SerializeField] private bool showGizmos;

        public float size;

        private void OnDrawGizmosSelected ()
        {
            if (!showGizmos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube (transform.position, Vector3.one * size);
            }
        }

        private void OnDrawGizmos ()
        {
            if (showGizmos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube (transform.position, Vector3.one * size);
            }
        }
    }
}