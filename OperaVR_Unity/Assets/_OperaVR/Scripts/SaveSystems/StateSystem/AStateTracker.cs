using UnityEngine;

namespace OperaVR
{
    public abstract class AStateTracker : MonoBehaviour
    {
        public string Id = string.Empty;
        public abstract bool IsDirty { get; }
        protected virtual string GetVariableKey(string sceneKey) => sceneKey + "_State_" + Id.Substring(0, 12);
        
        public abstract void Save(string sceneKey);
        public abstract void Load(string sceneKey);

        protected bool TryGetSceneKey(out string sceneKey)
        {
            if (!StateSystem.Instance)
            {
                sceneKey = string.Empty;
                return false;
            }

            sceneKey = StateSystem.Instance.Settings.SceneKey;
            return true;
        }
        
        protected void QuickSave()
        {
            if (!TryGetSceneKey(out var sceneKey))
            {
                return;
            }
            Save(sceneKey);
        }
    }
}
