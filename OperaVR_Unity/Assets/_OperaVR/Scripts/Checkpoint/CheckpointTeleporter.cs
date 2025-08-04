using SpatialSys.UnitySDK;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class CheckpointTeleporter : MonoBehaviour
    {
        private const string VARIABLE_KEY = "Checkpoint";

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
                Debug.Log("No checkpoint in data");
                return;
            }
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
