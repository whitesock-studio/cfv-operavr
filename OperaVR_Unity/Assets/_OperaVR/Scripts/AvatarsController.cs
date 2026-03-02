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
                ToggleRemoteVisibility();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ToggleLocalVisibility();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ToggleLocalVisibilityOtherAvatars();
            }
        }

        public void ToggleRemoteVisibility()
        {
            Debug.LogError("Toggle remote vis");
            // Change visibility on my avatar to everyone's client
            SpatialBridge.actorService.localActor.avatar.visibleRemotely = !SpatialBridge.actorService.localActor.avatar.visibleRemotely;
        }

        public void ToggleLocalVisibility()
        {
            Debug.LogError("Toggle local vis");
            // Change visibility of my avatar only on my client
            SpatialBridge.actorService.localActor.avatar.visibleLocally = !SpatialBridge.actorService.localActor.avatar.visibleLocally;
        }

        public void ToggleLocalVisibilityOtherAvatars()
        {
            Debug.LogError("Toggle other local vis");
            // Change visibility of other avatars on my client
            foreach (var actor in SpatialBridge.actorService.actors.Values)
            {
                if (actor != SpatialBridge.actorService.localActor)
                {
                    actor.avatar.visibleLocally = !actor.avatar.visibleLocally;
                }
            }
        }
    }
}
