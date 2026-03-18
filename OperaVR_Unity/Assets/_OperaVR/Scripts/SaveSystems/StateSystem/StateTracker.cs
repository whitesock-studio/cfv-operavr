using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class StateTracker : AStateTracker
    {
        private bool _isDirty;
        public override bool IsDirty => _isDirty;
        
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_IsActive";

        private void OnEnable()
        {
            _isDirty = true;
        }

        private void OnDisable()
        {
            _isDirty = true;
        }
        
        public override void Save(string sceneKey)
        {
            _isDirty = false;
            WorldData.SaveVariable(GetVariableKey(sceneKey), gameObject.activeSelf, 
                _ => { });
        }

        public override void Load(string sceneKey)
        {
            WorldData.HasVariable(GetVariableKey(sceneKey), HasVariableCallback);
            return;
            
            void HasVariableCallback(DataStoreHasVariableRequest request)
            {
                if (!request.hasVariable)
                {
                    return;
                }
                WorldData.TryGetVariable(GetVariableKey(sceneKey), GetVariableCallback);
            }

            void GetVariableCallback(DataStoreGetVariableRequest request)
            {
                gameObject.SetActive(request.boolValue);
            }
        }
    }
}
