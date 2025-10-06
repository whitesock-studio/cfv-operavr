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

            var offset = SpatialBridge.cameraService.forward * _distance;
            transform.position = SpatialBridge.cameraService.position + offset;
        }
    }
}
