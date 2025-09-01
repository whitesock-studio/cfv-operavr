using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class SpaceObjectSnapper : MonoBehaviour
    {
        private void OnEnable()
        {
            SpatialBridge.spaceContentService.onObjectSpawned += OnObjectSpawned;
        }

        private void OnDisable()
        {
            SpatialBridge.spaceContentService.onObjectSpawned -= OnObjectSpawned;
        }

        private void OnObjectSpawned(IReadOnlySpaceObject readOnlySpaceObject)
        {
            TryDespawnPreviousObject();

            if (!isActiveAndEnabled)
                return;

            StartCoroutine(SnapObjectNextFrame(readOnlySpaceObject));
        }

        private IEnumerator SnapObjectNextFrame(IReadOnlySpaceObject readOnlySpaceObject)
        {
            yield return null;

            var spaceObject = (ISpaceObject)readOnlySpaceObject;
            spaceObject.position = transform.position;
            spaceObject.rotation = transform.rotation;
            spaceObject.scale = transform.localScale;
        }

        private void TryDespawnPreviousObject()
        {
            foreach (var spaceObject in SpatialBridge.spaceContentService.allObjects.Values)
            {
                if (Vector3.Distance(transform.position, spaceObject.position) > .1f)
                {
                    continue;
                }
                SpatialBridge.spaceContentService.DestroySpaceObject(spaceObject.objectID);
            }
        }
    }
}
