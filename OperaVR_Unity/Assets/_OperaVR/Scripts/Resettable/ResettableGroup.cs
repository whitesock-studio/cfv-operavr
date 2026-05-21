using UnityEngine;

namespace OperaVR
{
    public class ResettableGroup : MonoBehaviour
    {
        private IResettable[] _resettables;
        
        private void Awake()
        {
            _resettables = GetComponentsInChildren<IResettable>(true);
        }

        public void Reset()
        {
            foreach (var resettable in _resettables)
            {
                resettable.Reset();
            }
        }
    }
}
