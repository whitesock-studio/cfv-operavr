using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance;

        [SerializeField]
        private SaveSystemsSettings _settings;
        private string DefaultPositionKey => _settings.SceneKey + "_Position";
        private string DefaultDoesTeleportKey => _settings.SceneKey + "_DoesTeleport";

        public string GetPositionKey() => DefaultPositionKey;
        public string GetDoesTeleportKey() => DefaultDoesTeleportKey;
        public string GetPositionKey(string sceneKey) => sceneKey + "_Position";
        public string GetDoesTeleportKey(string sceneKey) => sceneKey + "_DoesTeleport";

        private float _nextSaveTime;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _nextSaveTime = Time.time + _settings.StartDelay;
        }

        private void Start()
        {
            WorldData.HasVariable(GetDoesTeleportKey(), 
                response => StartCoroutine(OnCheckTeleportResponse(response)));
            return;
            
            IEnumerator OnCheckTeleportResponse(DataStoreHasVariableRequest response)
            {
                yield return new WaitForSeconds(.5f); 

                if (!response.hasVariable)
                {
                    StartCoroutine(WaitForActorAndTeleport());
                    yield break;
                }
                WorldData.TryGetVariable(GetDoesTeleportKey(), 
                    callback => StartCoroutine(OnDoesTeleportResponse(callback)));
                yield break;

                IEnumerator OnDoesTeleportResponse(DataStoreGetVariableRequest response)
                {
                    if (!response.boolValue)
                    {
                        yield break;
                    }

                    StartCoroutine(WaitForActorAndTeleport());
                }

                IEnumerator WaitForActorAndTeleport()
                {
                    while (!SpatialBridge.actorService.localActor.avatar.isBodyLoaded)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(.5f); 

                    TeleportToLastCheckpoint();
                }
            }
        }

        private void TeleportToLastCheckpoint()
        {
            Debug.LogError("Loading " + GetPositionKey());
            WorldData.TryGetVariable(GetPositionKey(), OnPositionVariableResponse);
        }

        private void OnPositionVariableResponse(DataStoreGetVariableRequest response)
        {
            Debug.LogError("Loading " + GetPositionKey() + ", response = " + response.vector3Value);

            var localAvatar = SpatialBridge.actorService.localActor.avatar;
            localAvatar.position = response.vector3Value;
        }
        
        private void Update()
        {
            if (Time.time < _nextSaveTime)
            {
                return;
            }

            _nextSaveTime = Time.time + _settings.TimeBetweenSaves;
            var localActor = SpatialBridge.actorService.localActor;
            Debug.LogError("Saving " + GetPositionKey() + ", position = " + localActor.avatar.position);
            WorldData.SaveVariable(GetPositionKey(), localActor.avatar.position, 
                _ => Debug.Log($"Auto position save"));
        }
    }
}
