namespace OperaVR
{
    using UnityEngine;
    using SpatialSys.UnitySDK;

    public class CameraTools : MonoBehaviour
    {
        public void ActivateFirstPerson()
        {
			SpatialBridge.cameraService.forceFirstPerson = true;
		}

        public void DeactivateFirstPerson()
        {
			SpatialBridge.cameraService.forceFirstPerson = false;
		}
    }
}