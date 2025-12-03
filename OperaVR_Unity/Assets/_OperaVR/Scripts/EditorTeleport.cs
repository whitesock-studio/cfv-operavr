using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    public class EditorTeleport : MonoBehaviour
    {
#if UNITY_EDITOR
        private float InteractiveRadius => 1f;
        private SpatialAvatarTeleporter _teleporter;
        private IAvatar _avatar;
        
        private IAvatar Avatar
        {
            get
            {
                _avatar ??= SpatialBridge.actorService.localActor.avatar;
                return _avatar;
            }   
        }
        
        private void Awake()
        {
            _teleporter = GetComponent<SpatialAvatarTeleporter>();
        }

        private void LateUpdate()
        {
            if (Avatar == null)
            {
                return;
            }

            if (Vector3.Distance(Avatar.position, transform.position) > InteractiveRadius)
            {
                return;
            }

            Interact(); 
        }

        private void Interact()
        {
            Avatar.position = _teleporter.targetLocation.position;
            Avatar.rotation = _teleporter.targetLocation.rotation;
        }

#endif
    }
}
