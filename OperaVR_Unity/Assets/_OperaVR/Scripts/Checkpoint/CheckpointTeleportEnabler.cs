using UnityEngine;

namespace OperaVR
{
    public class CheckpointTeleportEnabler : MonoBehaviour
    {
        [SerializeField]
        private bool _setForCustomScene = true;
        
        [SerializeField, Tooltip("Only matters when _setForCustomScene is true")]
        private string _sceneKey = "SceneKey";
        
        public void SetTeleportEnabled(bool value)
        {
            var key = _setForCustomScene
                ? CheckpointManager.Instance.GetDoesTeleportKey(_sceneKey)
                : CheckpointManager.Instance.GetDoesTeleportKey();
            
            WorldData.SaveVariable(key, value, 
                _ => Debug.Log($"Teleporter " + (value ? "enabled" : "disabled")));
        }
    }
}
