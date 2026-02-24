using SpatialSys.UnitySDK;
using System;

namespace OperaVR
{
    public static class WorldData
    {
        public static void TryGetVariable(string key, 
            Action<DataStoreGetVariableRequest> callbackPostGet)
        {
            SpatialBridge.userWorldDataStoreService.
                GetVariable(key, null).SetCompletedEvent(callbackPostGet);
        }

        public static void SaveVariable(string key, object value, 
            Action<DataStoreOperationRequest> callbackPostSet)
        {
            SpatialBridge.userWorldDataStoreService.SetVariable(key, value).
                SetCompletedEvent(callbackPostSet);
        }
        
        public static void HasVariable(string key, 
            Action<DataStoreHasVariableRequest> callbackPostSet)
        {
            SpatialBridge.userWorldDataStoreService.HasVariable(key).
                SetCompletedEvent(callbackPostSet);
        }

        public static void ClearAllVariables()
        {
            SpatialBridge.userWorldDataStoreService.ClearAllVariables();
        }
    }
}
