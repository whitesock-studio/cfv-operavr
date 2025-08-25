using UnityEngine;

namespace OperaVR
{
    public class Path : MonoBehaviour
    {
        public Transform[] Points;

        [SerializeField]
        private Color _color;

        [SerializeField]
        private bool _loops;
        public bool Loops => _loops;

        private void OnDrawGizmos()
        {
            if (Points.Length < 2)
            {
                return;
            }
            Gizmos.color = _color;
            Gizmos.DrawWireCube(Points[0].position + Vector3.up, new Vector3(.3f, 2f, .3f));
            for (int i = 0; i < Points.Length - 1; i++)
            {
                var point = Points[i];
                Gizmos.DrawLine(point.position, Points[i + 1].position);
            }
            if (_loops)
            {
                Gizmos.DrawLine(Points[0].position, Points[^1].position);
            }
        }
    }
}
