using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class ResettableObject : MonoBehaviour, IResettable
    {
        private Animator _animator;
        private Vector3 _startingPosition;
        private Quaternion _startingRotation;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _startingPosition = transform.position;
            _startingRotation = transform.rotation;
        }

        public void Reset()
        {
            if (_animator != null)
            {
                _animator.enabled = false;
                _animator.enabled = true;
                _animator.Rebind();
            }
            transform.position = _startingPosition;
            transform.rotation = _startingRotation;
        }
    }
}
