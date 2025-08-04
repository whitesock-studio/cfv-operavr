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
    }
}
