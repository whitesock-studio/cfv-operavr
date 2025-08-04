using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(SphereCollider))]
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private int _id = 0;
        public int Id => _id;

        [SerializeField]
        private SphereCollider _collider;

        private const string VARIABLE_KEY = "Checkpoint";

        private void OnDrawGizmos()
        {
            if (!_collider)
            {
                return;
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _collider.radius);
        }

        public void RegisterToCheckpoint()
        {
            WorldData.SaveVariable(VARIABLE_KEY, _id, 
                (_) => Debug.Log($"Checkpoint {_id}"));
        }
    }
}
