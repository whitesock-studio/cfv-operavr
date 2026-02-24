using UnityEngine;

namespace OperaVR
{
    public abstract class AStateTracker : MonoBehaviour
    {
        protected string Id;
        protected virtual string GetVariableKey(string sceneKey) => sceneKey + "_State_" + Id;
        
        public abstract void Save(string sceneKey);
        public abstract void Load(string sceneKey);
    }
}
