using SpatialSys.UnitySDK;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class CheckpointTeleporter : MonoBehaviour
    {
        [SerializeField]
        private bool _teleportAtStart = false;

        private const string VARIABLE_KEY = "Checkpoint";

        private IEnumerator Start()
        {
            while (!SpatialBridge.actorService.localActor.avatar.isBodyLoaded)
            {
                yield return null;
            }

            yield return new WaitForSeconds(.5f); 

            TeleportToLastCheckpoint();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                TeleportToLastCheckpoint();
            }
        }

        private void TeleportToLastCheckpoint()
        {
            WorldData.TryGetVariable(VARIABLE_KEY, OnVariableResponse);
        }

        private void OnVariableResponse(DataStoreGetVariableRequest response)
        {
            if (!TryFindCheckpoint(response.intValue, out var checkpoint))
            {
                Debug.LogError("CHECKPOINT: No checkpoint in data");
                return;
            }
            Debug.LogError("CHECKPOINT: Checkpoint data found");
            var localAvatar = SpatialBridge.actorService.localActor.avatar;
            localAvatar.position = checkpoint.transform.position;
        }

        private bool TryFindCheckpoint(int id, out Checkpoint checkpoint)
        {
            var checkpoints = FindObjectsOfType<Checkpoint>(); 
            checkpoint = checkpoints.FirstOrDefault(c => c.Id == id);
            return checkpoint != null;
        }
    }
}
