using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class Hotspot360 : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private SpaceObjectSnapper _snapper;

        [SerializeField]
        private GameObject _invertedSphere;

        [SerializeField]
        private SpatialInteractable _activationInteractable;
        
        [SerializeField]
        private SpatialInteractable _deactivationInteractable;

        [SerializeField]
        private MovementTools _movementTools;
        
        [SerializeField]
        private CameraTools _cameraTools;

        [SerializeField]
        private Transform _spawnPoint;

        private Vector3 _previousActorPosition;
        private Quaternion _previousActorRotation;

        private void Start()
        {
            Deactivate(false);
        }

        public void Activate()
        {
            _invertedSphere.SetActive(true);
            _camera.gameObject.SetActive(true);
            _snapper.gameObject.SetActive(true);

            _previousActorPosition = SpatialBridge.actorService.localActor.avatar.position;
            _previousActorRotation = SpatialBridge.actorService.localActor.avatar.rotation;
            SpatialBridge.actorService.localActor.avatar.position = _spawnPoint.position;
            SpatialBridge.actorService.localActor.avatar.rotation = _spawnPoint.rotation;

            _movementTools.DeactivatePlayerInputsExceptLookAndAction();
            _cameraTools.ActivateFirstPerson();

            _activationInteractable.gameObject.SetActive(false);
            _deactivationInteractable.gameObject.SetActive(true);
        }

        public void Deactivate(bool teleport = true)
        {
            if (teleport)
            {
                SpatialBridge.actorService.localActor.avatar.position = _previousActorPosition;
                SpatialBridge.actorService.localActor.avatar.rotation = _previousActorRotation;
            }

            _movementTools.ActivatePlayerInputs();
            _cameraTools.DeactivateFirstPerson();

            _invertedSphere.SetActive(false);
            _camera.gameObject.SetActive(false);
            _snapper.gameObject.SetActive(false);

            _activationInteractable.gameObject.SetActive(true);
            _deactivationInteractable.gameObject.SetActive(false);
        }
    }
}
