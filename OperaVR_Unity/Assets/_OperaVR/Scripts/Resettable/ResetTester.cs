using UnityEngine;

namespace OperaVR
{
    public class ResetTester : MonoBehaviour
    {
        [SerializeField]
        private ResettableGroup _group;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _group.Reset();
            }
        }
    }
}
