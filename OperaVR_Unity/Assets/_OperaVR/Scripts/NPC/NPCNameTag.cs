using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class NPCNameTag : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _minMaxAlphaDistance;

        [SerializeField]
        private Vector2 _minMaxAlpha = Vector2.one;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private void Update()
        {
            var localActor = SpatialBridge.actorService.localActor;
            if (localActor == null)
            {
                return;
            }

            var distance = Vector3.Distance(localActor.avatar.position, transform.position);
            var normalizedDistance = Mathf.Clamp01(
                Mathf.InverseLerp(_minMaxAlphaDistance.x, _minMaxAlphaDistance.y, distance));
            _canvasGroup.alpha = Mathf.InverseLerp(_minMaxAlpha.x, _minMaxAlpha.y, normalizedDistance);
        }
    }
}
