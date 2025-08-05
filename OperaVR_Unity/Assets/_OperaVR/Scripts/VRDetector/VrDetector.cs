using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    public class VrDetector : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _debugText;

        private void Update()
        {
            _debugText.text = SpatialBridge.actorService.localActor.platform.ToString();
        }
    }
}
