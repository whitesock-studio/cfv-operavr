using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class InputsManager : MonoBehaviour
    {
        public static InputsManager Instance;

        private BlockAvatarInput _blockAvatarInput;
        private IAvatarInputActionsListener _currentInput;

        private bool _isMovementLocked;
        private Vector3 _lockedPosition;

        private void Awake()
        {
            //Singleton Pattern
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            _isMovementLocked = false;
            var blockInputObject = new GameObject("BlockInputs", typeof(BlockAvatarInput));
            _blockAvatarInput = blockInputObject.GetComponent<BlockAvatarInput>();
        }

        private void Update()
        {
            if (!_isMovementLocked)
            {
                return;
            }
            SpatialBridge.actorService.localActor.avatar.position = _lockedPosition;
        }

        public void RestoreInputs()
        {
            if (_currentInput == null)
            {
                SpatialBridge.inputService.StartAvatarInputCapture(false, false, false, false, null);
                return;
            }
            SpatialBridge.inputService.ReleaseInputCapture(_currentInput);
            _currentInput = null;
            _isMovementLocked = false;
        }

        public void BlockInputs()
        {
            if (SpatialBridge.actorService.localActor.platform != SpatialPlatform.MetaQuest)
            {
                SpatialBridge.inputService.StartCompleteCustomInputCapture(_blockAvatarInput);
                _currentInput = _blockAvatarInput;
            }
            _lockedPosition = SpatialBridge.actorService.localActor.avatar.position;
            _isMovementLocked = true;
        }

        public void BlockInputsExceptLookAndActions()
        {
            if (SpatialBridge.actorService.localActor.platform != SpatialPlatform.MetaQuest)
            {
                SpatialBridge.inputService.StartAvatarInputCapture(true, true, true, false, _blockAvatarInput);
                _currentInput = _blockAvatarInput;
            }
            _lockedPosition = SpatialBridge.actorService.localActor.avatar.position;
            _isMovementLocked = true;
        }
    }
}
