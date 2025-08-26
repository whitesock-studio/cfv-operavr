using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private string _assetId = "digi_avatar";

        public bool HasAvatar => _avatar != null;

        public bool HasReachedDestination => Vector3.Distance(_avatar.position, _destination) <= .1f;

        private IAvatar _avatar;
        private Vector3 _destination;

        private IEnumerator Start()
        {
            while (!SpatialBridge.actorService.localActor.avatar.isBodyLoaded)
            {
                yield return null;
            }

            yield return new WaitForSeconds(.5f);

            var request = SpatialBridge.spaceContentService.SpawnAvatar(
                AssetType.EmbeddedAsset, _assetId, transform.position, transform.rotation, _name);
            request.completed += (op) => OnAvatarSpawned(op, request);
        }

        private void FixedUpdate()
        {
            if (!HasAvatar)
            {
                return;
            }
            transform.position = _avatar.position;
            transform.rotation = _avatar.rotation;
        }

        public void SetSpeeds(float runningSpeed, float walkingSpeed)
        {
            _avatar.runSpeed = runningSpeed;
            _avatar.walkSpeed = walkingSpeed;
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
            _avatar.SetDestination(destination, false);
        }

        private void OnAvatarSpawned(SpatialAsyncOperation op, SpawnAvatarRequest request)
        {
            _avatar = request.avatar;
        }
    }
}
