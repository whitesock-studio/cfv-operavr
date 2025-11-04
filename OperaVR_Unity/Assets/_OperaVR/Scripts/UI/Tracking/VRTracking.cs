using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class VRTracking : MonoBehaviour
    {
        [SerializeField]
        private float _distance = 2f;

        void Update()
        {
            if (SpatialBridge.actorService.localActor.platform != SpatialPlatform.MetaQuest)
            {
                return;
            }

            var forward = Vector3.ProjectOnPlane(SpatialBridge.cameraService.forward, 
                transform.up).normalized;
            var offset = forward * _distance;
            transform.position = Vector3.Lerp(transform.position, 
                SpatialBridge.cameraService.position + offset, Time.deltaTime * 5f);
        }
    }
}
