using UnityEngine;

namespace OperaVR
{
    public class NpcPathStarter : MonoBehaviour
    {
        [SerializeField]
        private NPC _npc;

        [SerializeField]
        private Path _path;

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
                return;
            }
            _isOn = false;
        }
    }
}
