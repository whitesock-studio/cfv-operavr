using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(SphereCollider))]
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private SphereCollider _collider;

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
            WorldData.SaveVariable(CheckpointManager.Instance.GetPositionKey(), transform.position, 
                _ => Debug.Log($"Checkpoint saved"));
        }
    }
}
