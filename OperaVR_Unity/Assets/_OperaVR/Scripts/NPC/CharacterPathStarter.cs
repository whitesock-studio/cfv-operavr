using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class CharacterPathStarter : MonoBehaviour
    {
        [SerializeField]
        private Transform _character;

        [SerializeField]
        private Path _path;

        [SerializeField, Range(0f, 1f)]
        private float _animationSpeed = 1f;
        
        [SerializeField]
        private float _movementSpeed = 3f;
        
        [SerializeField]
        private float _changeSpeedTime = .3f;
        
        private int _currentPathIndex = -1;
        
        private const float STOP_DISTANCE = 0.1f;
        
        private void Update()
        {
            if (_currentPathIndex == -1)
            {
                return;
            }
            var destination = _path.Points[_currentPathIndex].position;
            var movement = destination - _character.position;
            var distanceToDestination = movement.magnitude;

            if (distanceToDestination < STOP_DISTANCE)
            {
                Progress();
                return;
            }
            
            var direction = movement.normalized;
            var directionNoZ = direction;
            directionNoZ.y = 0;
            directionNoZ.Normalize();
            var frameSpeed = _movementSpeed * Time.deltaTime;
            
            if (distanceToDestination < frameSpeed)
            {
                _character.position = destination;
                _character.rotation = Quaternion.LookRotation(directionNoZ);
                return;
            }

            _character.position += direction * frameSpeed;
            _character.rotation = Quaternion.Lerp(_character.rotation, 
                Quaternion.LookRotation(directionNoZ), Time.deltaTime * 10f);
        }

        private void Progress()
        {
            _currentPathIndex++;
            if (_currentPathIndex >= _path.Points.Length)
            {
                _currentPathIndex = _path.Loops ? 0 : -1;
                if (_currentPathIndex == -1)
                {
                    StartCoroutine(SetSpeedCoroutine(
                        _character.GetComponent<Animator>(), 0f));
                }
            }
        }

        public void StartPath()
        {
            _currentPathIndex = 0;
            StartCoroutine(SetSpeedCoroutine(
                _character.GetComponent<Animator>(), _animationSpeed));
        }
        
        private IEnumerator SetSpeedCoroutine(Animator animator, float targetSpeed)
        {
            var startingSpeed = animator.GetFloat("Speed");
            var t = 0f;
            while (t < _changeSpeedTime)
            {
                t += Time.deltaTime;
                animator.SetFloat("Speed", Mathf.Lerp(startingSpeed, targetSpeed, t / _changeSpeedTime));
                yield return null;
            }
            animator.SetFloat("Speed", targetSpeed);
        }
    }
}
