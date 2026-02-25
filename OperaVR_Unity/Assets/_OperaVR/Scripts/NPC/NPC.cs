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

        [SerializeField, Range(0, 359f)]
        private float _startingAngle;

        public bool HasAvatar => _avatar != null;

        public bool HasReachedDestination => Vector3.Distance(_avatar.position, _destination) <= 1f;

        private IAvatar _avatar;
        public IAvatar Avatar => _avatar;
        private Vector3 _destination;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(.3f, 2f, .3f));
            var forwardPoint = Quaternion.AngleAxis(_startingAngle, Vector3.up) * Vector3.forward;
            var lookingPoint = transform.position + forwardPoint;
            Gizmos.DrawLine(transform.position, lookingPoint);
            Gizmos.DrawWireCube(lookingPoint + Vector3.up * .3f, new Vector3(.3f, .6f, .3f));
        }

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

        public void Sit(bool isSit)
        {
            if (isSit)
            {
                StartCoroutine(DelayedSit());       
                return;
            }
            _avatar.Stand();
        }
        
        private IEnumerator DelayedSit()
        {
            yield return new WaitForSeconds(1);
            _avatar.Sit(transform);      
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
            if (_avatar == null)
            {
                return;
            }

            //_avatar.visibleRemotely = false;
            _avatar.position = transform.position;
            var forwardPoint = Quaternion.AngleAxis(_startingAngle, Vector3.up) * Vector3.forward * .15f;
            var lookingPoint = transform.position + forwardPoint;
            _avatar.SetDestination(lookingPoint);
        }
    }
}
