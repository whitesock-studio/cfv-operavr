using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class AvatarsController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                SetRemoteAvatarState(true);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                SetRemoteAvatarState(false);
            }
        }
        
        public void SetRemoteAvatarState(bool visible)
        {
            var allAvatars = SpatialBridge.spaceContentService.avatars;
            // Change visibility of other avatars on my client
            foreach (var pair in allAvatars)
            {
                var readOnlyAvatar = pair.Value;
                if (readOnlyAvatar.spaceObject == SpatialBridge.actorService.localActor.avatar.spaceObject)
                {
                    Debug.LogError("FOUND LOCAL ACTOR AVATAR");
                    continue;
                }

                if (readOnlyAvatar.spaceObject.isMine)
                {
                    Debug.LogError("FOUND AVATAR MINE");
                    continue;
                }

                Debug.LogError("FOUND AVATAR NOT MINE");
                if (!readOnlyAvatar.spaceObject.canTakeOwnership)
                {
                    Debug.LogError("CANT PASS OWNERSHIP");
                    continue;
                }
                var previousOwner = readOnlyAvatar.spaceObject.ownerActorNumber;
                SpatialBridge.spaceContentService.TakeOwnership(readOnlyAvatar.spaceObject.objectID);
                
                Debug.LogError("OWNERSHIP PASSED, editing");
                var editableAvatar = pair.Value as IAvatar;
                editableAvatar.visibleRemotely = visible;
                
                if (!readOnlyAvatar.spaceObject.canTakeOwnership)
                {
                    continue;
                }
                
                SpatialBridge.spaceContentService.TransferOwnership(
                    readOnlyAvatar.spaceObject.objectID, previousOwner);
            }
        }
    }
}
