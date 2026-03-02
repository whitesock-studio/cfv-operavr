using UnityEngine;

namespace OperaVR
{
    public class TargetTracker : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _trackingSpeed = 5f;

        private void Update()
        {
            var targetDirection = Quaternion.LookRotation(
                _target.position - transform.position, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetDirection, 
                Mathf.Clamp01(_trackingSpeed * Time.deltaTime));
        }
    }
}
