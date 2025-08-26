using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OperaVR
{
    public class NpcPathStarter : MonoBehaviour
    {
        [Header("Default run speed is 5.13")]
        public float RunningSpeed = 5.13f;

        [Header("Default walk speed is 3.20")]
        public float WalkingSpeed = 3.20f;

        [SerializeField]
        private NPC _npc;

        [SerializeField]
        private Path _path;

        [SerializeField]
        private bool _activateOnStart;

        [Header("Triggered when the path is complete\n(only for loop == false paths)")]
        public UnityEvent OnComplete;

        [Header("Triggered every time the path loops\n(only for loop == true paths)")]
        public UnityEvent OnLoop;

        private bool _isOn;
        private int _currentIndex = 0;

        public void Activate()
        {
            if (_npc == null || _path == null)
            {
                return;
            }
            StartPath();
        }

        private IEnumerator Start()
        {
            if (!_activateOnStart)
            {
                yield break;
            }
            while (!_npc.HasAvatar)
            {
                yield return null;
            }
            Activate();
        }

        private void Update()
        {
            if (!_isOn)
            {
                return;
            }
            FollowPath();
        }

        private void StartPath()
        {
            _isOn = true;
            _currentIndex = 0;
            _npc.SetDestination(_path.Points[_currentIndex].position);
            _npc.SetSpeeds(RunningSpeed, WalkingSpeed);
        }

        private void FollowPath()
        {
            if (!_npc.HasReachedDestination)
            {
                return;
            }

            _currentIndex++;
            
            if (_currentIndex < _path.Points.Length)
            {
                _npc.SetDestination(_path.Points[_currentIndex].position);
                return;
            }
            if (_path.Loops)
            {
                _currentIndex %= _path.Points.Length;
                _npc.SetDestination(_path.Points[_currentIndex].position);
                OnLoop?.Invoke();
                return;
            }
            OnComplete?.Invoke();
            _isOn = false;
        }
    }
}
