using System;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(Animator))]
    public class NPCCharacter : MonoBehaviour
    {
        private float _speed;
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        public bool HasReachedDestination => Vector3.Distance(transform.position, _destination) <= 1f;

        [SerializeField]
        private TMP_Text _nameText;

        [SerializeField]
        private float _stopDistance = .5f;
        
        private Vector3 _destination;
        private bool _isMoving;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var movement = _destination - transform.position;
            var distanceToDestination = movement.magnitude;

            if (distanceToDestination < _stopDistance)
            {
                return;
            }
            
            var direction = movement.normalized;
            var directionNoZ = direction;
            directionNoZ.y = 0;
            directionNoZ.Normalize();
            var frameSpeed = Speed * Time.deltaTime;
            
            if (distanceToDestination < frameSpeed)
            {
                transform.position = _destination;
                transform.rotation = Quaternion.LookRotation(directionNoZ);
                _isMoving = false;
                return;
            }

            transform.position += direction * frameSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionNoZ), 
                Time.deltaTime * 10f);
        }

        public void SetName(string name)
        {
            _nameText.text = name;
        }
        
        public void SetDestination(Vector3 destination)
        {
            _isMoving = true;
            _destination = destination;
        }
        
        public void SetEmote(int emoteKey)
        {
            _animator.SetInteger("Emote", emoteKey);
        }
    }
}
