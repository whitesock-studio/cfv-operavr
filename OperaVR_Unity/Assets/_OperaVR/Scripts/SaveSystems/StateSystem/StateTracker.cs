using SpatialSys.UnitySDK;

namespace OperaVR
{
    public class StateTracker : AStateTracker
    {
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_IsActive";
        
        public override void Save(string sceneKey)
        {
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
