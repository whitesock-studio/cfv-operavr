using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class InputsManager : MonoBehaviour
    {
        public static InputsManager Instance;

        private BlockAvatarInput _blockAvatarInput;
        private IAvatarInputActionsListener _currentInput;

        private void Awake()
        {
            //Singleton Pattern
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            var blockInputObject = new GameObject("BlockInputs", typeof(BlockAvatarInput));
            _blockAvatarInput = blockInputObject.GetComponent<BlockAvatarInput>();
        }

        public void BlockInputs()
        {
            SpatialBridge.inputService.StartCompleteCustomInputCapture(_blockAvatarInput);
            _currentInput = _blockAvatarInput;
        }

        public void RestoreInputs()
        {
            SpatialBridge.inputService.ReleaseInputCapture(_currentInput);
            _currentInput = null;
        }
    }
}
